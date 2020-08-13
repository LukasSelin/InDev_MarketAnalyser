using System;
using System.Text;
using System.Collections.Generic;


namespace AlbionMarketChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> auctionList = new List<string>();
            var endpoint = PortFinder.SelectPort();
            auctionList = PacketSniffer.ListenFromDevice(PacketSniffer.SelectDevice(), endpoint.Port);
            foreach (var auction in auctionList)
            {
                Console.WriteLine(auction);
            }
        }
    }
}