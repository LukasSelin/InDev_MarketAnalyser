using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;
using menu;
using System.Linq;
using static AlbionMarketChecker.PortFinderHelper;

namespace AlbionMarketChecker
{
    class PortFinder
    {
        public static IPEndPoint SelectPort()
        {
            var endPoints = ActiveConnections();
            var menuContent = new List<string>();
            var menuContentDimension2 = new List<string>();
            var remoteEndPoint = new List<IntrestingProcess>();
            var remoteEndPointDimension2 = new List<IntrestingProcess>();

            TcpRow[] tcpRows = endPoints.Rows.ToArray();
            for (var i = 0; i < endPoints.Rows.Count(); i++)
            {
                var processId = tcpRows[i].ProcessId;
                Process process = Process.GetProcessById(processId);

                if (UniqueName(menuContent, process.ProcessName))
                {
                    menuContent.Add(process.ProcessName);
                    remoteEndPoint.Add(new IntrestingProcess()
                    {
                        ProcessId = processId,
                        ProcessName = process.ProcessName,
                        RemoteEndPoint = tcpRows[i].RemoteEndPoint
                    });
                }
                else
                {
                    menuContentDimension2.Add(process.ProcessName);
                    remoteEndPointDimension2.Add(new IntrestingProcess()
                    {
                        ProcessId = processId,
                        ProcessName = process.ProcessName,
                        RemoteEndPoint = tcpRows[i].RemoteEndPoint
                    });
                }
            }

            var menuHeader = "Select Process: ";
            var menu = new Menu(menuContent);
            menu = menu.GetMenu(menu, menuHeader);

            var endpoints = remoteEndPointDimension2.FindAll(i => i.ProcessName == menu.Items[menu.SelectedIndex]);

            if (endpoints.Count != 0)
            {
                menuContent.Clear();
                foreach (var point in endpoints)
                {
                    menuContent.Add(point.ProcessId + " " + point.ProcessName + " " + point.RemoteEndPoint);
                }

                menu = new Menu(menuContent);
                menu = menu.GetMenu(menu, menuContent[0]);
            }

            return remoteEndPoint[menu.SelectedIndex].RemoteEndPoint;
        }
        private static TcpTable ActiveConnections()
        {
            return ManagedIpHelper.GetExtendedTcpTable(true);
        }

        private static bool UniqueName(List<string> nameList, string name)
        {
            var list = nameList.FindAll(i => i == name);

            return list.Count == 0;
        }
    }
    class IntrestingProcess
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public IPEndPoint RemoteEndPoint { get; set; }
    }
}