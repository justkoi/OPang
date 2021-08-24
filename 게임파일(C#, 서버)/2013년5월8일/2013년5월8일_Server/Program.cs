using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace _2013년5월8일_Server
{
    class Program
    {

        public static Socket Server, Client;

        public static byte[] getByte = new byte[1024];
        public static byte[] setByte = new byte[1024];

        public const int sPort = 5000;

        public static int byteArrayDefrag(byte[] sData)
        {
            int endLength = 0;

            for (int i = 0; i < sData.Length; i++)
            {
                if ((byte)sData[i] != (byte)0)
                {
                    endLength = i;
                }
            }

            return endLength;
        }

        [STAThread]
        static void Main(string[] args)
        {
            string stringbyte = null;
            string strServerIP = null;

            //Console.WriteLine(">>[서버]서버IP를 입력해주십시오.");
            //strServerIP = Console.ReadLine();

            strServerIP = "192.168.43.23";
            IPAddress serverIP = IPAddress.Parse(strServerIP);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

            try
            {

                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(serverEndPoint);
                Server.Listen(10);

                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine(" 1:1 Secret Talk : 당신은 \'서버\' 입니다. ");
                Console.WriteLine(" 1:1 Secret Talk : 클라이언트의 연결을 기다립니다........ ");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine(">> 연결대기중입니다...");

                Client = Server.Accept();
                Client2 = Server.Accept();

                if (Client.Connected)
                {
                    while (true)
                    {
                        int g_DataLength = Client.Receive(getByte, 0, getByte.Length, SocketFlags.None);
                        stringbyte = Encoding.UTF7.GetString(getByte);


                        if (stringbyte != String.Empty)
                        {
                            int getValueLength = 0;
                            getValueLength = byteArrayDefrag(getByte);
                            stringbyte = Encoding.UTF7.GetString(getByte, 0, getValueLength + 1);
                            Console.WriteLine("\t[Client1]수신 :{0} ({1})", stringbyte, getValueLength);
                            setByte = Encoding.UTF7.GetBytes(stringbyte);

                            Client.Send(setByte, 0, setByte.Length, SocketFlags.None);

                            for (int i = 0; i < getByte.Length; i++)
                            {
                                getByte[i] = 0;
                            }

                        }
                    }
                }

                if (Client2.Connected)
                {
                    while (true)
                    {
                        int g_DataLength = Client2.Receive(getByte, 0, getByte.Length, SocketFlags.None);
                        stringbyte = Encoding.UTF7.GetString(getByte);


                        if (stringbyte != String.Empty)
                        {
                            int getValueLength = 0;
                            getValueLength = byteArrayDefrag(getByte);
                            stringbyte = Encoding.UTF7.GetString(getByte, 0, getValueLength + 1);
                            Console.WriteLine("\t[Client2]수신 :{0} ({1})", stringbyte, getValueLength);
                            setByte = Encoding.UTF7.GetBytes(stringbyte);

                            Client2.Send(setByte, 0, setByte.Length, SocketFlags.None);

                            for (int i = 0; i < getByte.Length; i++)
                            {
                                getByte[i] = 0;
                            }

                        }
                    }
                }
            }
            catch (System.Net.Sockets.SocketException socketEx)
            {
                Console.WriteLine("[Error]:{0}", socketEx.Message);
            }
            catch (System.Exception commonEx)
            {
                Console.WriteLine("[Error]:{0}", commonEx.Message);
            }
            finally
            {
                Server.Close();
                Client.Close();
            }
        }
    }
}




//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;

//namespace _2013년5월8일_Server
//{
//    class ServerClass
//    {
//        public static Socket Server, ServerYou, Client;

//        public static byte[] getByte = new byte[1024];
//        public static byte[] setByte = new byte[1024];

//        public const int sPort = 5000;
//        public const int sPortyou = 5001;

//        static public IPAddress serverIP = IPAddress.Parse("192.168.42.120");
//        static public IPAddress serverIPYou = IPAddress.Parse("192.168.42.120");

//        static public string strServerIP = null;
//        static public string strClientIP = null;
//        public static string strMsg = null;

        

//        [STAThread]
//        static void Main(string[] args)
//        {
//            ServerInit();

//            Thread UserOne = new Thread(new ThreadStart(ServerStart));

//            //Thread UserTwo = new Thread(new ThreadStart(ServerStart));

//            UserOne.Start();
//            //UserTwo.Start();





//        }
//        static public void ServerInit()
//        {

//            Console.WriteLine(">>[서버]서버IP를 입력해주십시오.");
//            strServerIP = Console.ReadLine();

//            Console.WriteLine(">>[서버]클라이언트IP를 입력해주십시오.");
//            strClientIP = Console.ReadLine();


//            serverIP = IPAddress.Parse(strServerIP);
//            serverIPYou = IPAddress.Parse(strClientIP);

//        }



//        static public void ServerStart()
//        {

//            string stringbyte = null;
//            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);
//            IPEndPoint serverEndPointYou = new IPEndPoint(serverIPYou, sPortyou);

//            try
//            {
//                Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
//                                ProtocolType.Udp);
//                ServerYou = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
//                                ProtocolType.Udp);

//                Server.Bind(serverEndPoint);
//                ServerYou.Connect(serverEndPointYou);

//                Console.WriteLine("-----------------------------------------------------");
//                Console.WriteLine(" 1:1 Secret Talk : 당신은 \'서버\' 입니다. ");
//                Console.WriteLine(" 1:1 Secret Talk : 클라이언트의 연결을 기다립니다........ ");
//                Console.WriteLine(" 반쌍방형 통신 / 무전기와 같음");
//                Console.WriteLine("-----------------------------------------------------");
//                Console.WriteLine(">> 연결대기중입니다...");

//                while (true)
//                {
//                    Server.Receive(getByte, 0, getByte.Length, SocketFlags.None);
//                    stringbyte = Encoding.UTF7.GetString(getByte);
//                    Console.WriteLine(Server.Connected);
//                    if (stringbyte != String.Empty)
//                    {
//                        int getValueLength = 0;

//                        getValueLength = byteArrayDefrag(getByte);
//                        stringbyte = Encoding.UTF7.GetString(getByte, 0, getValueLength + 1);

//                        Console.WriteLine("\t수신 :{0} ({1})",
//                                        stringbyte, getValueLength + 1);

//                        Console.Write(">>");

//                        strMsg = Console.ReadLine();



//                        setByte = Encoding.UTF7.GetBytes(strMsg);
//                        ServerYou.Send(setByte, 0, setByte.Length, SocketFlags.None);


//                        Console.WriteLine("\t송신 :{0} | ({1})",
//                                        strMsg, setByte.Length + 1);

//                        for (int i = 0; i < getByte.Length; i++)
//                        {
//                            getByte[i] = 0;
//                        }

//                        for (int i = 0; i < setByte.Length; i++)
//                        {
//                            setByte[i] = 0;
//                        }
//                    }
//                }
//            }

//            catch (System.Net.Sockets.SocketException socketEx)
//            {
//                Console.WriteLine("[Error]:{0}", socketEx.Message);
//            }
//            catch (System.Exception commonEx)
//            {
//                Console.WriteLine("[Error]:{0}", commonEx.Message);
//            }
//            finally
//            {
//                Server.Close();
//                ServerYou.Close();
//            }
//        }

//        public static int byteArrayDefrag(byte[] sData)
//        {
//            int endLength = 0;
//            for (int i = 0; i < sData.Length; i++)
//            {
//                if ((byte)sData[i] != (byte)0)
//                {
//                    endLength = i;
//                }
//            }

//            return endLength;
//        }
//    }
//}

