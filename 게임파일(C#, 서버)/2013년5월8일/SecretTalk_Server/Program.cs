using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using AsyncSocket;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;


namespace SecretTalk_Server
{

    public enum E_BLOCK_NUMBER
    {
        E_BLOCK_NUMBER_WATER,
        E_BLOCK_NUMBER_FIRE,
        E_BLOCK_NUMBER_METAL,
        E_BLOCK_NUMBER_WOOD,
        E_BLOCK_NUMBER_EARTH,
        E_BLOCK_NUMBER_MAX,
        E_BLOCK_NUMBER_ITEM_RAINBOW,
        E_BLOCK_NUMBER_ITEM_RESET,
    };

    public enum E_BLOCK_LIGHT
    {
        E_BLOCK_LIGHT_BLACK,
        E_BLOCK_LIGHT_WHITE,
        E_BLOCK_LIGHT_MAX
    };

    public enum E_BLOCK_STATE
    {
        E_BLOCK_STATE_NONE,
        E_BLOCK_STATE_CREATING,
        E_BLOCK_STATE_ACT,
        E_BLOCK_STATE_CHANGING,
    };

    public enum E_BLOCK_MODE
    {
        E_BLOCK_MODE_COLOR,
        E_BLOCK_MODE_RUNE,
        E_BLOCK_MODE_MAX
    }
    public enum E_NETWORK_COMMAND //!< 4자리의 네트워크 커맨드
    {
        E_NETWORK_COMMAND_BITE = 8, //!< 유니코드는 8바이트를 읽습니다....^^~~
        E_NETWORK_COMMAND_NAME = 1001, //!< 사용자 이름 및 계정생성 요청
        E_NETWORK_COMMAND_CHAT = 1002, //!< 채팅 메세지 송수신
        E_NETWORK_COMMAND_LIST = 1003, //!< 접속 리스트 요청
        E_NETWORK_COMMAND_ROOM = 1004, //!< 방 정보 요청
        E_NETWORK_COMMAND_JOIN = 1005, //!< 방 입장 요청
        E_NETWORK_COMMAND_MATCH = 1006, //!< 매치 대기열 등록
        E_NETWORK_COMMAND_START = 1007, //!< 게임 시작 요청

        E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT = 1101, //!< P1_Draw (패 뽑기) 요청
        E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT = 1201, //!< P2_Draw (패 뽑기) 요청
        E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER = 2101, //!< P1_Draw (패 뽑기) 명령
        E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER = 2201, //!< P2_Draw (패 뽑기) 명령

        E_NETWORK_COMMAND_P1_SELECT_MB_FROM_CLIENT = 1102, //!< P1_SelectMagicBlock (패 선택) 요청
        E_NETWORK_COMMAND_P2_SELECT_MB_FROM_CLIENT = 1202, //!< P2_SelectMagicBlock (패 선택) 요청
        E_NETWORK_COMMAND_P1_SELECT_MB_FROM_SERVER = 2102, //!< P1_SelectMagicBlock (패 선택) 명령
        E_NETWORK_COMMAND_P2_SELECT_MB_FROM_SERVER = 2202, //!< P2_SelectMagicBlock (패 선택) 명령

        E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_CLIENT = 1103, //!< P1_SelectMagicBlock (패 놓기) 요청
        E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_CLIENT = 1203, //!< P2_SelectMagicBlock (패 놓기) 요청
        E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_SERVER = 2103, //!< P1_SelectMagicBlock (패 놓기) 명령
        E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_SERVER = 2203, //!< P2_SelectMagicBlock (패 놓기) 명령

        E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_CLIENT = 1104, //!< P1_SelectMagicBlock (블록 생성) 요청
        E_NETWORK_COMMAND_P2_CREATE_BLOCK_FROM_CLIENT = 1204, //!< P2_SelectMagicBlock (블록 생성) 요청
        E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_SERVER = 2104, //!< P1_SelectMagicBlock (블록 생성) 명령
        E_NETWORK_COMMAND_P2_CREATE_BLOCK_FROM_SERVER = 2204, //!< P2_SelectMagicBlock (블록 생성) 명령
    }

    class Room
    {

        public List<int> m_nClientList = new List<int>(); //!< 룸에 접속한 클라이언트 인덱스번호
        public string m_strRoomName = "TestRoom";
        public int m_nMax = 6;
        public int m_nIndex = 0;

        public Room(string strRoomName, int nMax)
        {
            m_strRoomName = strRoomName;
            m_nMax = nMax;
            m_nIndex = Program.g_nRoomIndex++;
        }

        public int Join(int nClientIndex)
        {
            if (m_nClientList.Count >= m_nMax)
                return 0;

            Program.clientList[nClientIndex].nRoomNumber = m_nIndex;
            m_nClientList.Add(nClientIndex);
            
            return 1;

        }

        

        
    };

    class Program
    {
        static public AsyncSocketServer server;
        static public List<AsyncSocketClient> clientList;
        //static List<string> strNameList;
        static List<bool> g_bAsk_On = new List<bool>();

        static public int id;
        static public string strIP = null;

        static public int g_nRoomIndex = 0;


        static public List<Room> g_RoomList = new List<Room>();

        static public List<int> g_MatchList = new List<int>();

        static public byte[] buSize = new byte[4];

        static public int g_nStartTime = 0;

        static public Thread StartTimer = null;


        public const int D_MAP_WIDTH = 6;
        public const int D_MAP_HEIGHT = 6;

        static public int[,] g_nBlockMap = new int[D_MAP_WIDTH,D_MAP_HEIGHT];

        //static void CheckAsk(Object stateInfo)
        //{
        //    for (int i = 0; i < g_bAsk_On.Count; i++)
        //    {
        //        if(g_bAsk_On[i] == false)
        //        {
        //            Console.Write("\t**{0}/{0}**\t", i, strNameList.Count);
        //            Console.WriteLine("!>>응답하지 않는 클라이언트 [{0}]", strNameList[i]);
        //        }
        //    }
        //}


        static public void CreateNewRoom(string strRoomName, int nMax)
        {
            Room pTemp = new Room(strRoomName, nMax);
            
            g_RoomList.Add(pTemp);
            
        }

        public static string GetExternalIp()
        {

            //TimeSpan.FromSeconds(

           // DateTime.
            
            WebClient client = new WebClient();
            
            // Add a user agent header in case the requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)");

            string baseurl = "http://checkip.dyndns.org/";

            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            s = s.Replace("<html><head><title>Current IP Check</title></head><body>", "").Replace("</body></html>", "").ToString();
            s = s.Replace("Current IP Address: ", "");
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");
            return s;
        }

        static void Main(string[] args)
        {
            
            
            Console.WriteLine("===== WindowsPhone7 : Secret Talk =====");
            Console.WriteLine(" >> 서버의 외부IP를 받아옵니다...");
            Console.WriteLine(">[http://checkip.dyndns.org/] 접속중...");

            //string WanIP = ew System.Net.WebCnlient().DownloadString(("http://www.whatismyip.com/automation/n09230945.asp"));

            string WanIP = GetExternalIp();
            string inIP = null;


            //strIP = WanIP;

            //string whatIsMyIp = "http://www.whatismyip.com/automation/n09230945.asp";

            //WebClient wc = new WebClient();
            //UTF8Encoding utf8 = new UTF8Encoding();
            //string requestHtml = "";

            //requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));

            //IPAddress externalIp = null;

            //externalIp = IPAddress.Parse(requestHtml);



            Console.WriteLine(">> 외부 IP 확인 완료!");

            Console.WriteLine(" >> 서버의 내부IP를 가져옵니다...");
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            inIP = IPHost.AddressList[0].ToString();


            Console.WriteLine(">> 내부 IP 확인 완료!");


            Console.WriteLine(">> 생성을 시도할 IP를 선택해주십시오 (1 or 2)");
            Console.WriteLine(">> (1 : 외부) {0}", WanIP);
            Console.WriteLine(">> (2 : 내부) {0}", inIP);


            strIP = Console.ReadLine();

            if (strIP == "1")
            {
                strIP = WanIP;
            }
            else if (strIP == "2")
            {
                strIP = inIP;
            }
            else if (strIP == "3")
            {
                strIP = Console.ReadLine();
            }

            server = new AsyncSocketServer(strIP, 80);
            Console.WriteLine(">> 서버를 초기화 합니다...");


            server.OnAccept += new AsyncSocketAcceptEventHandler(OnAccept);
            server.OnError += new AsyncSocketErrorEventHandler(OnError);

            clientList = new List<AsyncSocketClient>(100);
            //strNameList = new List<string>(100);
            Console.WriteLine(">> 성공!");
            id = 0;

            Console.WriteLine(">> 서버 에서 클라이언트를 Listen 시작합니다...");
            try
            {
                server.Listen();
            }
            catch ( Exception e)
            {
              Console.WriteLine(e.Message);
            }
            Console.WriteLine(">> 성공!");



            Console.WriteLine(">> 방을 생성하고 있습니다...");

            CreateNewRoom("WindowsPhone7", 6);

            Console.WriteLine(">> 성공!");




            Console.WriteLine(">> 서버가 시작되었습니다!");

            Show_Server_RoomList();

            while (true)
            {
                Console.Write(">>*");
                string strString = "*[서버] :";
                strString += Console.ReadLine();

                SendAllClient(strString);
            }

        }

        static public void SendAllClient(string strMessage)
        {
            string strTemp = null;
            for (int i = 0; i < clientList.Count; i++)
            {
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString(), strMessage);
                clientList[i].Send(Encoding.Unicode.GetBytes(strTemp));
               
            }
        }

        static public void SendRoomClient(string strMessage, int nRoomIndex)
        {
            string strTemp = null;

            if (nRoomIndex < 0)
                return;

            for (int i = 0; i < g_RoomList[nRoomIndex].m_nClientList.Count; i++)
            {
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString(), strMessage);
                clientList[  g_RoomList[nRoomIndex].m_nClientList[i]  ].Send(Encoding.Unicode.GetBytes(strTemp));
            }
        }

        static public void SendRoomClientOrder(string strMessage, int nRoomIndex)
        {
            if (nRoomIndex < 0)
                return;

            for (int i = 0; i < g_RoomList[nRoomIndex].m_nClientList.Count; i++)
            {
                clientList[g_RoomList[nRoomIndex].m_nClientList[i]].Send(Encoding.Unicode.GetBytes(strMessage));
            }
        }

        static public void Show_Server_RoomList()
        {
            string strTemp = null;
            strTemp += "\n========대기중인 방 리스트========";
            //Console.WriteLine();
            for (int i = 0; i < g_RoomList.Count; i++)
            {
                strTemp += "\n== [" + i + "] : " + g_RoomList[i].m_strRoomName + " (" + g_RoomList[i].m_nClientList.Count + "/" + g_RoomList[i].m_nMax + ")";
                //Console.WriteLine("== [" + i + "] : " + g_RoomList[i].m_strRoomName + " ({0}/{1})", g_RoomList[i].m_nClientList.Count, g_RoomList[i].m_nMax);
            }
            strTemp += "\n==================================";
            Console.WriteLine(strTemp);
        }
        static public void Show_Client_RoomList(int nClient)
        {
            //string strTemp = null;
            //string strTemp2 = null;
            //strTemp += "\n========대기중인 방 리스트========";
            ////Console.WriteLine();
            //for (int i = 0; i < g_RoomList.Count; i++)
            //{
            //    strTemp += "\n== [" + i + "] : " + g_RoomList[i].m_strRoomName + " (" + g_RoomList[i].m_nClientList.Count + "/" + g_RoomList[i].m_nMax + ")";
            //    //Console.WriteLine("== [" + i + "] : " + g_RoomList[i].m_strRoomName + " ({0}/{1})", g_RoomList[i].m_nClientList.Count, g_RoomList[i].m_nMax);
            //}
            //strTemp += "\n==================================";
            ////clientList[nClient].Send(Encoding.Unicode.GetBytes(strTemp));


            //strTemp2 = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString(), strTemp);
            //clientList[nClient].Send(Encoding.Unicode.GetBytes(strTemp2));


            string strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString());

            for (int i = 0; i < g_RoomList.Count; i++)
            {
                //!< 방이름 / 사람 수 (한자리수숫자)
                strTemp += string.Format("{0}Γ{1}{2}", g_RoomList[i].m_strRoomName, g_RoomList[i].m_nClientList.Count, g_RoomList[i].m_nMax);

            }

            //Console.WriteLine("Room==>" + strTemp);
            clientList[nClient].Send(Encoding.Unicode.GetBytes(strTemp));
        }

        static public void Show_Client_UserList(int nClient)
        {

            string strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString());
            
            for (int k = 0; k < clientList.Count; k++)
            {
                //!< 이름 / 플레이상태(한자리수숫자)
                strTemp += string.Format("{0}Γ{1}", clientList[k].strName,(int)clientList[k].eState);
                
            }

            //Console.WriteLine("List==>" + strTemp);
            clientList[nClient].Send(Encoding.Unicode.GetBytes(strTemp));

        }

        static private void OnAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            AsyncSocketClient worker = new AsyncSocketClient(id, e.Worker);

            // 데이터 수신을 대기한다.
            worker.Receive();

            worker.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
            worker.OnClose += new AsyncSocketCloseEventHandler(OnClose);
            worker.OnError += new AsyncSocketErrorEventHandler(OnError);
            worker.OnSend += new AsyncSocketSendEventHandler(OnSend);
            worker.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);

            // 접속한 클라이언트를 List에 포함한다.
            clientList.Add(worker);

            string strTemp = string.Format("[System:Access] 접속 확인 IP:Port[{0}]", e.Worker.RemoteEndPoint.ToString());
            Console.WriteLine(strTemp);


        }


        static private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            //(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + "Error Message: " + e.AsyncSocketException.ToString() + "\n");

            Console.WriteLine("[Error!] 클라이언트"+e.ID.ToString() + "  " + e.AsyncSocketException.ToString());

            if (e.AsyncSocketException is SocketException)
            {
                for (int i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i].ID == e.ID)
                    {
                        string LoginMessage = "[" + clientList[i].strName + "]님이 로그아웃 하셨습니다!";

                        clientList.Remove(clientList[i]);

                        //Console.WriteLine("Error {0} {1}", clientList.Count, strNameList.Count);

                        Console.WriteLine(LoginMessage);
                        SendAllClient(LoginMessage);
                        break;
                    }
                }
            }
            

            //for ( ; i < clientList.Count; i++)
            //{
            //    clientList[i].ID--;
            //}


        }

        static public int SearchNumber(int nID)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if( clientList[i].ID == nID)
                {
                    return i;
                }
            }
            return -1;
        }

        static public int SearchRoom(int nRoomNumber)
        {
            for (int i = 0; i < g_RoomList.Count; i++)
            {
                if (g_RoomList[i].m_nIndex == nRoomNumber)
                {
                    return g_RoomList[i].m_nIndex;
                }
            }
            return -1;
        }

        static public void StartSet() //!< 게임 시작 세팅
        {


            //Console.Write("Step1");
            Random rand = new Random();
            for(int y=0; y<D_MAP_HEIGHT; y++)
            {
                for(int x=0; x<D_MAP_WIDTH; x++)
                {
                    g_nBlockMap[x,y] = -1;
                }
            }

            for(int y=0; y<D_MAP_HEIGHT; y++)
            {
                for(int x=0; x<D_MAP_WIDTH; x++)
                {
                    //g_nBlockMap[x,y]

                    while(true)
                    {
                        int nRandNum = rand.Next(0,(int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
                        //Console.Write("{0}\n", nRandNum);
                        if(SearchBlock(x + 1,y + 0,nRandNum) == false)
                            continue;
                        if(SearchBlock(x - 1,y + 0,nRandNum) == false)
                            continue;
                        if(SearchBlock(x + 0,y + 1,nRandNum) == false)
                            continue;
                        if(SearchBlock(x + 0,y - 1,nRandNum) == false)
                            continue;

                        g_nBlockMap[x,y] = nRandNum;
                        break;
                    }

                }
            }

            //Console.Write("Step2");

        }

        static public bool SearchBlock(int nX, int nY, int nRandNum)
        {

            if(nX < 0 || nX >= D_MAP_WIDTH)
                return true;
            if(nY < 0 || nY >= D_MAP_HEIGHT)
                return true;
            if(g_nBlockMap[nX,nY] == nRandNum)
                return false;

            return true;

        }


        static private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {

            // Console.WriteLine("HOST -> PC: Receive ID: " + e.ID.ToString() + " -Bytes received: " + e.ReceiveBytes.ToString());
            //string strSave = Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes); //!< 전송받은 데이터를 확인합니다.
            string strCommand = Encoding.Unicode.GetString(e.ReceiveData, 0, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터의 유형을 분석합니다.
            string strData = Encoding.Unicode.GetString(e.ReceiveData, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE, e.ReceiveBytes - (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터를 확인합니다

            string strTemp = null;
            int nSeek = 0;
            const int nSeekCount = 2;
            
            //string strMessage = "[" + clientList[nIndex].strName + "] :" + Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                
            int nIndex = SearchNumber(e.ID); //!< 클라이언트의 ID를 현재 클라이언트 리스트의 인덱스로 변환

            if (nIndex == -1) //!< 존재하지 않는다면 리턴
                return;

            //!< 커멘드에 따른 분기

            if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString()) 
            {

                strTemp = string.Format("[{0}] : {1}",clientList[nIndex].strName,strData);
                // "[" + clientList[nIndex].strName + "] :"


                Console.WriteLine("[System:Chat] "+strTemp);

                SendAllClient(strTemp);
            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_CLIENT).ToString())
            {
                //Console.WriteLine("CreateBlock");
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                strTemp = "";
                strTemp += string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_SERVER).ToString());
                
                string strX = "";
                string strY = "";
                string strBlockNumber = "";
                int nX = 0;
                int nY = 0;
                int nBlockNumber = 0;

               // Console.WriteLine("Step1");
                //Console.WriteLine("Step1-1 " + strData);
                while (true)
                {

                    strX = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                   // Console.WriteLine("Step2 "+strX);
                    nSeek += nSeekCount;
                    nX = Int32.Parse(strX);

                    strY = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                    nSeek += nSeekCount;
                    nY = Int32.Parse(strY);

                    strBlockNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                    nSeek += nSeekCount;
                    nBlockNumber = Int32.Parse(strBlockNumber);

                    strTemp += string.Format("{0}{1}{2}", nX, nY, nBlockNumber);

                    if (nSeek >= e.ReceiveBytes)
                        break;
                }

               // Console.WriteLine("Step3");



                //!< 다음 생성 블록정보 수신
                //!< 다음 생성 블록 X,Y 좌표 및 종류

                SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                //int nV1 = g_RoomList[SearchRoom(clientList[nIndex].nRoomNumber)].m_nClientList[0];
                //int nV2 = g_RoomList[SearchRoom(clientList[nIndex].nRoomNumber)].m_nClientList[01];
               // strTemp = string.Format("[System:CreateBlock] {0}번방 [{1}] vs [{2}] 에서 블록 생성 정보 전송을 요청합니다", clientList[nIndex].nRoomNumber, clientList[nV1].strName, clientList[nV2].strName);
               // Console.WriteLine("[System:CreateBlock] ");

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_CLIENT).ToString())
            {
                string strNot = "";
                string strIndex = "";
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                while (true)
                {
                    strNot = "";
                    strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                    
                    nSeek += nSeekCount;

                   if (strNot.Equals("Γ") == true)
                        break;

                    strIndex += strNot;
                }
                
                int nBlockIndex = Int32.Parse(strIndex);

                string strBlockNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                int nBlockNumber = Int32.Parse(strBlockNumber);


                //!< 블록 선택정보 수신
                //!< 1플레이어의 블록배열Index번째 BlockNumber종류

                strTemp = string.Format("{0}{1}Γ{2}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_SERVER).ToString(), nBlockIndex, nBlockNumber);
                SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                strTemp = string.Format("[System:PutMagicBlock] {0}번방 [{1}]님이 블록 선택 정보 전송을 요청합니다", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_CLIENT).ToString())
            {
                string strNot = "";
                string strIndex = "";
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                while (true)
                {
                    strNot = "";
                    strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);

                    nSeek += nSeekCount;

                    if (strNot.Equals("Γ") == true)
                        break;

                    strIndex += strNot;
                }

                int nBlockIndex = Int32.Parse(strIndex);

                string strBlockNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                int nBlockNumber = Int32.Parse(strBlockNumber);

                //!< 블록 선택정보 수신
                //!< 2플레이어의 블록배열Index번째 BlockNumber종류

                strTemp = string.Format("{0}{1}Γ{2}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_SERVER).ToString(), nBlockIndex, nBlockNumber);
                SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                strTemp = string.Format("[System:PutMagicBlock] {0}번방 [{1}]님이 블록 선택 정보 전송을 요청합니다", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_SELECT_MB_FROM_CLIENT).ToString())
            {
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                string strIndex = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                int nDrawIndex = Int32.Parse(strIndex);

                //!< 패 선택정보 수신
                //!< 1플레이어의 배열Index번째
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_SELECT_MB_FROM_SERVER).ToString(), nDrawIndex);
                SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                strTemp = string.Format("[System:SelectMagicBlock] {0}번방 [{1}]님이 패 선택 정보 전송을 요청합니다", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_SELECT_MB_FROM_CLIENT).ToString())
            {
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                string strIndex = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                int nDrawIndex = Int32.Parse(strIndex);

                //!< 패 선택정보 수신
                //!< 2플레이어의 배열Index번째
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_SELECT_MB_FROM_SERVER).ToString(), nDrawIndex);
                SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);


                strTemp = string.Format("[System:SelectMagicBlock] {0}번방 [{1}]님이 패 선택 정보 전송을 요청합니다", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);
                
                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT).ToString())
            {
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                Console.WriteLine("[System:Draw]" +strCommand+strData);


                while (true)
                {
                    if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT).ToString())
                    {
                        string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                        nSeek += nSeekCount;
                        int nRandomNumber = Int32.Parse(strNumber);

                        strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                        //Console.WriteLine(strTemp);

                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                        SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);


                        strTemp = string.Format("[System:Draw] {0}번방 [{1}]님이 Draw를 요청합니다.", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                        Console.WriteLine(strTemp);
                        //clientList[g_MatchList[0]].Send(Encoding.Unicode.GetBytes(strTemp));
                        //clientList[g_MatchList[1]].Send(Encoding.Unicode.GetBytes(strTemp));

                    }
                    else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT).ToString())
                    {
                        string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                        nSeek += nSeekCount;
                        int nRandomNumber = Int32.Parse(strNumber);

                        strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                        //Console.WriteLine(strTemp);

                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                        SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                        strTemp = string.Format("[System:Draw] {0}번방 [{1}]님이 Draw를 요청합니다.", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                        Console.WriteLine(strTemp);
                        //clientList[g_MatchList[0]].Send(Encoding.Unicode.GetBytes(strTemp));
                        //clientList[g_MatchList[1]].Send(Encoding.Unicode.GetBytes(strTemp));

                    }

                    if (nSeek >= e.ReceiveBytes)
                    {
                        //Console.WriteLine("메세지의 끝입니다.(루프 탈출)");
                        break;
                    }
                }



                //Console.WriteLine("메세지의 끝입니다.(1)");

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT).ToString())
            {
                nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
                Console.WriteLine("[System:Draw]" + strCommand + strData);

                while (true)
                {
                    if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT).ToString())
                    {
                        string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                        nSeek += nSeekCount;
                        int nRandomNumber = Int32.Parse(strNumber);

                        strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                        //Console.WriteLine(strTemp);

                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                        SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                        strTemp = string.Format("[System:Draw] {0}번방 [{1}]님이 Draw를 요청합니다.", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                        Console.WriteLine(strTemp);
                        //clientList[g_MatchList[0]].Send(Encoding.Unicode.GetBytes(strTemp));
                        //clientList[g_MatchList[1]].Send(Encoding.Unicode.GetBytes(strTemp));
                    }
                    else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT).ToString())
                    {
                        string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                        nSeek += nSeekCount;
                        int nRandomNumber = Int32.Parse(strNumber);

                        strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                        //Console.WriteLine(strTemp);

                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                        SendRoomClientOrder(strTemp, clientList[nIndex].nRoomNumber);

                        strTemp = string.Format("[System:Draw] {0}번방 [{1}]님이 Draw를 요청합니다.", clientList[nIndex].nRoomNumber, clientList[nIndex].strName);

                        Console.WriteLine(strTemp);
                        //clientList[g_MatchList[0]].Send(Encoding.Unicode.GetBytes(strTemp));
                        //clientList[g_MatchList[1]].Send(Encoding.Unicode.GetBytes(strTemp));
                    }

                    if (nSeek >= e.ReceiveBytes)
                        break;
                }

                //Console.WriteLine("메세지의 끝입니다.(2)");
            }
            //!< 자동매치 등록
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_MATCH).ToString())
            {
                strTemp = string.Format("[{0}]{1}", clientList[nIndex].strName, "님이 자동매치에 등록하셨습니다.");

                //Console.WriteLine(strTemp);
                SendAllClient(strTemp);

                strTemp = string.Format("[System:Match] [{0}]님이 자동매치에 등록하셨습니다.", clientList[nIndex].strName);

                Console.WriteLine(strTemp);

                g_MatchList.Add(nIndex);

                clientList[nIndex].eState = E_CLIENTSTATE.E_CLIENTSTATE_MATCHTING;

                //!< 둘 이상의 플레이어가 모였을경우
                if (g_MatchList.Count >= 2)
                {
                    int nSaveNumber = g_nRoomIndex;

                    //!< 방을 생성합니다. 방번호 nSaveNumber
                    CreateNewRoom("Room" + nSaveNumber.ToString(), 2);
                    int nRoomNumber = SearchRoom(nSaveNumber);
                    if (nRoomNumber == -1)
                    {
                        Console.WriteLine("방이 존재하지 않습니다.");
                        return;
                    }
                    g_RoomList[nRoomNumber].Join(g_MatchList[0]);
                    g_RoomList[nRoomNumber].Join(g_MatchList[1]);

                    //!< 시작 세팅하기~
                    StartSet();

                    //strTemp = string.Format("<{0}> [{1}] vs [{2}] 게임이 시작되었습니다!", ("Room" + nSaveNumber.ToString()), clientList[g_MatchList[0]].strName, clientList[g_MatchList[1]].strName);
                    //Console.WriteLine(strTemp);

                    //SendAllClient(strTemp);

                    strTemp = string.Format("[System:StartMatch] <Room{0}> [{1}] vs [{2}] 게임이 시작되었습니다!", nSaveNumber, clientList[g_MatchList[0]].strName, clientList[g_MatchList[1]].strName);

                    Console.WriteLine(strTemp);

                    string strMap = "";
                    for (int y = 0; y < D_MAP_HEIGHT; y++)
                    {
                        for (int x = 0; x < D_MAP_WIDTH; x++)
                        {
                            strMap += g_nBlockMap[x, y];
                        }
                    }

                    // Console.Write("Step3");

                    //!< 패킷뭉침방지 0.1초 슬립
                    System.Threading.Thread.Sleep(100);

                    strTemp = string.Format("{0}{1}Γ{2}Γ1", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_START).ToString(), clientList[g_MatchList[1]].strName, strMap);
                    clientList[g_MatchList[0]].Send(Encoding.Unicode.GetBytes(strTemp));

                    strTemp = string.Format("{0}{1}Γ{2}Γ2", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_START).ToString(), clientList[g_MatchList[0]].strName, strMap);
                    clientList[g_MatchList[1]].Send(Encoding.Unicode.GetBytes(strTemp));

                    clientList[g_MatchList[0]].eState = E_CLIENTSTATE.E_CLIENTSTATE_PLAYGAME;
                    clientList[g_MatchList[1]].eState = E_CLIENTSTATE.E_CLIENTSTATE_PLAYGAME;

                    // Console.Write("Step4");

                    //!< 완료 0번과 1번을 매치리스트에서 삭제~

                    g_MatchList.Clear();
                    //  Console.Write("Step5");
                }

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_NAME).ToString())
            {
                int nClient = id;
                int nIndex2 = SearchNumber(nClient);

                if (nIndex2 == -1)
                    return;

                clientList[nIndex2].strName = strData;

                string strLoginMessage = "[" + clientList[nIndex2].strName + "]님이 로그인 하셨습니다!";

                id++;

                //Console.WriteLine(">>Index변환 : {0} <== {1}", nIndex, e.ID);


                //Console.WriteLine(strLoginMessage);
                SendAllClient(strLoginMessage);

                strTemp = string.Format("[System:Login] [{0}]님이 로그인 하셨습니다!", clientList[nIndex2].strName);

                Console.WriteLine(strTemp);


                //Show_Client_UserList(nIndex2);
                //Show_Client_RoomList(nIndex2);
            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString())
            {
                strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 list를 요청합니다!";
                //Console.WriteLine(strTemp);

                Show_Client_UserList(nIndex);

                strTemp = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 list전송을 완료하였습니다!";
                //Console.WriteLine(strTemp);


                strTemp = string.Format("[System:UserList] [{0}]님이 접속자 정보를 요청합니다.", clientList[nIndex].strName);

                Console.WriteLine(strTemp);


            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString())
            {
                strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 room정보를 요청합니다!";
                //Console.WriteLine(strTemp);

                Show_Client_RoomList(nIndex);

                strTemp = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 room정보 전송을 완료하였습니다!";
                //Console.WriteLine(strTemp);


                strTemp = string.Format("[System:RoomList] [{0}]님이 방 정보를 요청합니다.", clientList[nIndex].strName);

                Console.WriteLine(strTemp);

            }
            else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_JOIN).ToString())
            {


                for (int i = 0; i < strData.Length; i++) //!< 데이터에 숫자가 아닌것이 들어있는지 검사
                {
                    if (strData[i] == '0' ||
                        strData[i] == '1' ||
                        strData[i] == '2' ||
                        strData[i] == '3' ||
                        strData[i] == '4' ||
                        strData[i] == '5' ||
                        strData[i] == '6' ||
                        strData[i] == '7' ||
                        strData[i] == '8' ||
                        strData[i] == '9')
                    {

                    }
                    else
                    {

                        strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 문자가 포함된 Join을 요청합니다! ==> 실패!";
                        Console.WriteLine(strTemp);
                        return;
                    }
                }

                int nRoomNumber = Int32.Parse(strData); //!< 숫자 파싱

                strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 " + nRoomNumber.ToString() + "번방으로의 join을 요청합니다!";
                Console.WriteLine(strTemp);

                int nResult = SearchRoom(nRoomNumber); //!< 해당 방 번호를 검사해서 얻어옴 없으면 리턴
                if (nResult == -1)
                    return;

                int nResult2 = g_RoomList[nResult].Join(nIndex); //!< Join 결과 얻어옴 결과로 분기
                if (nResult2 == 0)
                {
                    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에서 " + nRoomNumber.ToString() + "번방으로의 join을 실패하였습니다! (풀방)";

                    Console.WriteLine(strTemp2);

                    clientList[nIndex].Send(Encoding.Unicode.GetBytes("==> 방이 전부차서 Join에 실패하였습니다!"));


                }
                else if (nResult2 == 1)
                {
                    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에서 " + nRoomNumber.ToString() + "번방으로의 join을 완료하였습니다!";

                    Console.WriteLine(strTemp2);

                    clientList[nIndex].nRoomNumber = nRoomNumber;
                    clientList[nIndex].Send(Encoding.Unicode.GetBytes("==> " + nRoomNumber.ToString() + "번 방에 입장하셨습니다!"));

                }


            }

            //else if (strSave.Contains("/room") == true)
            //{
            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 room정보를 요청합니다!";
            //    Console.WriteLine(strTemp);
            //    //Send_ShowList(nIndex);
            //    Show_Client_RoomList(nIndex);
            //    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 room정보 전송을 완료하였습니다!";
            //    Console.WriteLine(strTemp2);
            //}
            //else if (strSave.Contains("/list") == true)
            //{
            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 list를 요청합니다!";
            //    Console.WriteLine(strTemp);
            //    Send_ShowList(nIndex);
            //    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 list전송을 완료하였습니다!";
            //    Console.WriteLine(strTemp2);
            //}

            //if (strSave.Contains("/answer_onEYTNM") == true)
            //{
            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 응답하였습니다!";
            //    Console.WriteLine(strTemp);
            //}
            //else if (strSave.Contains("/join") == true && e.ReceiveBytes >= 7)
            //{
            //    bool bBreak = true;
            //    string strRoomNumber = Encoding.Unicode.GetString(e.ReceiveData, 6, e.ReceiveBytes - 6);
            //    for (int i = 0; i < strRoomNumber.Length; i++)
            //    {
            //        if (strRoomNumber[i] == '0' ||
            //            strRoomNumber[i] == '1' ||
            //            strRoomNumber[i] == '2' ||
            //            strRoomNumber[i] == '3' ||
            //            strRoomNumber[i] == '4' ||
            //            strRoomNumber[i] == '5' ||
            //            strRoomNumber[i] == '6' ||
            //            strRoomNumber[i] == '7' ||
            //            strRoomNumber[i] == '8' ||
            //            strRoomNumber[i] == '9')
            //        {

            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    int nRoomNumber = Int32.Parse(strRoomNumber);

            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 " + nRoomNumber.ToString()  + "번방으로의 join을 요청합니다!";
            //    Console.WriteLine(strTemp);

            //    int nResult = SearchRoom(nRoomNumber);
            //    if (nResult == -1)
            //        return;

            //    int nResult2 = g_RoomList[nResult].Join(nIndex);
            //    if (nResult2 == 0)
            //    {
            //        string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에서 " + nRoomNumber.ToString() + "번방으로의 join을 실패하였습니다! (풀방)";

            //        Console.WriteLine(strTemp2);
                    
            //        clientList[nIndex].Send(Encoding.Unicode.GetBytes("==> 방이 전부차서 Join에 실패하였습니다!"));


            //    }
            //    else if (nResult2 == 1)
            //    {
            //        string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에서 "+ nRoomNumber.ToString()  + "번방으로의 join을 완료하였습니다!";

            //        Console.WriteLine(strTemp2);

            //        clientList[nIndex].nRoomNumber = nRoomNumber;
            //        clientList[nIndex].Send(Encoding.Unicode.GetBytes("==> " + nRoomNumber.ToString() + "번 방에 입장하셨습니다!"));
                
            //    }

            //    //Join_Room(int nRoomNumber);
            //    //Console.WriteLine(strTemp);
            //    //Send_ShowList(nIndex);
            //    //string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 list전송을 완료하였습니다!";
            //    //Console.WriteLine(strTemp2);


            //}
            //else if (strSave.Contains("/room") == true)
            //{
            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 room정보를 요청합니다!";
            //    Console.WriteLine(strTemp);
            //    //Send_ShowList(nIndex);
            //    Show_Client_RoomList(nIndex);
            //    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 room정보 전송을 완료하였습니다!";
            //    Console.WriteLine(strTemp2);
            //}
            //else if (strSave.Contains("/list") == true)
            //{
            //    //strNameList.Contains<string>(strNameList[e.ID]);
            //    string strTemp = "==> [" + clientList[nIndex].strName + "]님의 클라이언트에서 list를 요청합니다!";
            //    Console.WriteLine(strTemp);
            //    Send_ShowList(nIndex);
            //    string strTemp2 = "<== [" + clientList[nIndex].strName + "]님의 클라이언트에 list전송을 완료하였습니다!";
            //    Console.WriteLine(strTemp2);
            //}
            
            //else
            //{

            //    //string Message = "[Client" + e.ID.ToString() + "] :" + Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes);
            //    string Message = "[" + clientList[nIndex].strName + "] :" + Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes);
            //    Console.WriteLine(Message);

            //    SendRoomClient(Message, clientList[nIndex].nRoomNumber);

            //}

            //UpdateTextFunc(txtMessage, "HOST -> PC: Receive ID: " + e.ID.ToString() + " -Bytes received: " + e.ReceiveBytes.ToString() + "\n");
        }

        static private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            //UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " -Bytes sent: " + e.SendBytes.ToString() + "\n");
        }

        static private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {

           Console.WriteLine("HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
        }

        static private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            Console.WriteLine(">> 접속 PC");
            //UpdateTextFunc(txtMessage, "HOST -> PC: Connected ID: " + e.ID.ToString() + "\n");
        }
    }
}
