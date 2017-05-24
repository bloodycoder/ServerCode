using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ShareUtil;
namespace test_server
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Log初始化
            LogConfig config = new LogConfig();
            config.logLevel = 1;
            config.isConsole = true;
            config.isFile = false;
            config.filePath = "./pig.log";
            Log.init(config);
            #endregion
            #region server初始化
            TcpServer tcpServer = new TcpServer();
            tcpServer.Start();
            #endregion
            string key = Console.ReadLine();
            while (key != "q") {
                key = Console.ReadLine();
            }
            tcpServer.Stop();
            Console.ReadKey();
        }
    }
}
