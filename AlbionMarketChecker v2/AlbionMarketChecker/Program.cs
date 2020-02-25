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
            auctionList = PacketSniffer.ListenFromDevice(PacketSniffer.SelectDevice());
            foreach (var auction in auctionList)
            {
                Console.WriteLine(auction);
            }
        }
    }
}