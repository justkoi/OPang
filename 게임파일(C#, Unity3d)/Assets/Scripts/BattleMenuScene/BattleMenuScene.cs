using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AsyncSocket;
using System;
using System.Collections.Generic;
public enum E_CLIENTSTATE
{
    E_CLIENTSTATE_NONE, //!< 아무것도 아님
    E_CLIENTSTATE_MATCHTING, //!< 자동매치 등록 중
    E_CLIENTSTATE_PLAYGAME, //!< 게임 중
};
public enum E_CHATMODE
{
    E_CHATMODE_CHATLIST, //!<채팅
    E_CHATMODE_USERLIST, //!< 접속자
    E_CHATMODE_ROOMLIST, //!< 게임 방
}
public class ChatLine
{
    public int m_nLine = 10;
    public string[] g_strLine;
    public string[] m_strLine_ChatList;
    public string[] m_strLine_UserList;
    public string[] m_strLine_RoomList;
    public E_CHATMODE m_eMode;

    public ChatLine(int nLine)
    {
        m_nLine = nLine;
        m_eMode = E_CHATMODE.E_CHATMODE_CHATLIST;

        g_strLine = new string[nLine];
        m_strLine_ChatList = new string[nLine];
        m_strLine_UserList = new string[nLine];
        m_strLine_RoomList = new string[nLine];

        Clear();
    }

    public void Clear()
    {
        for (int i = 0; i < g_strLine.Length; i++)
        {
            m_strLine_ChatList[i] = "";
            m_strLine_UserList[i] = "";
            m_strLine_RoomList[i] = "";
        }


        if (m_eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_ChatList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_UserList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_RoomList[i];
            }
        }
    }

    public void Clear(E_CHATMODE eMode)
    {
        if (eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                m_strLine_ChatList[i] = "";
            }
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                m_strLine_UserList[i] = "";
            }
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                m_strLine_RoomList[i] = "";
            }
        }


        if (eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_ChatList[i];
            }
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_UserList[i];
            }
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_RoomList[i];
            }
        }
    }

    public void Input(string strMessage, E_CHATMODE eMode)
    {
        if (eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            //!< 한칸씩 당긴다
            for (int i = g_strLine.Length - 1; i > 0; i--)
            {
                m_strLine_ChatList[i] = m_strLine_ChatList[i - 1];
            }
            m_strLine_ChatList[0] = strMessage;
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            //!< 한칸씩 당긴다
            for (int i = g_strLine.Length - 1; i > 0; i--)
            {
                m_strLine_UserList[i] = m_strLine_UserList[i - 1];
            }
            m_strLine_UserList[0] = strMessage;
        }
        else if (eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            //!< 한칸씩 당긴다
            for (int i = g_strLine.Length - 1; i > 0; i--)
            {
                m_strLine_RoomList[i] = m_strLine_RoomList[i - 1];
            }
            m_strLine_RoomList[0] = strMessage;
        }

        if (m_eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_ChatList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_UserList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_RoomList[i];
            }
        }
    }

    public void ChangeMode(E_CHATMODE eMode)
    {
        m_eMode = eMode;
        if (m_eMode == E_CHATMODE.E_CHATMODE_CHATLIST)
        {
            for (int i = 0; i <  g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_ChatList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_USERLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_UserList[i];
            }
        }
        else if (m_eMode == E_CHATMODE.E_CHATMODE_ROOMLIST)
        {
            for (int i = 0; i < g_strLine.Length; i++)
            {
                g_strLine[i] = m_strLine_RoomList[i];
            }
        }
    }
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

public enum E_READY_STATE
{
    E_READY_STATE_NONE,
    E_READY_STATE_MATCHING, //!< 매칭 중
}

public class OrderDraw
{
    public int m_nBlockNumber = 0;

    public OrderDraw(int nBlockNumber)
    {
        m_nBlockNumber = nBlockNumber;
    }
}


public class OrderSelectMagicBlock
{
    public int m_nDeckIndex = 0;

    public OrderSelectMagicBlock(int nDeckIndex)
    {
        m_nDeckIndex = nDeckIndex;
    }
}


public class OrderPutBlock
{
    public int m_nBlockIndex = 0;
    public E_BLOCK_NUMBER m_eBlockNumber = E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER;
    public OrderPutBlock(int nBlockIndex, E_BLOCK_NUMBER eBlockNumber)
    {
        m_nBlockIndex = nBlockIndex;
        m_eBlockNumber = eBlockNumber;
    }
}

public class OrderCreateBlock
{
    public int m_nX = 0;
    public int m_nY = 0;
    public E_BLOCK_NUMBER m_eBlockNumber = E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER;

    public OrderCreateBlock(int nX, int nY, E_BLOCK_NUMBER eBlockNumber)
    {
        m_nX = nX;
        m_nY = nY;
        m_eBlockNumber = eBlockNumber;
    }
}

public class BattleMenuScene : MonoBehaviour
{


    static public AsyncSocketClient sock = null;
       
    static public SoundManager g_SoundMng = null;
    public UILabel m_GoldNumber = null;
    public UILabel m_GoldNumberMinus = null;
    static public UILabel m_UILabel = null;

    static public GameObject g_pChatWindow = null;

    public UILabel m_ID_Input = null;
    public UILabel m_IP_Input = null;
    public UILabel m_Msg_Input = null;

    static private int m_nStage = 7;

    public GameObject m_pUIBlind = null;

    public GameObject m_pItemStore = null;
    public GameObject m_pItemSlot_1 = null;
    public GameObject m_pItemSlot_2 = null;
    public GameObject m_pItemButton_1 = null;

    public int m_nStep = 0;

    static public int g_nMinusGold = 0;

    static public string g_strIP = "192.168.43.23";
    static public string g_strId = "AndroidUser";
    static public string g_strMsg = "";


    static public bool g_bCunnected = false;

    static public string strTemp = null;

    public const int D_CHAT_LINE = 10;

    static public ChatLine g_ChatWindow = new ChatLine(D_CHAT_LINE);

    static public UILabel[] g_LineLabel = new UILabel[D_CHAT_LINE];


    static public float m_fStartTimer = 0.0f;

    static public E_READY_STATE m_eState = E_READY_STATE.E_READY_STATE_NONE;

    static public float m_fStartTime = 3.0f;

    static public List<OrderDraw> g_P1_OrderToDrawList = new List<OrderDraw>();
    static public List<OrderDraw> g_P2_OrderToDrawList = new List<OrderDraw>();

    static public List<OrderSelectMagicBlock> g_P1_OrderToSelectMagicBlockList = new List<OrderSelectMagicBlock>();
    static public List<OrderSelectMagicBlock> g_P2_OrderToSelectMagicBlockList = new List<OrderSelectMagicBlock>();

    static public List<OrderPutBlock> g_P1_OrderToPutBlockList = new List<OrderPutBlock>();
    static public List<OrderPutBlock> g_P2_OrderToPutBlockList = new List<OrderPutBlock>();

    static public List<OrderCreateBlock> g_P1_OrderToCreateBlockList = new List<OrderCreateBlock>();
    // static public List<OrderCreateBlock> g_P2_OrderToPutBlockList = new List<OrderPutBlock>();

    static public void SendMessageToServer(string strMessage)
    {
        Debug.Log(strMessage);
        sock.Send(Encoding.Unicode.GetBytes(strMessage)); //GetEncoding("euc-kr")
    }


    static private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
    {
        g_bCunnected = true;
        Debug.Log("Cunnected!");
        m_UILabel.text = "연결 됨!";




        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_NAME).ToString(), g_strId);
        SendMessageToServer(strTemp);


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

    static public void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
    {

        Debug.Log("Received!");

        //Debug.Log("Default1 : " + Encoding.Unicode.EncodingName);
        //Debug.Log("Default2 : " + Encoding.Unicode.WebName);
        //Debug.Log("Default3 : " + Encoding.Unicode.BodyName);
        //Debug.Log("Default4 : " + Encoding.Unicode.HeaderName);

        //Encoding.Unicode
        //Debug.Log("Default1 : " + Encoding.Unicode.EncodingName);
       // Debug.Log("Default1 : " + Encoding.Unicode.EncodingName);
        

        string strCommand = Encoding.Unicode.GetString(e.ReceiveData, 0, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터의 유형을 분석합니다.
        string strData = Encoding.Unicode.GetString(e.ReceiveData, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE, e.ReceiveBytes - (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터를 확인합니다


        string strTemp = null;

        int nSeek = 0;
        const int nSeekCount = 2;

        Debug.Log("R:" + strCommand+strData);

        Debug.Log("RC:" + strCommand);

       // Debug.Log("LSTEP0");

        if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString())
        {

            strTemp = string.Format("\t{0}", strData);
            //Console.WriteLine(strTemp);
            Debug.Log(strTemp);
            g_ChatWindow.Input(strTemp, E_CHATMODE.E_CHATMODE_CHATLIST);
            UpdateChat();

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            //strTemp = "";
           // strTemp += string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_CLIENT).ToString());
            Debug.Log("Step1");

            string strX = "";
            string strY = "";
            string strBlockNumber = "";
            int nX = 0;
            int nY = 0;
            int nBlockNumber = 0;


            while (true)
            {

                strX = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                nX = Int32.Parse(strX);

                strY = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                nY = Int32.Parse(strY);

                strBlockNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                nBlockNumber = Int32.Parse(strBlockNumber);

                g_P1_OrderToCreateBlockList.Add(new OrderCreateBlock(nX, nY, (E_BLOCK_NUMBER)nBlockNumber));
                Debug.Log("Step2 :" + g_P1_OrderToCreateBlockList.Count);
                //strTemp += string.Format("{0}{1}{2}", nX, nY, nBlockNumber);

                if (nSeek >= e.ReceiveBytes)
                    break;
            }


            Debug.Log("Step3");

            //!< 다음 생성 블록정보 수신
            //!< 다음 생성 블록 X,Y 좌표 및 종류


           

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString())
        {
            // Debug.Log("LSTEP1");
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            string strName = "";
            string strState = "";

            string strNot = "";
            E_CLIENTSTATE eState = E_CLIENTSTATE.E_CLIENTSTATE_NONE;

            // Debug.Log("LSTEP2");
            while (true)
            {
                //!< 이름 / 플레이상태(한자리수숫자)

                strName = "";
                strState = "";
                while (true)
                {
                    // Debug.Log("LSTEP2W");
                    strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                    nSeek += nSeekCount;

                    if (strNot.Equals("Γ") == true)
                        break;
                    strName += strNot;
                }
               //  Debug.Log("LSTEP3 : " + strName);
                strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                int nState = Int32.Parse(strNot);
                // Debug.Log("LSTEP3N : " + strNot + "/" + nState);

                if (nState == (int)E_CLIENTSTATE.E_CLIENTSTATE_NONE)
                    strState = "대기실";
                else if (nState == (int)E_CLIENTSTATE.E_CLIENTSTATE_MATCHTING)
                    strState = "게임찾는중";
                else if (nState == (int)E_CLIENTSTATE.E_CLIENTSTATE_PLAYGAME)
                    strState = "게임중";

                strTemp = string.Format("[{0}]     <{1}>", strName, strState);

                // Debug.Log("LSTEP3I :" + strTemp);
                g_ChatWindow.Input(strTemp, E_CHATMODE.E_CHATMODE_USERLIST);
                //g_ChatWindow.ChangeMode(E_CHATMODE.E_CHATMODE_USERLIST);
                //UpdateChat();

                // Debug.Log("LSTEP3-2" + nState);

                if (nSeek >= e.ReceiveBytes)
                {

                    // Debug.Log("LSTEP4");
                    break;
                }
            }

            // Debug.Log("LSTEP5");

            UpdateChat();
        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString())
        {
            // Debug.Log("LSTEP1");
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            string strName = "";
            string strState = "";

            string strNot = "";
            E_CLIENTSTATE eState = E_CLIENTSTATE.E_CLIENTSTATE_NONE;

            // Debug.Log("LSTEP2");
            while (true)
            {
                //!< 방이름 / 현재명/Max명

                strName = "";
                strState = "";
                while (true)
                {
                    // Debug.Log("LSTEP2W");
                    strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                    nSeek += nSeekCount;

                    if (strNot.Equals("Γ") == true)
                        break;
                    strName += strNot;
                }
                //  Debug.Log("LSTEP3 : " + strName);
                strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                int nCount = Int32.Parse(strNot);
                strNot = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount);
                nSeek += nSeekCount;
                int nCountMax = Int32.Parse(strNot);
                // Debug.Log("LSTEP3N : " + strNot + "/" + nState);


                strTemp = string.Format("[{0}]     {1}/{2}", strName, nCount, nCountMax);

                // Debug.Log("LSTEP3I :" + strTemp);
                g_ChatWindow.Input(strTemp, E_CHATMODE.E_CHATMODE_ROOMLIST);
                //g_ChatWindow.ChangeMode(E_CHATMODE.E_CHATMODE_USERLIST);
                //UpdateChat();

                // Debug.Log("LSTEP3-2" + nState);

                if (nSeek >= e.ReceiveBytes)
                {

                    // Debug.Log("LSTEP4");
                    break;
                }
            }

            // Debug.Log("LSTEP5");

            UpdateChat();
        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_SERVER).ToString())
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


            g_P1_OrderToPutBlockList.Add(new OrderPutBlock(nBlockIndex, (E_BLOCK_NUMBER)nBlockNumber));

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_SERVER).ToString())
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


            g_P2_OrderToPutBlockList.Add(new OrderPutBlock(nBlockIndex, (E_BLOCK_NUMBER)nBlockNumber));

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_SELECT_MB_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            string strDrawIndex = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
            int nDrawIndex = Int32.Parse(strDrawIndex);

            g_P2_OrderToSelectMagicBlockList.Add(new OrderSelectMagicBlock(nDrawIndex));

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_SELECT_MB_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            string strDrawIndex = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
            int nDrawIndex = Int32.Parse(strDrawIndex);

            g_P1_OrderToSelectMagicBlockList.Add(new OrderSelectMagicBlock(nDrawIndex));

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;

            Debug.Log("Step1-1" + nSeek + "/" + strCommand);
            Debug.Log("Is True? : " + (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString()));
            Debug.Log("Is True? : " + (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString()));

            while (true)
            {
                if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다


                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);

                    Debug.Log("Number : " + nRandomNumber);

                    g_P1_OrderToDrawList.Add(new OrderDraw(nRandomNumber));
                    //Draw(1, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P1Draw");

                    //strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                }
                else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);


                    g_P2_OrderToDrawList.Add(new OrderDraw(nRandomNumber));
                    //Draw(2, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P2Draw");

                    // strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);



                }

                if (nSeek >= e.ReceiveBytes)
                {
                    Debug.Log("메세지의 끝입니다.(루프 탈출)");
                    break;
                }
            }

            //int nRandomNumber = Int32.Parse(strData);

            //Console.WriteLine(strTemp);
            Debug.Log("Receive_P1_Draw");
        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;

            Debug.Log("Step1-2/" + nSeek + "/" + strCommand);
            Debug.Log("Is True? : " + (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString()));
            Debug.Log("Is True? : " + (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString()));


            while (true)
            {
                if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);

                    Debug.Log("Number : " + nRandomNumber);


                    g_P1_OrderToDrawList.Add(new OrderDraw(nRandomNumber));
                    //Draw(1, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Debug.Log("P1Draw");

                    //strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);


                }
                else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);


                    g_P2_OrderToDrawList.Add(new OrderDraw(nRandomNumber));
                    //Draw(2, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Debug.Log("P2Draw");

                    // strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);



                }

                if (nSeek >= e.ReceiveBytes)
                {
                    Debug.Log("메세지의 끝입니다.(루프 탈출)");
                    break;
                }
            }
        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString())
        {

            strTemp = string.Format("{0}", strData);
            // Console.WriteLine(strTemp);

        }
        else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_START).ToString())
        {

            string strName = "";
            string strNot = "";

            Debug.Log("Step1");
            Debug.Log(strData);
            nSeek = (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;
            while (true)
            {
                strNot = "";
                strNot += Encoding.Unicode.GetString(e.ReceiveData, nSeek, 2);
                Debug.Log(strNot);
                nSeek += 2;
                if (strNot.Equals("Γ") == true)
                    break;
                strName += strNot;
            }

            for (int y = 0; y < UserDefine.D_MAP_HEIGHT; y++)
            {
                for (int x = 0; x < UserDefine.D_MAP_WIDTH; x++)
                {
                    strNot = "";
                    strNot += Encoding.Unicode.GetString(e.ReceiveData, nSeek, 2);

                    Debug.Log(strNot);
                    GameScene.m_nbBlockMap[x, y] = System.Int32.Parse(strNot);
                    nSeek += 2;

                }
            }
            Debug.Log("Step23");

            nSeek += 2;
            strNot = "";
            strNot += Encoding.Unicode.GetString(e.ReceiveData, nSeek, 2);
            //nSeek += 2;
            Debug.Log(strNot);
            GameScene.g_nMyPlayer = System.Int32.Parse(strNot);
            Debug.Log(strNot);
            //Debug.Log("Step2");
            strTemp = string.Format("vs [{0}] 매칭완료!", strName);
            g_ChatWindow.Input(strTemp, E_CHATMODE.E_CHATMODE_CHATLIST);
            strTemp = string.Format("잠시 후 게임을 시작합니다!");
            g_ChatWindow.Input(strTemp, E_CHATMODE.E_CHATMODE_CHATLIST);
            UpdateChat();

            m_fStartTimer = m_fStartTime;

            if (GameScene.g_nMyPlayer == 1)
            {
                GameScene.g_strPlayer1_Name = g_strId;
                GameScene.g_strPlayer2_Name = strName;
            }
            else if (GameScene.g_nMyPlayer == 2)
            {
                GameScene.g_strPlayer1_Name = strName;
                GameScene.g_strPlayer2_Name = g_strId;
            }


            //Debug.Log(strTemp);

        }
        //string Message = "　　　　　" + Encoding.Unicode.GetString(e.ReceiveData, 0, e.ReceiveBytes);
        //Console.WriteLine(Message);
        //}
    }

    static private void OnError(object sender, AsyncSocketErrorEventArgs e)
    {
        // UpdateTextFunc(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + " Error Message: " + e.AsyncSocketException.ToString() + "\n");
    }       


    // Use this for initialization
    void Start()
    {
        m_nStep = 0;
        m_eState = E_READY_STATE.E_READY_STATE_NONE;

        g_pChatWindow = GameObject.Find("Chat");
        


        sock = new AsyncSocketClient(0);

        // 이벤트 핸들러 재정의
        sock.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
        sock.OnClose += new AsyncSocketCloseEventHandler(OnClose);
        sock.OnSend += new AsyncSocketSendEventHandler(OnSend);
        sock.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);
        sock.OnError += new AsyncSocketErrorEventHandler(OnError);

        g_strId += UnityEngine.Random.Range(0, 1000).ToString();

        m_ID_Input = GameObject.Find("ID_Input").GetComponentInChildren<UILabel>();
        m_IP_Input = GameObject.Find("IP_Input").GetComponentInChildren<UILabel>();
        m_Msg_Input = GameObject.Find("Msg_Input").GetComponentInChildren<UILabel>();
        m_UILabel = GameObject.Find("Message").GetComponentInChildren<UILabel>();


        for (int i = 0; i < D_CHAT_LINE; i++)
        {
            g_LineLabel[i] = g_pChatWindow.transform.FindChild("Line" + i.ToString()).GetComponent<UILabel>();
        }


        // Cunnect();
                

        g_SoundMng = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        g_SoundMng.Play(10, true, 0.6f);

        m_GoldNumber = GameObject.Find("GoldNumber").GetComponentInChildren<UILabel>();
        m_GoldNumber.text = GameScene.g_nGold.ToString();

        m_GoldNumberMinus = GameObject.Find("GoldMinusNumber").GetComponentInChildren<UILabel>();
        m_GoldNumberMinus.text = "( -" + g_nMinusGold.ToString() + ")";

        //m_ID_Input = GameObject.Find("GoldNumber").GetComponentInChildren<UILabel>();

        //m_pUIBlind = GameObject.Find("UIBlind");
        //m_pUIBlind.active = false;

        //m_pItemStore = GameObject.Find("ItemStore");

        //for (int i = 0; i < m_pItemStore.transform.childCount; i++)
        //{
        //    m_pItemStore.transform.GetChild(i).gameObject.active = false;
        //}


        //m_pItemSlot_1 = GameObject.Find("ItemSlot_1");
        //m_pItemSlot_2 = GameObject.Find("ItemSlot_2");
        //m_pItemButton_1 = GameObject.Find("ItemButton_1");

        //m_pItemStore.active = false;
        //m_pItemSlot_1.active = false;
        //m_pItemSlot_2.active = false;
        //m_pItemButton_1.active = false;

        m_GoldNumberMinus.transform.gameObject.active = false;

        UpdateChat();


        g_ChatWindow.Input("환영합니다!", E_CHATMODE.E_CHATMODE_CHATLIST);
        UpdateChat();

    }

    static public void Cunnect()
    {
        sock.Connect(g_strIP, 80);
        Debug.Log("연결 중...");
        m_UILabel.text = "연결 중...";
    }

    static public void AutoMatch()
    {
        if (g_bCunnected == true)
        {
            strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_MATCH).ToString());
            sock.Send(Encoding.Unicode.GetBytes(strTemp));

            Debug.Log("대전 상대를 검색하는 중...");
            m_UILabel.text = "대전 상대를 검색하는 중...";

            m_eState = E_READY_STATE.E_READY_STATE_MATCHING;
        }
        else
        {
            Debug.Log("먼저 Cunnect를 해주십시오");
            m_UILabel.text = "먼저 Cunnect를 해주십시오";
        }
    }


    static public void Send()
    {
        if (g_bCunnected == true)
        {
            if (g_strMsg.Length >= 5 && g_strMsg.Substring(0, 5) == "/list")
            {
                strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString());
            }
            else if (g_strMsg.Length >= 5 && g_strMsg.Substring(0, 5) == "/room")
            {
                strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString());
            }
            else if (g_strMsg.Length >= 7 && g_strMsg.Substring(0, 6) == "/join ")
            {
                g_strMsg = g_strMsg.Replace("/join ", "");
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_JOIN).ToString(), g_strMsg);
            }
            else
            {
                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_CHAT).ToString(), g_strMsg);
            }
            sock.Send(Encoding.Unicode.GetBytes(strTemp));

            m_UILabel.text = "전송 완료!";
        }
        else
        {
            g_ChatWindow.Input("먼저 Cunnect를 해주십시오", E_CHATMODE.E_CHATMODE_CHATLIST);
            UpdateChat();
            m_UILabel.text = "서버에 연결되어 있지 않습니다.";
        }
    }
    static public void UpdateChat()
    {
        for (int i = 0; i < g_ChatWindow.m_nLine; i++)
        {
            g_LineLabel[i].text = g_ChatWindow.g_strLine[i];
        }
    }

    public void GameStart()
    {

        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_MAN;
        Application.LoadLevel("GameScene");
        
    }
    // Update is called once per frame
    void Update()
    {
        g_strId = m_ID_Input.text;
        g_strIP = m_IP_Input.text;
        g_strMsg = m_Msg_Input.text;

        if (m_fStartTimer > 0.0f)
        {
            m_fStartTimer -= Time.deltaTime;

            if (m_fStartTimer <= 0.0f)
            {
                GameStart();
            }
        }
        //if (GameScene.g_P1_Inventory.Inv_kind[0, 0] == -1)
        //{
        //    m_pItemSlot_1.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "null";
        //}
        //else
        //{
        //    m_pItemSlot_1.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "Item_" + GameScene.g_P1_Inventory.Inv_kind[0, 0];
        //}

        //if (GameScene.g_P1_Inventory.Inv_kind[0, 1] == -1)
        //{
        //    m_pItemSlot_2.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "null";
        //}
        //else
        //{
        //    m_pItemSlot_2.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "Item_" + GameScene.g_P1_Inventory.Inv_kind[0, 1];
        //}
    }

    public void Open_ItemStore()
    {
        m_pUIBlind.active = true;

        m_pItemStore.active = true;

        for (int i = 0; i < m_pItemStore.transform.childCount; i++)
        {
            m_pItemStore.transform.GetChild(i).gameObject.active = true;
        }

        m_pItemSlot_1.active = true;
        m_pItemSlot_2.active = true;
        m_pItemButton_1.active = true;
        m_GoldNumberMinus.transform.gameObject.active = true;
        m_nStep = 1;
    }
    public void Close_ItemStore()
    {
        m_pUIBlind.active = false;

        m_pItemStore.active = false;

        for (int i = 0; i < m_pItemStore.transform.childCount; i++)
        {
            m_pItemStore.transform.GetChild(i).gameObject.active = false;
        }
        m_pItemSlot_1.active = false;
        m_pItemSlot_2.active = false;
        m_pItemButton_1.active = false;
        m_GoldNumberMinus.transform.gameObject.active = false;
        m_nStep = 0;

        g_nMinusGold = 0;

        GameScene.g_P1_Inventory.Inv_kind[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_kind[0, 1] = -1;

        GameScene.g_P1_Inventory.Inv_num[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_num[0, 1] = -1;


        m_GoldNumberMinus.text = "( -" + StageMenuScene.g_nMinusGold.ToString() + ")";


    }

    void OnDestroy()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

}
