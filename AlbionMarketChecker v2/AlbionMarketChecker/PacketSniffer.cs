using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using menu;

namespace AlbionMarketChecker
{
    public static class PacketSniffer
    {
        public static string filterString = "ip and udp and len == 1242";
        public static LivePacketDevice SelectDevice()
        {
            IList<LivePacketDevice> allDevcies = LivePacketDevice.AllLocalMachine;

            if (allDevcies.Count == 0)
            {
                Console.WriteLine("No interfaces found.");
            }

            string menuHeader = "Select a capture device";
            List<string> menuContent = new List<string>();

            foreach (var device in allDevcies)
            {
                menuContent.Add(device.Description);
            }

            var menu = new Menu(menuContent);
            menu = menu.GetMenu(menu, menuHeader);

            return allDevcies[menu.SelectedIndex];
        } // Select device

        public static List<string> ListenFromDevice(LivePacketDevice device)
        {
            using (PacketCommunicator communicator =
                    device.Open(62388, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                using (BerkeleyPacketFilter filter = communicator.CreateFilter(filterString))
                {
                    communicator.SetFilter(filter);
                }

                Console.WriteLine("Listening on " + device.Description + "...");

                communicator.ReceivePackets(0, PacketHandler);
                return auctionList;
            }
        }

        private static string uncompletedAuction;
        private static List<string> auctionList = new List<string>();
        private static void PacketHandler(Packet packet)
        {
            var convertedString = packet.Ethernet.IpV4.Udp.Payload.Decode(Encoding.ASCII);
            bool isTrue = true;

            int beginingOfAuction = convertedString.IndexOf('{');
            string auction = convertedString.Substring(beginingOfAuction);

            do
            {
                if (uncompletedAuction == null)
                {
                    int endOfAuctionIndex = auction.IndexOf('}');
                    if (endOfAuctionIndex == -1)
                    {
                        uncompletedAuction = auction;
                        isTrue = false;
                    }
                    else
                    {
                        string completedAuction = auction.Substring(0, endOfAuctionIndex + 1);
                        auctionList.Add(completedAuction);

                        // Get remaining data from the packet
                        auction = auction.Substring(endOfAuctionIndex);
                    }
                }
                else
                {
                    var payload = packet.Ethernet.IpV4.Udp.Payload;
                    var splitAuction = payload.Subsegment(44, payload.Decode(Encoding.ASCII).IndexOf('}'));
                    var completedString = uncompletedAuction + splitAuction;

                    uncompletedAuction = null;
                    auctionList.Add(completedString);
                }
            } while (isTrue);
        }
    }
}
