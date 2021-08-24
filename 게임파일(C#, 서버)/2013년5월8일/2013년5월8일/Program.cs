using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace _2013년5월8일
{
    class Program
    {
        [STAThread] static void Main(string[] args)
        {
            int g_nExType = 3;

            switch(g_nExType)
            {
                case 1: //!< (1) 첫번째 초기화 방법 , IP로 초기화
                    IPAddress ipaddr = IPAddress.Parse("207.46.68.21");
                    Console.WriteLine(ipaddr.ToString());
                    break;

                case 2: //!< (2) 두번째 초기화 방법 , DNS로 초기화
                    IPHostEntry ipentry = Dns.Resolve("www.game.hs.kr"); //!< 네이버의 IP를 전부 얻어온다.

                    foreach (IPAddress ipaddr2 in ipentry.AddressList)
                    {
                        Console.WriteLine(ipaddr2.ToString());
                    }
                    break;
                case 3:
                    string strURI = "http://www.game.hs.kr";
                    Uri uri = new Uri(strURI);
                    Console.WriteLine("Host : " + uri.Host);
                    //!< www.naver.com

                    Console.WriteLine("URL Query : " + uri.Query);
                    //쿼리가 출력된다.
                    Console.WriteLine("Host Type : " + uri.HostNameType);
                    //!< Dns가 출력된다. IP인 경우 Ipv4로 출력된다.
                    Console.WriteLine(uri.AbsolutePath);
                    //!< threadboard/content.asp 경로
                    break;
                case 4:
                    //!< 서버로 사용될 컴퓨터의 IPAdress 객체 생성 현 PC를 서버로 이용합니다.
                    IPAddress iaAdress = Dns.Resolve("localhost").AddressList[0];
                    //!< IP Address  ipaddr = IPAddress.Parse("127.0.0.1"); 해도 상관없습니다.

                    //!< 소켓에 이용될 연결 종점을 생성합니다. 포트는 8000번
                    IPEndPoint iepEndPoint = new IPEndPoint(iaAdress, 8000);
                    //!< 소켓 객체를 생성합니다.

                    Socket skSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
                    skSocket.Bind(iepEndPoint); //!< 위에서 생성한 연결종점을 Bind합니다 (연결)


                    Socket skNewSocket = skSocket.Accept();
                    //!< 클라이언트의 접속을 기다립니다.

                    //!< 0 ~ 1023 까지는 시스템 포트로서 시스템이 사용하게 될 포트로 예약되어져 있다고 생각하시면 됩니다.
                    //!< 1024 ~ 49151까지가 사용자 포트가 됩니다. MSSQL이나 네트워크 게임 등 네트웤 통신이 필요로 하는 응용프로그램들이 사용하게 될 범위이며. 우리가 작성하게 될 프로그램들도 저 범위안에서 포트를 지정하여 사용하게 됩니다.
                    break;
                    
            }
        
        
        }
    }
}
