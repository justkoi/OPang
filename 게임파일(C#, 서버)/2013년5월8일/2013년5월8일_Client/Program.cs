using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace _2013년5월8일_Client
{
    class Program
    {
        public static Socket socket;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
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
            string sendstring = null;
            string getstring = null;


            string strServerIP = null;

            Console.WriteLine(">>[클라이언트]서버IP를 입력해주십시오.");
            strServerIP = Console.ReadLine();


            IPAddress serverIP = IPAddress.Parse(strServerIP);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine(" 1:1 Secret Talk : 당신은 \'클라이언트\' 입니다. ");
            Console.WriteLine(" 1:1 Secret Talk : 서버로 접속을 시작합니다. [엔터를 입력하세요] ");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine(">> 엔터키를 누르면 연결됩니다!");

            Console.ReadLine();

            socket.Connect(serverEndPoint);

            if (socket.Connected)
            {
                Console.WriteLine(">> 정삭적으로 연결 되었습니다.(전송할 데이터를 입력해주세요)");

            }

            while (true)
            {
                Console.Write(">>");
                sendstring = Console.ReadLine();

                if (sendstring != String.Empty)
                {

                    setbyte = Encoding.UTF7.GetBytes(sendstring);

                    int setValueLength = 0;
                    string setstring = null;

                    setValueLength = byteArrayDefrag(setbyte);
                    setstring = Encoding.UTF7.GetString(setbyte, 0, setValueLength + 1);


                    socket.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
                    socket.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);

                    getstring = Encoding.UTF7.GetString(getbyte);

                    Console.WriteLine("\t수신 :{0} ({1})", setstring, setValueLength);

                }
            }

        }

    }
}


//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace ClientSideSocket
//{
//    class ClientClass
//    {
//        public static Socket socket, socketme;
//        public static byte[] getbyte = new byte[1024];
//        public static byte[] setbyte = new byte[1024];

//        public const int sPort = 5000;            // 발신용
//        public const int sPortMe = 5001;        // 수신용

//        [STAThread]
//        static void Main(string[] args)
//        {
//            string sendstring = null;
//            string strServerIP = null;
//            string strClientIP = null;

//            int byteEndLength = 0;
            


//            Console.WriteLine(">>[클라이언트]서버IP를 입력해주십시오.");
//            strServerIP = Console.ReadLine();

//            Console.WriteLine(">>[클라이언트]클라이언트IP를 입력해주십시오.");
//            strClientIP = Console.ReadLine();


//            IPAddress serverIP = IPAddress.Parse(strServerIP);
//            IPAddress ServerIPMe = IPAddress.Parse(strClientIP);

//            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);
//            IPEndPoint serverEndPointMe = new IPEndPoint(ServerIPMe, sPortMe);

//            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
//                            ProtocolType.Udp);
//            socketme = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
//                            ProtocolType.Udp);

//            Console.WriteLine("-----------------------------------------------------");
//            Console.WriteLine(" 1:1 Secret Talk : 당신은 \'클라이언트\' 입니다. ");
//            Console.WriteLine(" 1:1 Secret Talk : 서버로 접속을 시작합니다. [엔터를 입력하세요] ");
//            Console.WriteLine(" 반쌍방형 통신 / 무전기와 같음");
//            Console.WriteLine("-----------------------------------------------------");
//            Console.WriteLine(">> 엔터키를 누르면 연결됩니다!");
//            Console.ReadLine();

//            socket.Connect(serverEndPoint);
//            socketme.Bind(serverEndPointMe);

//            if (socket.Connected)
//            {
//                Console.WriteLine(">> 정상적으로 연결되었습니다(전송한 데이터를 입력해주세요)");
//            }

//            while (true)
//            {
//                Console.Write(">>");
//                sendstring = Console.ReadLine();
//                sendMessage(sendstring);
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

//        public static void sendMessage(string sendmsg)
//        {
//            string getstring = null;

//            if (sendmsg != string.Empty)
//            {
//                int getValueLength = 0;

//                int setValueLength = 0;
//                string setstring = null;

//                setbyte = Encoding.UTF7.GetBytes(sendmsg);


//                setValueLength = byteArrayDefrag(setbyte);
//                setstring = Encoding.UTF7.GetString(setbyte, 0, setValueLength + 1);


//                socket.Send(setbyte, 0, setbyte.Length, SocketFlags.None);

//                Console.WriteLine("\t송신 :{0} ({1})", setstring,
//                              setValueLength + 1);

//                socketme.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);


//                getValueLength = byteArrayDefrag(getbyte);
//                getstring = Encoding.UTF7.GetString(getbyte, 0, getValueLength + 1);


//                Console.WriteLine("\t수신 :{0} ({1})", getstring,
//                              getValueLength + 1);
//            }
//            else
//            {
//                Console.WriteLine("수신된 데이터 없음");
//            }
//        }
//    }
//}


