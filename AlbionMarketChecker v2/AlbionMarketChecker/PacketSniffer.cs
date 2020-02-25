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
                    device.Open(65535, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                using (BerkeleyPacketFilter filter = communicator.CreateFilter(filterString))
                {
                    communicator.SetFilter(filter);
                }

                Console.WriteLine("Listening on " + device.Description + "...");
                Console.WriteLine("Press any key to exit...");
                bool iskeyPress = true;
                do
                {
                    communicator.ReceivePackets(0, PacketHandler);
                    
                    var keyPressed = Console.ReadKey();
                    iskeyPress = keyPressed.Key == ConsoleKey.Enter;
                } while (iskeyPress);
                return auctionList;
            }
        }

        private static string uncompletedAuction;
        private static List<string> auctionList = new List<string>();
        private static void PacketHandler(Packet packet)
        {
            bool isTrue = true;
            var payload = packet.Ethernet.IpV4.Udp.Payload;
            string auction = null;

            do
            {
                if (uncompletedAuction == null)
                {

                    var convertedString = payload.Decode(Encoding.ASCII);
                    int endOfAuctionIndex = convertedString.IndexOf('}'); ;

                    for (int i = 0; i < auctionList.Count / 3; i++)
                    {
                        auction.Substring(endOfAuctionIndex);
                        endOfAuctionIndex = convertedString.IndexOf('}');
                    }

                    int startOfAuctionIndex = convertedString.IndexOf('{');

                    if (endOfAuctionIndex == -1)
                    {
                        uncompletedAuction = auction;
                        isTrue = false;
                        Console.WriteLine("Dåre");

                    }
                    else
                    {
                        endOfAuctionIndex++;
                        auction = payload.Subsegment(startOfAuctionIndex, endOfAuctionIndex).Decode(Encoding.ASCII);

                        // Get remaining data from the packet
                        auctionList.Add(auction);
                        auction = auction.Substring(endOfAuctionIndex);

                        if (auction == "")
                        {
                            uncompletedAuction = auction;
                            isTrue = false;
                        }
                    }
                }
                else
                {
                    var splitAuction = payload.Subsegment(44, payload.Decode(Encoding.ASCII).IndexOf('}'));
                    var completedString = uncompletedAuction + splitAuction.Decode(Encoding.ASCII);
                    Console.WriteLine("Hej hej");

                    uncompletedAuction = null;
                    auctionList.Add(completedString);
                }
            } while (isTrue);
        }
    }
}
