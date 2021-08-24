using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;


namespace SecretTalk_Client
{

    public enum E_NETWORK_COMMAND //!< 4자리의 네트워크 커맨드
    {
        E_NETWORK_COMMAND_BITE = 8, //!< 유니코드 8바이트 나머지는 4바이트
        E_NETWORK_COMMAND_NAME = 1001, //!< 사용자 이름 및 계정생성 요청
        E_NETWORK_COMMAND_CHAT = 1002, //!< 채팅 메세지 송수신
        E_NETWORK_COMMAND_LIST = 1003, //!< 접속 리스트 요청
        E_NETWORK_COMMAND_ROOM = 1004, //!< 방 정보 요청
        E_NETWORK_COMMAND_JOIN = 1005, //!< 방 입장 요청
        E_NETWORK_COMMAND_MATCH = 1006, //!< 매치 대기열 등록
        E_NETWORK_COMMAND_START = 1007,
    }


    class Program
    {
        static private AsyncSocketClient sock = null;
        static public string strIP = null;
        static public bool m_bCunnected = false;
        

        static void Main(string[] args)
        {
            string strTemp = null;
            string strId = null;
            Console.WriteLine("===== WindowsPhone7 : Secret Talk =====");
            Console.WriteLine(">> 클라이언트를 시작합니다!");
            Console.WriteLine(">> (1) 전체 사용자에게 로그인 & 로그아웃 메세지 표시");
            Console.WriteLine(">> (2) ID는 반드시 입력하셔야 합니다.");
            Console.WriteLine(">> (3) 명령어 /list");

            do
            {
                Console.WriteLine(">> ID를 입력해 주십시오!");
                strId = Console.ReadLine();
            } while (strId == "");

            Console.WriteLine(">> 서버의 IP를 입력해주십시오 (Default 혹은 D 입력 시 기본 서버)");
            strIP = Console.ReadLine();
            if (strIP.Equals("D") == true || strIP.Equals("Default") == true)
            {
                strIP = "14.55.95.35";
            }

            sock = new AsyncSocketClient(0);

            // 이벤트 핸들러 재정의
            sock.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
            sock.OnClose += new AsyncSocketCloseEventHandler(OnClose);
            sock.OnSend += new AsyncSocketSendEventHandler(OnSend);
            sock.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);
            sock.OnError += new AsyncSocketErrorEventHandler(OnError);


            sock.Connect(strIP, 80);
            Console.WriteLine(">> 서버에 연결중입니다.");

            while(true)
            {
                if( m_bCunnected == true)
                {
                    Console.WriteLine(">> 서버에 정상적으로 접속되었습니다!");
                    break;
                }
            }

            //sock.Send(Encoding.Unicode.GetBytes("name" + strId))

            strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_NAME).ToString(), strId);
            SendMessage(strTemp);

            Console.WriteLine(strTemp);

            //if( sock.Connection.Connected == true )
            //{
            //    Console.WriteLine(">> 접속 성공!");
            //}
            //else if (sock.Connection.Connected == false)
            //{
            //    Console.WriteLine(">> 접속 실패!");
            //}
            while (true)
            {
                Console.Write(">>");
                string strString = Console.ReadLine();

                if (strString.Length >= 5 && strString.Substring(0, 5) == "/list")
                {
                    strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString());
                }
                else if (strString.Length >= 5 && strString.Substring(0, 5) == "/room")
                {
                    strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString());
                }
                else if (strString.Length >= 7 && strString.Substring(0, 6) == "/join ")
                {
                    strString = strString.Replace("/join ", "");
                    strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_JOIN).ToString(), strString);
                }
                else if (strString.Length >= 6 && strString.Substring(0, 6) == "/match")
                {
                    strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_MATCH).ToString());
                }
                else
                {
                    strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString(), strString);
                }

                SendMessage(strTemp);
            }

        }

        static private void SendMessage(string strMessage)
        {
            sock.Send(Encoding.Unicode.GetBytes(strMessage));
        }


        static private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            m_bCunnected = true;

            //UpdateTextFunc(txtMessage, "HOST -> PC: Connected ID: " + e.ID.ToString() + "\n");
        }

        static private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            //UpdateTextFunc(txtMessage, "HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
        }

        static private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            //UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " Bytes sent: " + e.SendBytes.ToString() + "\n");
        }

        static private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
           // UpdateTextFunc(txtMessage, "HOST -> PC: Receive ID: " + e.ID.ToString() + " Bytes received: " + e.ReceiveBytes.ToString() +
            //    " Data: " + new string(Encoding.Unicode.GetChars(e.ReceiveData)) + "\n");

            string strCommand = Encoding.Unicode.GetString(e.ReceiveData, 0, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터의 유형을 분석합니다.
            string strData = Encoding.Unicode.GetString(e.ReceiveData, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE, e.ReceiveBytes - (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터를 확인합니다

            string strTemp = null;


            //if (strSave.Contains("/ask_onEYTNM") == true)
            //{
            //    Console.WriteLine("==> 서버의 클라이언트 확인요청에 응답합니다!");
            //    SendMessage(strString);
            //    sock.Send(Encoding.Unicode.GetBytes("/answer_onEYTNM"));
            //}
            //else
            //{

            if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString())
            {

                strTemp = string.Format("\t{0}", strData);
                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString())
            {

                strTemp = string.Format("{0}", strData);
                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString())
            {

                strTemp = string.Format("{0}", strData);
                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_START).ToString())
            {

                strTemp = string.Format("vs [{0}] 매칭완료!", strData); Console.WriteLine(strTemp);
                strTemp = string.Format("잠시 후 게임을 시작합니다!"); Console.WriteLine(strTemp);
                // Console.WriteLine(strTemp);

            }

                //string Message = "　　　　　" + Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                //Console.WriteLine(Message);
            //}
        }

        static private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
           // UpdateTextFunc(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + " Error Message: " + e.AsyncSocketException.ToString() + "\n");
        }       

    }
}
