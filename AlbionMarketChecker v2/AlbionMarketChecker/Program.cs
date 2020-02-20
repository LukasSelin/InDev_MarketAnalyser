using menu;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Text;

using System.Collections.Generic;



namespace AlbionMarketChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            SelectDevice();
        }

        static void SelectDevice()
        {

            IList<LivePacketDevice> allDevcies = LivePacketDevice.AllLocalMachine;

            if (allDevcies.Count == 0)
            {
                Console.WriteLine("No interfaces found.");
            }

            string menuHeader = "Select capture device";
            List<string> menuContent = new List<string>();

            foreach (var device in allDevcies)
            {
                menuContent.Add(device.Description);
            }
            var menu = new Menu(menuContent);
            menu = menu.GetMenu(menu, menuHeader);

            PacketDevice selectedDevice = allDevcies[menu.SelectedIndex];

            using (PacketCommunicator communicator =
                    selectedDevice.Open(62388, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                using (BerkeleyPacketFilter filter = communicator.CreateFilter("ip and udp and len == 1242"))
                {
                    communicator.SetFilter(filter);
                }



                Console.WriteLine("Listening on " + selectedDevice.Description + "...");

                communicator.ReceivePackets(0, PacketHandler);
            }
        } // Select device

        static string uncompletedAuction;
        static List<string> vs = new List<string>();
        private static void PacketHandler(Packet packet)
        {
            // Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd") + " length:" + packet.Length);
            // The data payload
            // packet.Ethernet.IpV4.Udp.Payload
            var convertedString = packet.Ethernet.IpV4.Udp.Payload.Decode(Encoding.ASCII);
            bool isTrue = true;
            string auction;
            int beginingOfAuction = convertedString.IndexOf('{');
            auction = convertedString.Substring(beginingOfAuction);

            do
            {
                if(uncompletedAuction == null)
                {

                    if (auction.Contains('}'))
                    {
                       //  uncompletedAuction = null;
                        int endOfAuction = auction.IndexOf('}') + 1;
                        string completedAuction = auction.Substring(0, endOfAuction);
                        Console.WriteLine(completedAuction);
                        vs.Add(completedAuction);

                        // Send CompletedAuction to the database

                        // Get remaining data from the packet
                        auction = auction.Substring(endOfAuction);

                    }
                    else
                    {
                        uncompletedAuction = auction;
                        isTrue = false;
                        Console.WriteLine("Wohooo äntligen här!");
                    }
                }
                else
                {
                    // 44 bytes skippa i början alltid (tror jag)
                    var middleAuctionString = packet.Ethernet.IpV4.Udp.Payload.Subsegment(44, packet.Ethernet.IpV4.Udp.Payload.Length - 44);
                    var completedString = uncompletedAuction + convertedString;

                    // Send CompletedAuction to the database
                    Console.WriteLine(completedString);
                    vs.Add(completedString);
                    uncompletedAuction = null;
                }
            } while (isTrue);
        }
    }
}