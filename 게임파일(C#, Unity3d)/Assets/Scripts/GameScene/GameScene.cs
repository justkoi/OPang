using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AsyncSocket;
using System;

[System.Serializable]

public enum E_GAME_STEP
{
    E_GAME_STEP_READY,
    E_GAME_STEP_PLAY,
    E_GAME_STEP_END,
};

public enum E_PLAY_STEP
{
    E_PLAY_STEP_P1_SELECT_MAGIC,
    E_PLAY_STEP_P1_SELECT_BLOCK,
    E_PLAY_STEP_P1_POP_LOGIC,
    E_PLAY_STEP_P1_POP_CHECK,
    E_PLAY_STEP_AI_COMPUTING,
    E_PLAY_STEP_P2_SELECT_MAGIC,
    E_PLAY_STEP_P2_SELECT_BLOCK,
    E_PLAY_STEP_P2_POP_LOGIC,
    E_PLAY_STEP_P2_POP_CHECK,
};


public enum E_GAME_RESULT
{
    E_GAME_RESULT_NONE,
    E_GAME_RESULT_VICTORY,
    E_GAME_RESULT_DEFEAT,
    E_GAME_RESULT_DRAW,
};

public enum E_NOTE_DEFECTION
{
    E_NOTE_DEFECTION_LEFT,
    E_NOTE_DEFECTION_RIGHT,
    E_NOTE_DEFECTION_CENTER,
};

public enum E_GAME_MODE
{
    E_GAME_MODE_VS_AI,
    E_GAME_MODE_VS_MAN,
    E_GAME_MODE_MAX
};

public enum E_ITEM_NUMBER
{
    E_ITEM_NUMBER_RAINBOW,
    E_ITEM_NUMBER_RESET,
    E_ITEM_NUMBER_MAX,
}

public class GameScene : MonoBehaviour {

    static public int g_nP1_DeckMax = 2;
    static public int g_nP2_DeckMax = 3;

    static public Inventory g_P1_Inventory = new Inventory(2, 1);
    static public List<GameObject> m_goBlockList = new List<GameObject>();

    static public bool g_bPause = false;

    public E_GAME_STEP m_eGameStep = E_GAME_STEP.E_GAME_STEP_READY;
    public E_GAME_RESULT m_eGameResult = E_GAME_RESULT.E_GAME_RESULT_NONE;
    public E_PLAY_STEP m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
    static public E_GAME_MODE m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_AI;

    public GameObject[] m_goBlock = new GameObject[5];
    public GameObject[] m_goMagicBlock = new GameObject[5];
    public GameObject[] m_goItem = new GameObject[1];
    //public GameObject m_goMagicBlock = new GameObject();

    static public SoundManager g_SoundMng = null;
    static public EffectManager g_EffectMng = null;

    public GameObject m_goNowFocus = null;
    public GameObject m_goTouchFocus = null;

    public E_BLOCK_NUMBER m_eNowBlock;
    public E_BLOCK_NUMBER m_eTouchBlock;

    public GameObject m_goEnd_Win = null;
    public GameObject m_goEnd_Lose = null;
    public GameObject m_goEnd_Draw = null;
    public GameObject m_goEnd_Pause = null;
    public GameObject m_goEnd_ButtonMainMenu = null;
    public GameObject m_goEnd_ButtonBack = null;
    public GameObject m_goEnd_ButtonPause = null;
    public GameObject m_goEnd_ButtonResume = null;

    public GameObject m_goUIBlind = null;

    public float m_fTimer = 0.0f;

    public int[,] m_nMap = new int[UserDefine.D_MAP_WIDTH, UserDefine.D_MAP_HEIGHT];
    public float[,] m_fMapRegenTimer = new float[UserDefine.D_MAP_WIDTH, UserDefine.D_MAP_HEIGHT];

    public float m_fCreateTimer = 0.0f;
    public float m_fCreateTime = 0.25f;
    public int m_nCreateX = 0;
    public int m_nCreateY = 0; 

    public bool m_bStartCreateBlockEnded = false;


    public int m_nFeverGage = 0;
    public int m_nFeverGageMax = 100;

    public int m_nFeverGagePlus = 1;

    public float m_fFeverGageValue = 0.0f;

    public float m_fFeverTimer = 0.0f;
    public float m_fFeverTime = 20.0f;

    public float m_fTime = 0.0f;

    public float m_fTimeMax = 60.0f;

    public float m_fTimeLimitTimer = 0.0f;
    public float m_fTimeLimitMax = 10.0f;

    public UILabel pTimeNumber = null; //!< 제한시간 Number
    public UILabel pP1_ScoreNumber = null;  //!< 스코어1 Number
    public UILabel pP2_ScoreNumber = null;  //!< 스코어2 Number
    public UILabel pTurnNumber = null;  //!< 현재 Turn
    public UILabel pTargetTurnNumber = null;  //!< 목표 Turn

    public UIFilledSprite pTurnBar = null;
    public UIFilledSprite pTimeBar = null;

    static public int g_nScore_P1 = 0; //!< P1 자신의 스코어
    static public int g_nScore_P2 = 0; //!< P2 컴퓨터(상대방) 의 스코어

    static public int g_nGold = 1000; //!< 획득 골드
    


    static public int g_nScore_P1_Display = 0; //!< P1 자신의 스코어 (실제로 보여짐)
    static public int g_nScore_P2_Display = 0; //!< P2 컴퓨터(상대방) 의 스코어 (실제로 보여짐)

    static public int g_nStage = 1;
    public int g_nTurn = 1; //!< 흐른 턴 수 
    public int g_nTargetTurn = 30; //!< 목표 턴 수
    public int g_nTime = 0; //!< 제한시간

    //public GameObject m_goMagicBlock = null;

    public float m_fDrawTimer = 0.0f;
    public float m_fDrawTime = 2.5f;

    public int m_nP1_DrawNumber = 0;
    public int m_nP2_DrawNumber = 0;

    static public Deck m_Deck_P1 = new Deck();
    static public Deck m_Deck_P2 = new Deck();

    public MagicBlock m_SelectMagic_P1 = null;
    public MagicBlock m_SelectMagic_P2 = null;

    public GameObject m_goSelectMagic = null;

    public GameObject m_goAI_SelectMagic = null;
    public GameObject m_goAI_SelectBlock = null;
    

    public float m_fLogic_Pop_Delay = 0.0f;

    public float m_fLogic_Pop_Delay_Time = 0.6f;

    public float m_fNumberDisplayTimer = 0.0f;
    public float m_fNumberDisplayTime = 0.1f;

    public bool m_bPoped = false;

    public float m_fAI_Select_Magic_Timer = 0.0f;
    public float m_fAI_Select_Magic_Time = 3.0f;

    public float m_fAI_Select_Magic_Time_Min = 1.0f;
    public float m_fAI_Select_Magic_Time_Max = 4.0f;


    public float m_fAI_Select_Block_Timer = 0.0f;
    public float m_fAI_Select_Block_Time = 3.0f;

    public float m_fAI_Select_Block_Time_Min = 1.0f;
    public float m_fAI_Select_Block_Time_Max = 4.0f;

    public List<GameObject> m_BlockPopList = new List<GameObject>();

    //public List<CPopList> m_pAI_PopList_7 = new List<CPopList>();
    public List<CPopList> m_pAI_PopList_6 = new List<CPopList>();
    public List<CPopList> m_pAI_PopList_4 = new List<CPopList>();
    public List<CPopList> m_pAI_PopList_3 = new List<CPopList>();

    
    public List<int> m_nAI_BlockList_4 = new List<int>();

    public GameObject m_goAI_Think = null;

    public GameObject m_goP1_Think = null;
    public GameObject m_goP2_Think = null;

    public GameObject m_goTimeOver = null;

    public GameObject m_goTimeOverScore_P1 = null;
    public GameObject m_goTimeOverScore_P2 = null;

    public int m_nTimeOverMinusScore = 3;
    public int m_nTimeOverWarningCheckStep = 0;


    


    /////////////// BattleMode

    static public int g_nMyPlayer = 1;

    static public string strTemp = "";


    static public int[,] m_nbBlockMap = new int[UserDefine.D_MAP_WIDTH, UserDefine.D_MAP_HEIGHT];


    static public string g_strPlayer1_Name = "";
    static public string g_strPlayer2_Name = "";


    static private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
    {
        //UpdateTextFunc(txtMessage, "HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
    }

    static private void OnSend(object sender, AsyncSocketSendEventArgs e)
    {
        //UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " Bytes sent: " + e.SendBytes.ToString() + "\n");
    }

    private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
    {


        Debug.Log("Receive!");

        string strCommand = Encoding.Unicode.GetString(e.ReceiveData, 0, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터의 유형을 분석합니다.
        string strData = Encoding.Unicode.GetString(e.ReceiveData, (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE, e.ReceiveBytes - (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE); //!< 전송받은 데이터를 확인합니다


        string strTemp = null;

        int nSeek = 0;
        const int nSeekCount = 2;


        if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString())
        {
            nSeek += (int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_BITE;

            Debug.Log("Step1-1" + nSeek+"/" + strCommand);
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

                    Draw(1, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P1Draw");

                    //strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);

                }
                else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);
                    Draw(2, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P2Draw");

                   // strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);



                }

                if (nSeek >= e.ReceiveBytes)
                {
                    Console.WriteLine("메세지의 끝입니다.(루프 탈출)");
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

                    Draw(1, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P1의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P1Draw");

                    //strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_SERVER).ToString(), nRandomNumber);


                }
                else if (strCommand == ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString())
                {
                    string strNumber = Encoding.Unicode.GetString(e.ReceiveData, nSeek, nSeekCount); //!< 전송받은 데이터를 확인합니다
                    nSeek += nSeekCount;
                    int nRandomNumber = Int32.Parse(strNumber);
                    Draw(2, nRandomNumber);

                    //strTemp = string.Format("[{0}]님이 P2의 Draw를 요청합니다. Code[{1}]", clientList[nIndex].strName, nRandomNumber);
                    Console.WriteLine("P2Draw");

                    // strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_SERVER).ToString(), nRandomNumber);



                }

                if (nSeek >= e.ReceiveBytes)
                {
                    Console.WriteLine("메세지의 끝입니다.(루프 탈출)");
                    break;
                }
            }

            //int nRandomNumber = Int32.Parse(strData);

            //Console.WriteLine(strTemp);
            Debug.Log("Receive_P1_Draw");
        }
        
    }

    static private void OnError(object sender, AsyncSocketErrorEventArgs e)
    {
        // UpdateTextFunc(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + " Error Message: " + e.AsyncSocketException.ToString() + "\n");
    }       


	// Use this for initialization
	void Start () {

        Screen.SetResolution(480, 800, false);
       // Screen.SetResolution(Screen.width, Screen.width / 2 * 3, true);
        GameSateData.SaveData();

        m_nP1_DrawNumber = 0;
        m_nP2_DrawNumber = 0;
        
        m_fDrawTimer = 0.0f;

        m_fTimer = 0.0f;
        m_fFeverTimer = 0.0f;

        m_fCreateTimer = 0.0f;
        m_fLogic_Pop_Delay = 0.0f;

        m_fTimeLimitTimer = 0.0f;
        m_fNumberDisplayTimer = 0.0f;

        g_bPause = false;

        // g_P1_Inventory.Input_Item((int)E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW, 1);
        // g_P1_Inventory.Input_Item((int)E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW, 1);

        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
        {
            g_nP1_DeckMax = 2;
            g_nP2_DeckMax = 2;
        }

        if (g_P1_Inventory.Inv_kind[0,0] != -1)
        {
            GameObject CreateObject = (GameObject)Instantiate(m_goItem[g_P1_Inventory.Inv_kind[0, 0]], NewMng.New_Vector3(-1.85f, -3.53f, -2.1f), NewMng.New_Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            //CreateObject.transform.parent = GameObject.Find("Panel").transform;

            MagicBlock ThisBlock = CreateObject.GetComponent<MagicBlock>();
            ThisBlock.m_eBlockNumber = (E_BLOCK_NUMBER)(g_P1_Inventory.Inv_kind[0, 0] + 6);
            ThisBlock.m_eBlockItem = (E_ITEM_NUMBER)g_P1_Inventory.Inv_kind[0, 0];
            ThisBlock.m_nPlayer = 1;
        }
        if (g_P1_Inventory.Inv_kind[0,1] != -1)
        {
            GameObject CreateObject = (GameObject)Instantiate(m_goItem[g_P1_Inventory.Inv_kind[0, 1]], NewMng.New_Vector3(-1.13f, -3.53f, -2.1f), NewMng.New_Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            //CreateObject.transform.parent = GameObject.Find("Panel").transform;
            MagicBlock ThisBlock = CreateObject.GetComponent<MagicBlock>();
            ThisBlock.m_eBlockNumber = (E_BLOCK_NUMBER)(g_P1_Inventory.Inv_kind[0, 1] + 6);
            ThisBlock.m_eBlockItem = (E_ITEM_NUMBER)g_P1_Inventory.Inv_kind[0, 1];
            ThisBlock.m_nPlayer = 1;
        }




        GameScene.g_P1_Inventory.Inv_kind[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_kind[0, 1] = -1;

        GameScene.g_P1_Inventory.Inv_num[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_num[0, 1] = -1;



        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
        m_eGameStep = E_GAME_STEP.E_GAME_STEP_READY;
        m_eGameResult = E_GAME_RESULT.E_GAME_RESULT_NONE;

        g_SoundMng = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        g_EffectMng = GameObject.Find("EffectManager").GetComponent<EffectManager>();

        pTimeNumber = GameObject.Find("TimeNumber").GetComponentInChildren<UILabel>();
        pP1_ScoreNumber = GameObject.Find("ScoreNumber_P1").GetComponentInChildren<UILabel>();
        pP2_ScoreNumber = GameObject.Find("ScoreNumber_P2").GetComponentInChildren<UILabel>();
        pTurnNumber = GameObject.Find("TurnNumber").GetComponentInChildren<UILabel>();
        pTargetTurnNumber = GameObject.Find("TargetTurnNumber").GetComponentInChildren<UILabel>();


        m_goEnd_Win = GameObject.Find("Win");
        m_goEnd_Lose = GameObject.Find("Lose");
        m_goEnd_Draw = GameObject.Find("Draw");
        m_goEnd_Pause = GameObject.Find("Pause");
        m_goEnd_ButtonMainMenu = GameObject.Find("Button_MainMenu");
        m_goEnd_ButtonBack = GameObject.Find("Button_Back");
        m_goEnd_ButtonPause = GameObject.Find("Button_Pause");
        m_goEnd_ButtonResume = GameObject.Find("Button_Resume");

        m_goUIBlind = GameObject.Find("UIBlind");

        pTurnBar = GameObject.Find("TurnBar").GetComponentInChildren<UIFilledSprite>();
        pTimeBar = GameObject.Find("TimeBar").GetComponentInChildren<UIFilledSprite>();

        m_goTimeOver = GameObject.Find("TimeOver");

        m_goTimeOverScore_P1 = GameObject.Find("TimeOverScore_P1");
        m_goTimeOverScore_P2 = GameObject.Find("TimeOverScore_P2");
        
        m_goEnd_Win.active = false;
        m_goEnd_Lose.active = false;
        m_goEnd_Draw.active = false;
        m_goEnd_Pause.active = false;
        m_goEnd_ButtonResume.active = false;

        //m_goEnd.transform.position = NewMng.New_Vector3(0.0f, m_goEnd.transform.position.y, m_goEnd.transform.position.z);



        //GameObject.Find("BackGround_1").transform.localScale = new Vector3(UnityEngine.,,);

        m_goEnd_ButtonMainMenu.active = false;
        m_goEnd_ButtonBack.active = false;
        //m_goEnd_ButtonPause.active = false;
        m_goUIBlind.active = false;


        m_goAI_Think = GameObject.Find("AI_Think");
        m_goP1_Think = GameObject.Find("P1_Think");
        m_goP2_Think = GameObject.Find("P2_Think");

        m_goSelectMagic = GameObject.Find("Ring");
        Game_Init();

        g_SoundMng.Play(0, true, 0.6f);

        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
        {
            m_goP1_Think.transform.FindChild("UILabel").active = false;
            m_goP1_Think.transform.FindChild("UISlicedSprite").active = false;
            m_goP2_Think.transform.FindChild("UILabel").active = false;
            m_goP2_Think.transform.FindChild("UISlicedSprite").active = false;
        }
        else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
        {
            m_goP1_Think.transform.FindChild("UILabel").GetComponent<UILabel>().text = g_strPlayer1_Name;
            m_goP2_Think.transform.FindChild("UILabel").GetComponent<UILabel>().text = g_strPlayer2_Name;
        }


        m_goAI_Think.active = false;
        m_goP1_Think.active = false;
        m_goP2_Think.active = false;

        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
        {
            GameObject.Find("Stage").GetComponentInChildren<UILabel>().text = "Stage " + g_nStage.ToString();
        }
        else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
        {
            GameObject.Find("Stage").GetComponentInChildren<UILabel>().text = "Battle Mode";
        }
        //m_goTimeOver.active = false;

         //m_goTimeOverScore_P1.active = false;
         //m_goTimeOverScore_P2.active = false;

        m_goTimeOverScore_P1.GetComponentInChildren<UILabel>().text = "-" + m_nTimeOverMinusScore.ToString();

        


       // m_goUIBlind = GameObject.Find("UIBlind");
      //  m_goUIBlind.active = false;

        pTurnNumber.text = (g_nTime).ToString();

        // pTimeNumber.text = ((int)(m_fTimeMax - m_fTime)).ToString();
        // pScoreNumber.text = (g_nScore).ToString();

        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
        {
            Draw(1);
            Draw(2);
            m_nP1_DrawNumber++;
            m_nP2_DrawNumber++;
        }

        TimeDisplay();

        //BattleMenuScene.sock.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);
        
	}

    public void Pause()
    {
        g_bPause = true;

        //m_goEnd.active = true;
        //m_goEnd_ButtonResume.active = true;
        //m_goEnd_ButtonPause.active = false;

        m_goEnd_Pause.active = true;
        m_goEnd_ButtonMainMenu.active = true;
        m_goEnd_ButtonBack.active = true;
        m_goUIBlind.active = true;
        m_goEnd_ButtonResume.active = true;
    }


    //static public void SaveData()
    //{
    //    StreamWriter R = null;

    //    TextAsset data = (TextAsset)Resources.Load("Data", typeof(TextAsset));
    //    // puzdata.text is a string containing the whole file. To read it line-by-line:
    //    R = new StreamWriter(Application.dataPath + "/Resources/Data.txt");

    //    //FileStream FS = new FileStream("Stage_1.txt", FileMode.Open);
    //    //StreamReader R = new StreamReader(

    //    R.WriteLine(g_nGold);
    //    Debug.Log(Application.dataPath + "/Resources/Data.txt" + g_nGold);
    //    //int.TryParse(strGold, out g_nGold);

    //    R.Close();
    //}

    //static public void LoadData()
    //{
    //    StringReader R = null;

    //    TextAsset data = (TextAsset)Resources.Load("Data", typeof(TextAsset));
    //    // puzdata.text is a string containing the whole file. To read it line-by-line:
    //    R = new StringReader(data.text);

    //    //FileStream FS = new FileStream("Stage_1.txt", FileMode.Open);
    //    //StreamReader R = new StreamReader(

    //    string strGold = R.ReadLine();
    //    int.TryParse(strGold, out g_nGold);

        
    //    R.Close();
    //}

    public void Reset()
    {
        for (int i = 0; i < m_goBlockList.Count; i++)
        {
            int nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
            m_goBlockList[i].GetComponent<Block>().ChangeNumber((E_BLOCK_NUMBER)nRandomBlockNumber);
        }
    }


    public void Resume()
    {
        g_bPause = false;


        m_goEnd_Pause.active = false;
        m_goEnd_ButtonMainMenu.active = false;
        m_goEnd_ButtonBack.active = false;
        m_goUIBlind.active = false;
        m_goEnd_ButtonResume.active = false;
        //m_goEnd.active = false;
        //m_goEnd_ButtonMenu.active = false;
        //m_goEnd_ButtonResume.active = false;
        //m_goEnd_ButtonPause.active = true;
        //m_goUIBlind.active = false;
    }


    void OnDestroy()
    {

        for (int i = 0; i < m_goBlockList.Count; i++)
        {
            Destroy(m_goBlockList[i]);
            m_goBlockList.Remove(m_goBlockList[i]);
            break;
        }
        m_goBlockList.Clear();

        System.GC.Collect();
        Resources.UnloadUnusedAssets();

    }

    void SceneDestroy()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    void TurnDisplay()
    {
        pTurnNumber.text = (g_nTurn).ToString();
        pTurnBar.fillAmount = ((float)g_nTargetTurn - (float)g_nTurn) / (float)g_nTargetTurn;
    }
    void TimeDisplay()
    {
        int nVirtualNumber = Mathf.CeilToInt(m_fTimeLimitMax - m_fTimeLimitTimer);
        pTimeNumber.text = (nVirtualNumber).ToString();
        pTimeBar.fillAmount = (m_fTimeLimitMax - m_fTimeLimitTimer) / m_fTimeLimitMax;
        if ((m_fTimeLimitMax - m_fTimeLimitTimer) / m_fTimeLimitMax >= 0.3f)
        {
            pTimeBar.color = NewMng.New_Color(0.0f, 0.0f, 0.0f, 100.0f/255.0f);
        }
        else if ((m_fTimeLimitMax - m_fTimeLimitTimer) / m_fTimeLimitMax < 0.3f)
        {
            pTimeBar.color = NewMng.New_Color(1.0f, 0.0f, 0.0f, 100.0f / 255.0f);
        }
    }
    void NumberDisplay()
    {
        m_fNumberDisplayTimer += Time.deltaTime;
        if (m_fNumberDisplayTimer >= m_fNumberDisplayTime)
        {
            m_fNumberDisplayTimer = 0.0f;
            if (g_nScore_P1_Display < g_nScore_P1)
            {
                g_nScore_P1_Display++;
                pP1_ScoreNumber.text = (g_nScore_P1_Display).ToString();
            }
            if (g_nScore_P2_Display < g_nScore_P2)
            {
                g_nScore_P2_Display++;
                pP2_ScoreNumber.text = (g_nScore_P2_Display).ToString();
            }

            if (g_nScore_P1_Display > g_nScore_P1)
            {
                g_nScore_P1_Display--;
                pP1_ScoreNumber.text = (g_nScore_P1_Display).ToString();
            }
            if (g_nScore_P2_Display > g_nScore_P2)
            {
                g_nScore_P2_Display--;
                pP2_ScoreNumber.text = (g_nScore_P2_Display).ToString();
            }
        }


    }
    void Check_TimeOver_P1()
    {
        m_fTimeLimitTimer += Time.deltaTime;
        if (m_fTimeLimitTimer >= m_fTimeLimitMax - 3.0f && m_nTimeOverWarningCheckStep == 0)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }
        else if (m_fTimeLimitTimer >= m_fTimeLimitMax - 2.0f && m_nTimeOverWarningCheckStep == 1)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }
        if (m_fTimeLimitTimer >= m_fTimeLimitMax - 1.0f && m_nTimeOverWarningCheckStep == 2)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }

        if (m_fTimeLimitTimer >= m_fTimeLimitMax)
        {
            g_SoundMng.Play(7,false,0.33f);
            g_nScore_P1 -= m_nTimeOverMinusScore;
            m_goTimeOver.GetComponent<SmoothDisplay>().Play(1.0f);
            m_goTimeOverScore_P1.GetComponent<SmoothDisplay>().Play(1.0f);

            if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
            {
                m_goP1_Think.active = false;
                m_goP2_Think.active = true;
                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_AI_COMPUTING;

            }
            else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
            {
                m_goP1_Think.active = false;
                m_goP2_Think.active = true;
                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
            }



            m_fTimeLimitTimer = 0.0f;
            m_nTimeOverWarningCheckStep = 0;
        }
    }


    void Check_TimeOver_P2()
    {
        m_fTimeLimitTimer += Time.deltaTime;
        if (m_fTimeLimitTimer >= m_fTimeLimitMax - 3.0f && m_nTimeOverWarningCheckStep == 0)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }
        else if (m_fTimeLimitTimer >= m_fTimeLimitMax - 2.0f && m_nTimeOverWarningCheckStep == 1)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }
        if (m_fTimeLimitTimer >= m_fTimeLimitMax - 1.0f && m_nTimeOverWarningCheckStep == 2)
        {
            g_SoundMng.Play(8);
            m_nTimeOverWarningCheckStep++;
        }

        if (m_fTimeLimitTimer >= m_fTimeLimitMax)
        {
            g_SoundMng.Play(7, false, 0.33f);
            g_nScore_P2 -= m_nTimeOverMinusScore;
            m_goTimeOver.GetComponent<SmoothDisplay>().Play(1.0f);
            m_goTimeOverScore_P2.GetComponent<SmoothDisplay>().Play(1.0f);

            if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
            {
                m_goAI_Think.active = true;
                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_AI_COMPUTING;
            }
            else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
            {
                m_goP1_Think.active = true;
                m_goP2_Think.active = false;
                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
            }


            m_fTimeLimitTimer = 0.0f;
            m_nTimeOverWarningCheckStep = 0;
            g_nTurn++;
        }
    }
	// Update is called once per frame
	void Update () {
        if (g_bPause == false)
        {
            if (BattleMenuScene.g_P1_OrderToDrawList.Count > 0)
            {
                Draw(1, BattleMenuScene.g_P1_OrderToDrawList[0].m_nBlockNumber);
                BattleMenuScene.g_P1_OrderToDrawList.Remove(BattleMenuScene.g_P1_OrderToDrawList[0]);
            }
            else if (BattleMenuScene.g_P2_OrderToDrawList.Count > 0)
            {
                Draw(2, BattleMenuScene.g_P2_OrderToDrawList[0].m_nBlockNumber);
                BattleMenuScene.g_P2_OrderToDrawList.Remove(BattleMenuScene.g_P2_OrderToDrawList[0]);
            }

            if (m_eGameStep == E_GAME_STEP.E_GAME_STEP_READY)
            {
                //!< 인공지능 일 때
                if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
                {
                    Start_CreateBlock1();
                    StartDraw();
                }
                else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
                {
                    Start_CreateBlock2();
                    StartDraw();
                }


                if(m_bStartCreateBlockEnded == true && m_nP1_DrawNumber >= g_nP1_DeckMax && m_nP2_DrawNumber >= g_nP2_DeckMax)
                {
                    m_goP1_Think.active = true;
                    m_eGameStep = E_GAME_STEP.E_GAME_STEP_PLAY;
                }

            }
            else if (m_eGameStep == E_GAME_STEP.E_GAME_STEP_PLAY)
            {
                m_fTime += Time.deltaTime;

                NumberDisplay();
                TimeDisplay();
                TurnDisplay();
                RegenCheck();



                //!< 인공지능 일 때
                if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
                {

                    //!< 어떤색의 Magic블록을 선택하시겠습니까?
                    if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC)
                    {
                        Check_TimeOver_P1();
                        if (Input.GetMouseButtonDown(0) == true)
                        {
                            RaycastHit hit;
                            Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(m_Ray, out hit))
                            {

                                Debug.Log("asdf");
                                Debug.Log(hit.collider.name);
                                //Debug.Log(hit.collider.gameObject.transform.parent.name);
                                //Vector3 pos = hit.point;
                                if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                {
                                    MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();

                                    Debug.Log("asdf2");
                                    if (pBlock.m_nPlayer == 1)
                                    {
                                        Debug.Log("asdf3");
                                        if (pBlock.m_eBlockItem == E_ITEM_NUMBER.E_ITEM_NUMBER_RESET)
                                        {
                                            Reset();
                                            pBlock.AlphaDestroy();
                                            g_SoundMng.Play(1);
                                        }
                                        else
                                        {
                                            m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            m_SelectMagic_P1 = pBlock;
                                            g_SoundMng.Play(1);
                                        }

                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK;
                                    }
                                    //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                }

                                //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                //Destroy(hit.collider.gameObject);
                                //CalcTarget();

                            }

                        }
                    }
                    //!< 어디에 두시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK)
                    {
                        Check_TimeOver_P1();
                        if (Input.GetMouseButtonDown(0) == true)
                        {
                            RaycastHit hit;
                            Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(m_Ray, out hit))
                            {
                                //Vector3 pos = hit.point;
                                if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                {
                                    //Debug.Log("StepB1");
                                    MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();

                                    
                                    if (pBlock.m_nPlayer == 1)
                                    {
                                        if (pBlock.m_eBlockItem == E_ITEM_NUMBER.E_ITEM_NUMBER_RESET)
                                        {
                                            Reset();
                                            pBlock.AlphaDestroy();
                                            g_SoundMng.Play(1);
                                        }
                                        else
                                        {
                                            m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            m_SelectMagic_P1 = pBlock;
                                            g_SoundMng.Play(1);
                                        }
                                    }


                                    //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                }
                                else if (hit.collider.gameObject.GetComponent<Block>() != null) //!< 실제로 블록을 놓는 부분
                                {
                                    //Debug.Log("StepB2");
                                    Block pBlock = hit.collider.gameObject.GetComponent<Block>();
                                    if (m_SelectMagic_P1.m_eBlockNumber != pBlock.m_eBlockNumber)
                                    {

                                        m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                        //m_SelectMagic_P1 = pBlock;

                                        pBlock.ChangeNumber(m_SelectMagic_P1.m_eBlockNumber);
                                        m_Deck_P1.m_eDeckList.Remove(m_SelectMagic_P1);

                                        //Destroy(m_SelectMagic_P1.transform.gameObject);
                                        m_SelectMagic_P1.AlphaDestroy();

                                        if(m_SelectMagic_P1.m_eBlockNumber != E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
                                            Draw(1);

                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC;
                                        m_fTimeLimitTimer = 0.0f;
                                        g_SoundMng.Play(2);
                                    }
                                    //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                }

                                //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                //Destroy(hit.collider.gameObject);
                                //CalcTarget();
                            }
                        }
                    }
                    //!< 터트려 봅시다
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC)
                    {
                        if (m_fLogic_Pop_Delay <= m_fLogic_Pop_Delay_Time)
                        {
                            m_fLogic_Pop_Delay += Time.deltaTime;
                        }
                        else
                        {
                            Logic_Pop();
                            m_goSelectMagic.transform.position = new Vector3(-4.0f, m_goSelectMagic.transform.position.y, m_goSelectMagic.transform.position.z);

                            Fill_Block();
                            m_fLogic_Pop_Delay = 0.0f;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_CHECK;

                        }
                    }
                    //!< 다시 터트릴 수 있습니까? Yes or No
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_POP_CHECK)
                    {
                        if (m_bPoped == true)
                        {
                            if (Fill_Block_End() == true)
                            {
                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC;
                            }
                        }
                        else if (m_bPoped == false)
                        {
                            m_goP1_Think.active = false;
                            m_goP2_Think.active = true;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_AI_COMPUTING;
                        }
                    }

                    //!< AI 인공지능으로 넘어갑니다.

                    //!< AI 인공지능 계산 단계

                    //!< AI 행동의 다음 행동에 대한 모든 계산을 끝내고

                    //!< SelectMagicBlock과
                    //!< SelectBlock값을 저장합니다.

                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_AI_COMPUTING)
                    {
                        // m_pAI_PopList_6
                        bool m_bComputed = false;

                        AI_Pop();


                        ////!< 7개 모인 블록에 하나빼기

                        //if (m_bComputed == false && m_pAI_PopList_7.Count >= 1)
                        //{
                        //    int nDeckCount = 0;
                        //    for (nDeckCount = 0; nDeckCount < m_Deck_P2.m_eDeckList.Count; nDeckCount++)
                        //    {
                        //        if (m_Deck_P2.m_eDeckList[nDeckCount].GetComponent<MagicBlock>().m_eBlockNumber != m_pAI_PopList_7[0].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                        //        {
                        //            m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCount].transform.gameObject;

                        //            break;
                        //        }
                        //    }

                        //    //for (int i = 0; i < m_pAI_PopList_6.Count; i++)
                        //    //{
                        //    if (nDeckCount >= m_Deck_P2.m_eDeckList.Count)
                        //    {

                        //    }
                        //    else
                        //    {
                        //        int k = 0;
                        //        for (k = 0; k < m_pAI_PopList_7[0].m_BlockList.Count; k++)
                        //        {
                        //            //m_pAI_PopList_6[0].m_BlockList[j].GetComponent
                        //            int nAnotherBlockIndex = 0;

                        //            m_goAI_SelectBlock = m_pAI_PopList_7[0].m_BlockList[k];

                        //            E_BLOCK_NUMBER eSaveBlockNumber = m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber;

                        //            m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = m_goAI_SelectMagic.GetComponent<MagicBlock>().m_eBlockNumber;

                        //            nAnotherBlockIndex = k + 1;
                        //            if (nAnotherBlockIndex >= m_pAI_PopList_7[0].m_BlockList.Count)
                        //                nAnotherBlockIndex = 0;

                        //            for (int i = 0; i < m_goBlockList.Count; i++)
                        //            {
                        //                Block pBlock = m_goBlockList[i].GetComponent<Block>();
                        //                pBlock.CheckReset();
                        //            }

                        //            CPopList pCheckList = new CPopList();
                        //            pCheckList.Init(SearchBlock(m_pAI_PopList_7[0].m_BlockList[nAnotherBlockIndex].GetComponent<Block>().m_stIndex.m_nX, m_pAI_PopList_7[0].m_BlockList[nAnotherBlockIndex].GetComponent<Block>().m_stIndex.m_nY));

                        //            if (pCheckList.m_BlockList.Count == 5)
                        //            {

                        //                m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;
                        //                m_bComputed = true;
                        //                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;

                        //                Debug.Log("Case : 7Block");
                        //                break;

                        //            }

                        //            m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;


                        //        }
                        //    }
                        //    //}
                        //}


                        //!< 6개 이상 모인 블록에 하나빼기

                        if (g_nStage >= 3 && m_bComputed == false && m_pAI_PopList_6.Count >= 1)
                        {
                            int nDeckCount = 0;
                            for (nDeckCount = 0; nDeckCount < m_Deck_P2.m_eDeckList.Count; nDeckCount++)
                            {
                                if (m_Deck_P2.m_eDeckList[nDeckCount].GetComponent<MagicBlock>().m_eBlockNumber != m_pAI_PopList_6[0].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                {
                                    m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCount].transform.gameObject;

                                    break;
                                }
                            }

                            //for (int i = 0; i < m_pAI_PopList_6.Count; i++)
                            //{
                            if (nDeckCount >= m_Deck_P2.m_eDeckList.Count)
                            {

                            }
                            else
                            {
                                int k = 0;
                                for (k = 0; k < m_pAI_PopList_6[0].m_BlockList.Count; k++)
                                {
                                    //m_pAI_PopList_6[0].m_BlockList[j].GetComponent
                                    int nAnotherBlockIndex = 0;

                                    m_goAI_SelectBlock = m_pAI_PopList_6[0].m_BlockList[k];

                                    E_BLOCK_NUMBER eSaveBlockNumber = m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber;

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = m_goAI_SelectMagic.GetComponent<MagicBlock>().m_eBlockNumber;

                                    // nAnotherBlockIndex = k + 1;
                                    // if (nAnotherBlockIndex >= m_pAI_PopList_6[0].m_BlockList.Count)
                                    //    nAnotherBlockIndex = 0;

                                    for (int i = 0; i < m_goBlockList.Count; i++)
                                    {
                                        Block pBlock = m_goBlockList[i].GetComponent<Block>();
                                        pBlock.CheckReset();
                                    }

                                    for (int z = 0; z < m_pAI_PopList_6[0].m_BlockList.Count; z++)
                                    {
                                        if (z == k)
                                            continue;

                                        nAnotherBlockIndex = z;

                                        CPopList pCheckList = new CPopList();
                                        pCheckList.Init(SearchBlock(m_pAI_PopList_6[0].m_BlockList[nAnotherBlockIndex].GetComponent<Block>().m_stIndex.m_nX, m_pAI_PopList_6[0].m_BlockList[nAnotherBlockIndex].GetComponent<Block>().m_stIndex.m_nY));

                                        if (pCheckList.m_BlockList.Count == 5)
                                        {

                                            m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;
                                            m_bComputed = true;
                                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;

                                            Debug.Log("Case : 6 Over Block (Code " + k.ToString() + " " + z.ToString() + ")");
                                            break;

                                        }
                                        if (m_bComputed == true)
                                            break;
                                    }
                                    if (m_bComputed == true)
                                        break;

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;


                                }
                            }
                            //}
                        }

                        //!< 4개 모인블록에 하나 붙이기
                        if (g_nStage >= 1 && m_bComputed == false && m_pAI_PopList_4.Count >= 1)
                        {
                            int nDeckCount = 0;


                            for (nDeckCount = 0; nDeckCount < m_Deck_P2.m_eDeckList.Count; nDeckCount++)
                            {
                                if (m_Deck_P2.m_eDeckList[nDeckCount].GetComponent<MagicBlock>().m_eBlockNumber == m_pAI_PopList_4[0].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                {
                                    m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCount].transform.gameObject;
                                    break;
                                }
                            }

                            if (nDeckCount >= m_Deck_P2.m_eDeckList.Count)
                            {


                            }
                            else
                            {
                                E_BLOCK_NUMBER eNowBlockNumber = m_pAI_PopList_4[0].m_BlockList[0].GetComponent<Block>().m_eBlockNumber;
                                for (int i = 0; i < m_pAI_PopList_4[0].m_BlockList.Count; i++)
                                {
                                    int nBlock = 0;

                                    nBlock = SearchBlock(
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nX - 1,
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nY + 0);
                                    if (nBlock != -1)
                                    {
                                        if (m_goBlockList[nBlock].GetComponent<Block>().m_eBlockNumber != eNowBlockNumber)
                                            m_nAI_BlockList_4.Add(nBlock);
                                    }

                                    nBlock = SearchBlock(
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nX + 1,
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nY + 0);
                                    if (nBlock != -1)
                                    {
                                        if (m_goBlockList[nBlock].GetComponent<Block>().m_eBlockNumber != eNowBlockNumber)
                                            m_nAI_BlockList_4.Add(nBlock);
                                    }


                                    nBlock = SearchBlock(
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nX + 0,
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nY + 1);
                                    if (nBlock != -1)
                                    {
                                        if (m_goBlockList[nBlock].GetComponent<Block>().m_eBlockNumber != eNowBlockNumber)
                                            m_nAI_BlockList_4.Add(nBlock);
                                    }

                                    nBlock = SearchBlock(
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nX + 0,
                                        m_pAI_PopList_4[0].m_BlockList[i].GetComponent<Block>().m_stIndex.m_nY - 1);
                                    if (nBlock != -1)
                                    {
                                        if (m_goBlockList[nBlock].GetComponent<Block>().m_eBlockNumber != eNowBlockNumber)
                                            m_nAI_BlockList_4.Add(nBlock);
                                    }

                                }


                                int k = 0;
                                for (k = 0; k < m_nAI_BlockList_4.Count; k++)
                                {


                                    m_goAI_SelectBlock = m_goBlockList[m_nAI_BlockList_4[k]];

                                    E_BLOCK_NUMBER eSaveBlockNumber = m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber;

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = m_goAI_SelectMagic.GetComponent<MagicBlock>().m_eBlockNumber;


                                    for (int i = 0; i < m_goBlockList.Count; i++)
                                    {
                                        Block pBlock = m_goBlockList[i].GetComponent<Block>();
                                        pBlock.CheckReset();
                                    }

                                    CPopList pCheckList = new CPopList();
                                    pCheckList.Init(SearchBlock(m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nX, m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nY));


                                    if (pCheckList.m_BlockList.Count == 5)
                                    {

                                        Debug.Log("Case : 4Block (Code " + k.ToString() + ")");
                                        m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;
                                        m_bComputed = true;
                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;

                                        break;

                                    }

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;


                                }
                            }

                        }




                        //!< 전부 놓아보기
                        if (g_nStage >= 2 && m_bComputed == false)
                        {

                            int nDeckCount = 0;

                            for (nDeckCount = 0; nDeckCount < m_Deck_P2.m_eDeckList.Count; nDeckCount++)
                            {
                                Debug.Log(nDeckCount + "번째 덱에서 에러 /" + m_Deck_P2.m_eDeckList.Count);
                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCount].transform.gameObject;


                                int k = 0;
                                for (k = 0; k < m_goBlockList.Count; k++)
                                {


                                    m_goAI_SelectBlock = m_goBlockList[k];

                                    E_BLOCK_NUMBER eSaveBlockNumber = m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber;

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = m_goAI_SelectMagic.GetComponent<MagicBlock>().m_eBlockNumber;


                                    for (int i = 0; i < m_goBlockList.Count; i++)
                                    {
                                        Block pBlock = m_goBlockList[i].GetComponent<Block>();
                                        pBlock.CheckReset();
                                    }

                                    CPopList pCheckList = new CPopList();
                                    pCheckList.Init(SearchBlock(m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nX, m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nY));


                                    if (pCheckList.m_BlockList.Count == 5)
                                    {

                                        Debug.Log("Case : AllSet (Code " + k.ToString() + ")");
                                        m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;
                                        m_bComputed = true;
                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                        nDeckCount = m_Deck_P2.m_eDeckList.Count;
                                        break;

                                    }

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;

                                }
                            }

                        }



                        //!< 견제하기

                        //!< 4개블록이 있고 적이 덱에 같은색의 블록을 가지고 있다면 4개블록위치에 랜덤으로 착수


                        if (g_nStage >= 4 && m_bComputed == false)
                        {

                            for(int i=0; i<m_pAI_PopList_4.Count; i++)
                            {

                                int nDeckNumberCheck = 0;
                                for (nDeckNumberCheck = 0; nDeckNumberCheck < m_Deck_P2.m_eDeckList.Count; nDeckNumberCheck++)
                                {
                                    if (m_Deck_P2.m_eDeckList[nDeckNumberCheck].m_eBlockNumber != m_pAI_PopList_4[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                        break;
                                }
                                if (nDeckNumberCheck >= m_Deck_P2.m_eDeckList.Count)
                                    continue;



                                for(int j=0; j<m_Deck_P1.m_eDeckList.Count; j++)
                                {

                                    //!< 4개뭉친 블록과 적의 덱의 색이 같음
                                    if (m_pAI_PopList_4[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber == m_Deck_P1.m_eDeckList[j].m_eBlockNumber)
                                    {
                                        int nSelectMagic = 0;
                                        int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4[i].m_BlockList.Count);

                                        while (true)
                                        {
                                            nSelectMagic = UnityEngine.Random.Range(0, m_Deck_P2.m_eDeckList.Count);
                                            //!< 같은 색 못둠
                                            if (m_Deck_P2.m_eDeckList[nSelectMagic].m_eBlockNumber != m_pAI_PopList_4[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                                break;
                                        }
                                        m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nSelectMagic].transform.gameObject;
                                        Debug.Log("[4] " + i + "/" + m_pAI_PopList_4.Count);
                                        m_goAI_SelectBlock = m_pAI_PopList_4[i].m_BlockList[nSelectBlock];
                                        Debug.Log("Case : Attack 4 Block (Code " + nSelectMagic.ToString() + " " + nSelectBlock.ToString() + ")");

                                        m_bComputed = true;
                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                        break;
                                    }

                                }
                                if (m_bComputed == true)
                                    break;
                            }
                        }


                        //!< 고난이도 견제하기

                        //!< 전부 블록을 놓아보고 터지는 경우 터지는 블록을 포함한 자리에 랜덤으로 착수


                        if (g_nStage >= 7 && m_bComputed == false)
                        {

                            int nColorCount = 0;

                            for (nColorCount = 0; nColorCount < (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX; nColorCount++) //!< 모든 색상 전부 넣어봄
                            {
                                //m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nColorCount].transform.gameObject; //!< 덱에서 패를 뽑아 SelectMagic함


                                int k = 0;
                                for (k = 0; k < m_goBlockList.Count; k++) //!< 모든 블록의 위치에 한번씩 놓아봄
                                {


                                    m_goAI_SelectBlock = m_goBlockList[k]; //!< 선택

                                    E_BLOCK_NUMBER eSaveBlockNumber = m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber; //!< 이전 블록색 저장

                                    if (m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber == (E_BLOCK_NUMBER)nColorCount)
                                        continue;

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = (E_BLOCK_NUMBER)nColorCount;
                                    //!< 블록색 갈아치움

                                    for (int i = 0; i < m_goBlockList.Count; i++)
                                    {
                                        Block pBlock = m_goBlockList[i].GetComponent<Block>();
                                        pBlock.CheckReset();
                                    }
                                    //!< 터지는지 확인한다
                                    CPopList pCheckList = new CPopList();
                                    pCheckList.Init(SearchBlock(m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nX, m_goAI_SelectBlock.GetComponent<Block>().m_stIndex.m_nY));


                                    if (pCheckList.m_BlockList.Count == 5) //!< 터질경우
                                    {
                                        bool bChecked = false;
                                        int nDeckNumberCheck = 0;
                                        for (nDeckNumberCheck = 0; nDeckNumberCheck < m_Deck_P1.m_eDeckList.Count; nDeckNumberCheck++)
                                        {
                                            if (m_Deck_P1.m_eDeckList[nDeckNumberCheck].m_eBlockNumber == pCheckList.m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                            {
                                                int nP2Check = 0;
                                                for(nP2Check = 0; nP2Check < m_Deck_P2.m_eDeckList.Count; nP2Check++)
                                                {
                                                    Debug.Log("nP2Check : " + nP2Check.ToString() );

                                                    if (pCheckList.m_BlockList[0].GetComponent<Block>().m_eBlockNumber != m_Deck_P2.m_eDeckList[nP2Check].GetComponent<MagicBlock>().m_eBlockNumber)
                                                    {
                                                        break;
                                                    }
                                                }

                                                if (nP2Check < m_Deck_P2.m_eDeckList.Count)
                                                {
                                                    bChecked = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (bChecked == true)
                                        {
                                            m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;

                                            Debug.Log("Case : Attack Allset (Code " + k.ToString() + ")");

                                            int nSelectMagic = 0;
                                            int nSelectBlock = UnityEngine.Random.Range(0, pCheckList.m_BlockList.Count);

                                            while (true)
                                            {
                                                nSelectMagic = UnityEngine.Random.Range(0, m_Deck_P2.m_eDeckList.Count);
                                                //!< 같은 색 못둠
                                                if (m_Deck_P2.m_eDeckList[nSelectMagic].m_eBlockNumber != pCheckList.m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                                    break;

                                                if (m_Deck_P2.m_eDeckList[0].m_eBlockNumber == m_Deck_P2.m_eDeckList[1].m_eBlockNumber)
                                                    break;
                                            }


                                            while (true)
                                            {
                                                nSelectBlock = UnityEngine.Random.Range(0, pCheckList.m_BlockList.Count);
                                                //!< 두었던 블록이 아니면 빠져나
                                                if (pCheckList.m_BlockList[nSelectBlock] != m_goAI_SelectBlock)
                                                    break;
                                            }

                                            //!< 현재 블록리스트중에 랜덤으로 착수 //!< 단 SelectBlock의 위치는 제외

                                            m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nSelectMagic].transform.gameObject;
                                            m_goAI_SelectBlock = pCheckList.m_BlockList[nSelectBlock];

                                            m_bComputed = true;
                                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                            nColorCount = m_Deck_P2.m_eDeckList.Count;
                                            break;

                                         }

                                     }

                                    m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber = eSaveBlockNumber;
                                }

                                if (m_bComputed == true)
                                    break;
                            }

                        }

                        //!< 3개블록이 있고 적이 덱에 같은색의 블록을 가지고 있다면 4개블록위치에 랜덤으로 착수
                        if (g_nStage >= 5 && m_bComputed == false)
                        {

                            for (int i = 0; i < m_pAI_PopList_3.Count; i++)
                            {
                                int nDeckNumberCheck = 0;
                                for (nDeckNumberCheck = 0; nDeckNumberCheck < m_Deck_P2.m_eDeckList.Count; nDeckNumberCheck++)
                                {
                                    if (m_Deck_P2.m_eDeckList[nDeckNumberCheck].m_eBlockNumber != m_pAI_PopList_3[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                        break;
                                }
                                if (nDeckNumberCheck >= m_Deck_P2.m_eDeckList.Count)
                                    continue;


                                for (int j = 0; j < m_Deck_P1.m_eDeckList.Count; j++)
                                {

                                    //!< 3개뭉친 블록과 적의 덱의 색이 같음
                                    if (m_pAI_PopList_3[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber == m_Deck_P1.m_eDeckList[j].m_eBlockNumber)
                                    {
                                        int nSelectMagic = 0;
                                        int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_3[i].m_BlockList.Count);

                                        while (true)
                                        {
                                            nSelectMagic = UnityEngine.Random.Range(0, m_Deck_P2.m_eDeckList.Count);
                                            //!< 같은 색 못둠
                                            if (m_Deck_P2.m_eDeckList[nSelectMagic].m_eBlockNumber != m_pAI_PopList_3[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
                                                break;
                                        }


                                        m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nSelectMagic].transform.gameObject;
                                        Debug.Log("[3] "+ i + "/" + m_pAI_PopList_3.Count);
                                        m_goAI_SelectBlock = m_pAI_PopList_3[i].m_BlockList[nSelectBlock];
                                        Debug.Log("Case : Attack 3 Block (Code " + nSelectMagic.ToString() + " " + nSelectBlock.ToString() + ")");

                                        m_bComputed = true;
                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                        break;
                                    }

                                }
                            }
                        }

                        //!< 다음 한 수를 멀리 내다봄

                        /* int nSelectMagic = UnityEngine.Random.Range(0, 2);
                                        int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4.Count);

                                        m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nSelectMagic].transform.gameObject;
                                        m_goAI_SelectBlock = m_pAI_PopList_4[i].m_BlockList[nSelectBlock];
                                        Debug.Log("Case : Attack 4 Block (Code " + nSelectMagic.ToString() + nSelectBlock.ToString() + ")");

                                        m_bComputed = true;
                                        m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                        */

                        //!< 3개 블록이 있고 적이 덱에 같은색의 블록을 가지고 있지 않으며 나에게 그 모인 색의 블록이 있다면 이어서 착수
                        if (g_nStage >= 6 && m_bComputed == false)
                        {
                            int nDeckCheck = 0;
                            int nDeckCheck2 = 0;
                            for (int i = 0; i < m_pAI_PopList_3.Count; i++)
                            {
                                for (nDeckCheck = 0; nDeckCheck < m_Deck_P1.m_eDeckList.Count; nDeckCheck++)
                                {
                                    //!< 3개뭉친 블록과 적의 덱의 색이 전부 같지 않음 (같으면 빠져나감)
                                    if (m_pAI_PopList_3[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber == m_Deck_P1.m_eDeckList[nDeckCheck].m_eBlockNumber)
                                        break;

                                }
                                for (nDeckCheck2 = 0; nDeckCheck2 < m_Deck_P2.m_eDeckList.Count; nDeckCheck2++)
                                {
                                    //!< 3개뭉친 블록의 색이 나에게 있음
                                    if (m_pAI_PopList_3[i].m_BlockList[0].GetComponent<Block>().m_eBlockNumber == m_Deck_P2.m_eDeckList[nDeckCheck2].m_eBlockNumber)
                                        break;

                                }
                                if (nDeckCheck >= m_Deck_P1.m_eDeckList.Count && nDeckCheck2 < m_Deck_P2.m_eDeckList.Count )
                                {
                                    for (int n = 0; n < m_pAI_PopList_3[i].m_BlockList.Count; n++)
                                    {
                                        int nBlock = 0;
                                        int nX = 0;
                                        int nY = 0;

                                        bool nPass = true;


                                        //!< Check /////////////////////////////////
                                        nPass = true;
                                        nX = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX - 1;
                                        nY = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 0;

                                        for (int u = 0; u < m_pAI_PopList_3[i].m_BlockList.Count; u++)
                                        {
                                            if (nX == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nX &&
                                                nY == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nY) //!< 인덱스가 블록묶음 자신과 겹치면 Pass를 false함
                                            {
                                                nPass = false;
                                                break;
                                            }
                                        }
                                        // ////////////////////////////////////////

                                        if (nPass == true)
                                        {
                                            nBlock = SearchBlock(
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX - 1,
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 0);

                                            if (nBlock != -1)
                                            {
                                                int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4.Count);

                                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCheck2].transform.gameObject;
                                                m_goAI_SelectBlock = m_goBlockList[nBlock];

                                                Debug.Log("Case : Flow 3 Block (Code " + nDeckCheck2.ToString() + " " + nBlock.ToString() + ")");

                                                m_bComputed = true;
                                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                                break;
                                            }
                                        }


                                        //!< Check /////////////////////////////////
                                        nPass = true;
                                        nX = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 1;
                                        nY = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 0;

                                        for (int u = 0; u < m_pAI_PopList_3[i].m_BlockList.Count; u++)
                                        {
                                            if (nX == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nX &&
                                                nY == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nY) //!< 인덱스가 블록묶음 자신과 겹치면 Pass를 false함
                                            {
                                                nPass = false;
                                                break;
                                            }
                                        }
                                        // ////////////////////////////////////////

                                        if (nPass == true)
                                        {
                                            nBlock = SearchBlock(
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 1,
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 0);
                                            if (nBlock != -1)
                                            {
                                                int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4.Count);

                                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCheck2].transform.gameObject;
                                                m_goAI_SelectBlock = m_goBlockList[nBlock];

                                                Debug.Log("Case : Flow 3 Block (Code " + nDeckCheck2.ToString() + " " + nBlock.ToString() + ")");

                                                m_bComputed = true;
                                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                                break;
                                            }
                                        }


                                        //!< Check /////////////////////////////////
                                        nPass = true;
                                        nX = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 0;
                                        nY = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY - 1;

                                        for (int u = 0; u < m_pAI_PopList_3[i].m_BlockList.Count; u++)
                                        {
                                            if (nX == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nX &&
                                                nY == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nY) //!< 인덱스가 블록묶음 자신과 겹치면 Pass를 false함
                                            {
                                                nPass = false;
                                                break;
                                            }
                                        }
                                        // ////////////////////////////////////////

                                        if (nPass == true)
                                        {
                                            nBlock = SearchBlock(
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 0,
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY - 1);
                                            if (nBlock != -1)
                                            {
                                                int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4.Count);

                                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCheck2].transform.gameObject;
                                                m_goAI_SelectBlock = m_goBlockList[nBlock];

                                                Debug.Log("Case : Flow 3 Block (Code " + nDeckCheck2.ToString() + " " + nBlock.ToString() + ")");

                                                m_bComputed = true;
                                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                                break;
                                            }
                                        }

                                        //!< Check /////////////////////////////////
                                        nPass = true;
                                        nX = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 0;
                                        nY = m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 1;

                                        for (int u = 0; u < m_pAI_PopList_3[i].m_BlockList.Count; u++)
                                        {
                                            if (nX == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nX &&
                                                nY == m_pAI_PopList_3[i].m_BlockList[u].GetComponent<Block>().m_stIndex.m_nY) //!< 인덱스가 블록묶음 자신과 겹치면 Pass를 false함
                                            {
                                                nPass = false;
                                                break;
                                            }
                                        }
                                        // ////////////////////////////////////////

                                        if (nPass == true)
                                        {
                                            nBlock = SearchBlock(
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nX + 0,
                                                m_pAI_PopList_3[i].m_BlockList[n].GetComponent<Block>().m_stIndex.m_nY + 1);
                                            if (nBlock != -1)
                                            {
                                                int nSelectBlock = UnityEngine.Random.Range(0, m_pAI_PopList_4.Count);

                                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nDeckCheck2].transform.gameObject;
                                                m_goAI_SelectBlock = m_goBlockList[nBlock];

                                                Debug.Log("Case : Flow 3 Block (Code " + nDeckCheck2.ToString() + " " + nBlock.ToString() + ")");

                                                m_bComputed = true;
                                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                                                break;
                                            }
                                        }

                                        if (m_bComputed == true)
                                            break;
                                    }

                                }

                            if (m_bComputed == true)
                                break;
                            }   
                        }


                        //!< 랜덤하게 놓기

                        if (m_bComputed == false)
                        {
                            Debug.Log("Case : Random");

                            while (true)
                            {
                                int nRandomMagic = UnityEngine.Random.Range(0, m_Deck_P2.m_eDeckList.Count);
                                int nRandomBlock = UnityEngine.Random.Range(0, m_goBlockList.Count);
                                m_goAI_SelectMagic = m_Deck_P2.m_eDeckList[nRandomMagic].transform.gameObject;
                                m_goAI_SelectBlock = m_goBlockList[nRandomBlock];

                                //!< 같은 색상의 블록에는 놓을 수 없다.
                                if (m_goAI_SelectBlock.GetComponent<Block>().m_eBlockNumber != m_goAI_SelectMagic.GetComponent<MagicBlock>().m_eBlockNumber)
                                {
                                    Debug.Log("Case : Random <- ColorSet");
                                    break;
                                }

                            }


                            m_bComputed = true;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;

                        }







                        m_fAI_Select_Magic_Time = UnityEngine.Random.Range(m_fAI_Select_Magic_Time_Min, m_fAI_Select_Magic_Time_Max);
                        m_fAI_Select_Block_Time = UnityEngine.Random.Range(m_fAI_Select_Block_Time_Min, m_fAI_Select_Block_Time_Max);


                        //!< 계산시에 사용한 Object들 초기화

                        m_pAI_PopList_3.Clear();
                        m_pAI_PopList_4.Clear();
                        m_pAI_PopList_6.Clear();
                        //m_pAI_PopList_7.Clear();
                    }

                    //!< 어떤 색의 Magic블록을 선택하시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC)
                    {
                        m_fTimeLimitTimer += Time.deltaTime;
                        if (m_fTimeLimitTimer >= m_fTimeLimitMax)
                        {
                            m_goP1_Think.active = true;
                            m_goP2_Think.active = false;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
                            m_fTimeLimitTimer = 0.0f;
                        }
                        //!< 선택된 매직블록을 선택 후 플레이어처럼 행동합니다.
                        m_fAI_Select_Magic_Timer += Time.deltaTime;
                        if (m_fAI_Select_Magic_Timer >= m_fAI_Select_Magic_Time)
                        {
                            m_fAI_Select_Magic_Timer = 0.0f;
                            m_goSelectMagic.transform.position = new Vector3(m_goAI_SelectMagic.transform.position.x, m_goAI_SelectMagic.transform.position.y, m_goSelectMagic.transform.position.z);
                            m_SelectMagic_P2 = m_goAI_SelectMagic.GetComponent<MagicBlock>();
                            g_SoundMng.Play(1);

                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK;
                        }

                    }

                    //!< 어떤 블록에 Magic을 적용하시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK)
                    {
                        m_fTimeLimitTimer += Time.deltaTime;
                        if (m_fTimeLimitTimer >= m_fTimeLimitMax)
                        {
                            m_goP1_Think.active = true;
                            m_goP2_Think.active = false;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
                            m_fTimeLimitTimer = 0.0f;
                        }
                        //!< 플레이어처럼 행동합니다.
                        m_fAI_Select_Block_Timer += Time.deltaTime;
                        if (m_fAI_Select_Block_Timer >= m_fAI_Select_Block_Time)
                        {
                            m_fAI_Select_Block_Timer = 0.0f;

                            m_goSelectMagic.transform.position = new Vector3(m_goAI_SelectBlock.transform.position.x, m_goAI_SelectBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                            //m_SelectMagic_P1 = pBlock;

                            m_goAI_SelectBlock.GetComponent<Block>().ChangeNumber(m_SelectMagic_P2.m_eBlockNumber);
                            m_Deck_P2.m_eDeckList.Remove(m_SelectMagic_P2);

                            //Destroy(m_SelectMagic_P1.transform.gameObject);
                            m_SelectMagic_P2.AlphaDestroy();

                            Draw(2);

                            m_goAI_Think.active = false;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC;
                            m_fTimeLimitTimer = 0.0f;
                            g_SoundMng.Play(2);
                        }

                    }
                    //!< 터트려 봅시다
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC)
                    {
                        if (m_fLogic_Pop_Delay <= m_fLogic_Pop_Delay_Time)
                        {
                            m_fLogic_Pop_Delay += Time.deltaTime;
                        }
                        else
                        {
                            Logic_Pop();
                            m_goSelectMagic.transform.position = new Vector3(-4.0f, m_goSelectMagic.transform.position.y, m_goSelectMagic.transform.position.z);

                            Fill_Block();
                            m_fLogic_Pop_Delay = 0.0f;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_CHECK;

                        }
                    }

                    //!< 다시 터트릴 수 있습니까? Yes or No
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_POP_CHECK)
                    {
                        if (m_bPoped == true)
                        {
                            if (Fill_Block_End() == true)
                            {
                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC;
                            }
                        }
                        else if (m_bPoped == false)
                        {
                            m_goP1_Think.active = true;
                            m_goP2_Think.active = false;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
                            g_nTurn++;
                            if (g_nTurn > g_nTargetTurn)
                            {
                                g_nTurn--;
                                Game_End();
                            }
                        }
                    }


                }


                //!< 여기서부터 배틀모드입니다.

                //!< 네트워크 사용에서의 주의를 요합니다!

                else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
                {

                    //!< 어떤색의 Magic블록을 선택하시겠습니까?
                    if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC)
                    {
                        Check_TimeOver_P1();





                        //!< 네트워크 패킷 리스트 & 처리 ////////////////////////////////

                        if (BattleMenuScene.g_P1_OrderToSelectMagicBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P1_OrderToSelectMagicBlockList[0].m_nDeckIndex;

                            //!< 액션
                            
                            m_goSelectMagic.transform.position = new Vector3(m_Deck_P1.m_eDeckList[nDrawIndex].transform.position.x, m_Deck_P1.m_eDeckList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);
                            m_SelectMagic_P1 = m_Deck_P1.m_eDeckList[nDrawIndex];
                            g_SoundMng.Play(1);
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK;
                            
                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P1_OrderToSelectMagicBlockList.Remove(BattleMenuScene.g_P1_OrderToSelectMagicBlockList[0]);
                        }

                        //!<   //////// 내가 플레이어1 일 경우////////////////////////
                        if (g_nMyPlayer == 1)
                        {
                            if (Input.GetMouseButtonDown(0) == true)
                            {
                                RaycastHit hit;
                                Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(m_Ray, out hit))
                                {
                                    //Vector3 pos = hit.point;
                                    //!< 패를 선택합니다
                                    if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                    {
                                        MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();
                                        if (pBlock.m_nPlayer == 1) //!< 1플레이어의 패일경우
                                        {
                                            int  i =0;
                                            for (i = 0; i < m_Deck_P1.m_eDeckList.Count; i++)
                                            {
                                                if (m_Deck_P1.m_eDeckList[i] == pBlock)
                                                    break;
                                            }
                                            //!< 패 선택정보 전송
                                            //!< 필요한 정보는? 1플레이어의 배열 Index번째 패 m_ePlayStep의 이동여부
                                            strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_SELECT_MB_FROM_CLIENT).ToString(), i);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);


                                            // //!<

                                            // m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            // m_SelectMagic_P1 = pBlock;
                                            // g_SoundMng.Play(1);
                                            // m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK;

                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }

                                    //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                    //Destroy(hit.collider.gameObject);
                                    //CalcTarget();
                                }
                            }
                        }


                    }
                    //!< 어디에 두시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK)
                    {
                        Check_TimeOver_P1();

                        //!< 네트워크 패킷 리스트 & 처리 ////////////////////////////////

                        if (BattleMenuScene.g_P1_OrderToSelectMagicBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P1_OrderToSelectMagicBlockList[0].m_nDeckIndex;

                            //!< 액션
                            
                            m_goSelectMagic.transform.position = new Vector3(m_Deck_P1.m_eDeckList[nDrawIndex].transform.position.x, m_Deck_P1.m_eDeckList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);
                            m_SelectMagic_P1 = m_Deck_P1.m_eDeckList[nDrawIndex];
                            g_SoundMng.Play(1);
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK;
                            
                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P1_OrderToSelectMagicBlockList.Remove(BattleMenuScene.g_P1_OrderToSelectMagicBlockList[0]);
                        }

                        if (BattleMenuScene.g_P1_OrderToPutBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P1_OrderToPutBlockList[0].m_nBlockIndex;
                            E_BLOCK_NUMBER eBlockNumber = BattleMenuScene.g_P1_OrderToPutBlockList[0].m_eBlockNumber;

                            //!< 액션

                            m_goSelectMagic.transform.position = new Vector3(m_goBlockList[nDrawIndex].transform.position.x, m_goBlockList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);

                            m_goBlockList[nDrawIndex].GetComponent<Block>().ChangeNumber(m_SelectMagic_P1.m_eBlockNumber);
                            m_Deck_P1.m_eDeckList.Remove(m_SelectMagic_P1);

                            m_SelectMagic_P1.AlphaDestroy();

                            //Draw(1);

                            ////////// Draw P1
                            if (g_nMyPlayer == 1)
                            {
                                int nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
                                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT).ToString(), nRandomBlockNumber);
                                BattleMenuScene.SendMessageToServer(strTemp);
                            }
                            /////////

                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC;
                            m_fTimeLimitTimer = 0.0f;
                            g_SoundMng.Play(2);

                            
                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P1_OrderToPutBlockList.Remove(BattleMenuScene.g_P1_OrderToPutBlockList[0]);
                        }

                        //!<   //////// 내가 플레이어1 일 경우////////////////////////
                        if (g_nMyPlayer == 1)
                        {
                            if (Input.GetMouseButtonDown(0) == true)
                            {
                                RaycastHit hit;
                                Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(m_Ray, out hit))
                                {
                                    //Vector3 pos = hit.point;
                                    if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                    {
                                        //Debug.Log("StepB1");
                                        MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();
                                        if (pBlock.m_nPlayer == 1)
                                        {

                                            int i = 0;
                                            for (i = 0; i < m_Deck_P1.m_eDeckList.Count; i++)
                                            {
                                                if (m_Deck_P1.m_eDeckList[i] == pBlock)
                                                    break;
                                            }
                                            //!< 패 선택정보 전송
                                            //!< 필요한 정보는? 1플레이어의 배열 Index번째 패 m_ePlayStep의 이동여부
                                            strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_SELECT_MB_FROM_CLIENT).ToString(), i);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);

                                            // m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            // m_SelectMagic_P1 = pBlock;
                                            // g_SoundMng.Play(1);


                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }
                                    else if (hit.collider.gameObject.GetComponent<Block>() != null) //!< 실제로 블록을 놓는 부분
                                    {
                                        //Debug.Log("StepB2");

                                        
                                        Block pBlock = hit.collider.gameObject.GetComponent<Block>();
                                        if (m_SelectMagic_P1.m_eBlockNumber != pBlock.m_eBlockNumber)
                                        {

                                            int i = 0;
                                            for (i = 0; i < m_goBlockList.Count; i++)
                                            {
                                                if (m_goBlockList[i].GetComponent<Block>().m_stIndex.m_nX == pBlock.m_stIndex.m_nX &&
                                                    m_goBlockList[i].GetComponent<Block>().m_stIndex.m_nY == pBlock.m_stIndex.m_nY)
                                                    break;
                                            }
                                            //!< 블록 선택정보 전송
                                            //!< 블록을 놓는데 필요한 정보는? 선택된 블록 Index, 바꿀 색Index
                                            strTemp = string.Format("{0}{1}Γ{2}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_PUT_BLOCK_FROM_CLIENT).ToString(), i, (int)m_SelectMagic_P1.m_eBlockNumber);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);


                                            //!< 블록을 놓는데 필요한 정보는? 선택된 블록 Index, 바꿀 색Index
                                            //m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            
                                            //pBlock.ChangeNumber(m_SelectMagic_P1.m_eBlockNumber);
                                            //m_Deck_P1.m_eDeckList.Remove(m_SelectMagic_P1);

                                            //m_SelectMagic_P1.AlphaDestroy();

                                            //Draw(1);

                                            //m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC;
                                            //m_fTimeLimitTimer = 0.0f;
                                            //g_SoundMng.Play(2);


                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }

                                    //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                    //Destroy(hit.collider.gameObject);
                                    //CalcTarget();
                                }
                            }
                        }
                    }
                    //!< 터트려 봅시다
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC)
                    {
                        if (m_fLogic_Pop_Delay <= m_fLogic_Pop_Delay_Time)
                        {
                            m_fLogic_Pop_Delay += Time.deltaTime;
                        }
                        else
                        {
                            Logic_Pop();
                            m_goSelectMagic.transform.position = new Vector3(-4.0f, m_goSelectMagic.transform.position.y, m_goSelectMagic.transform.position.z);

                            Fill_Block();
                            m_fLogic_Pop_Delay = 0.0f;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_CHECK;

                        }
                    }
                    //!< 다시 터트릴 수 있습니까? Yes or No
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_POP_CHECK)
                    {
                        if (m_bPoped == true)
                        {
                            if (Fill_Block_End() == true)
                            {
                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC;
                            }
                        }
                        else if (m_bPoped == false)
                        {
                            m_goP1_Think.active = false;
                            m_goP2_Think.active = true;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC;
                        }
                    }

                    //!< 어떤색의 Magic블록을 선택하시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_MAGIC)
                    {
                        Check_TimeOver_P2();

                        //!< 네트워크 패킷 리스트 & 처리 ////////////////////////////////

                        if (BattleMenuScene.g_P2_OrderToSelectMagicBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P2_OrderToSelectMagicBlockList[0].m_nDeckIndex;

                            //!< 액션

                            m_goSelectMagic.transform.position = new Vector3(m_Deck_P2.m_eDeckList[nDrawIndex].transform.position.x, m_Deck_P2.m_eDeckList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);
                            m_SelectMagic_P2 = m_Deck_P2.m_eDeckList[nDrawIndex];
                            g_SoundMng.Play(1);
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK;

                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P2_OrderToSelectMagicBlockList.Remove(BattleMenuScene.g_P2_OrderToSelectMagicBlockList[0]);
                        }

                        //!<   //////// 내가 플레이어2 일 경우////////////////////////
                        if (g_nMyPlayer == 2)
                        {
                            if (Input.GetMouseButtonDown(0) == true)
                            {
                                RaycastHit hit;
                                Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(m_Ray, out hit))
                                {
                                    //Vector3 pos = hit.point;
                                    if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                    {
                                        MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();
                                        if (pBlock.m_nPlayer == 2)
                                        {
                                            int i = 0;
                                            for (i = 0; i < m_Deck_P2.m_eDeckList.Count; i++)
                                            {
                                                if (m_Deck_P2.m_eDeckList[i] == pBlock)
                                                    break;
                                            }
                                            //!< 패 선택정보 전송
                                            //!< 필요한 정보는? 1플레이어의 배열 Index번째 패 m_ePlayStep의 이동여부
                                            strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_SELECT_MB_FROM_CLIENT).ToString(), i);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);


                                            //m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            //m_SelectMagic_P2 = pBlock;
                                            //g_SoundMng.Play(1);

                                            //m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK;
                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }

                                    //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                    //Destroy(hit.collider.gameObject);
                                    //CalcTarget();
                                }
                            }
                        }
                    }
                    //!< 어디에 두시겠습니까?
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK)
                    {
                        Check_TimeOver_P2();

                        //!< 네트워크 패킷 리스트 & 처리 ////////////////////////////////

                        if (BattleMenuScene.g_P2_OrderToSelectMagicBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P2_OrderToSelectMagicBlockList[0].m_nDeckIndex;

                            //!< 액션

                            m_goSelectMagic.transform.position = new Vector3(m_Deck_P2.m_eDeckList[nDrawIndex].transform.position.x, m_Deck_P2.m_eDeckList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);
                            m_SelectMagic_P2 = m_Deck_P2.m_eDeckList[nDrawIndex];
                            g_SoundMng.Play(1);
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_SELECT_BLOCK;

                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P2_OrderToSelectMagicBlockList.Remove(BattleMenuScene.g_P2_OrderToSelectMagicBlockList[0]);
                        }


                        if (BattleMenuScene.g_P2_OrderToPutBlockList.Count > 0)
                        {
                            int nDrawIndex = BattleMenuScene.g_P2_OrderToPutBlockList[0].m_nBlockIndex;
                            E_BLOCK_NUMBER eBlockNumber = BattleMenuScene.g_P2_OrderToPutBlockList[0].m_eBlockNumber;

                            //!< 액션

                            m_goSelectMagic.transform.position = new Vector3(m_goBlockList[nDrawIndex].transform.position.x, m_goBlockList[nDrawIndex].transform.position.y, m_goSelectMagic.transform.position.z);

                            m_goBlockList[nDrawIndex].GetComponent<Block>().ChangeNumber(m_SelectMagic_P2.m_eBlockNumber);
                            m_Deck_P2.m_eDeckList.Remove(m_SelectMagic_P2);

                            m_SelectMagic_P2.AlphaDestroy();

                            //Draw(2);
                             ////////// Draw P1
                            if (g_nMyPlayer == 2)
                            {
                                int nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
                                strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT).ToString(), nRandomBlockNumber);
                                BattleMenuScene.SendMessageToServer(strTemp);
                            }

                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC;
                            m_fTimeLimitTimer = 0.0f;
                            g_SoundMng.Play(2);


                            //!< 리스트에서 명령 삭제
                            BattleMenuScene.g_P2_OrderToPutBlockList.Remove(BattleMenuScene.g_P2_OrderToPutBlockList[0]);
                        }
                        //!<   //////// 내가 플레이어2 일 경우////////////////////////
                        if (g_nMyPlayer == 2)
                        {
                            if (Input.GetMouseButtonDown(0) == true)
                            {
                                RaycastHit hit;
                                Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(m_Ray, out hit))
                                {
                                    //Vector3 pos = hit.point;
                                    if (hit.collider.gameObject.GetComponent<MagicBlock>() != null)
                                    {
                                        //Debug.Log("StepB1");
                                        MagicBlock pBlock = hit.collider.gameObject.GetComponent<MagicBlock>();
                                        if (pBlock.m_nPlayer == 2)
                                        {
                                            int i = 0;
                                            for (i = 0; i < m_Deck_P2.m_eDeckList.Count; i++)
                                            {
                                                if (m_Deck_P2.m_eDeckList[i] == pBlock)
                                                    break;
                                            }
                                            //!< 패 선택정보 전송
                                            //!< 필요한 정보는? 1플레이어의 배열 Index번째 패 m_ePlayStep의 이동여부
                                            strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_SELECT_MB_FROM_CLIENT).ToString(), i);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);
                                            //m_goSelectMagic.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, m_goSelectMagic.transform.position.z);
                                            //m_SelectMagic_P2 = pBlock;
                                            //g_SoundMng.Play(1);
                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }
                                    else if (hit.collider.gameObject.GetComponent<Block>() != null) //!< 실제로 블록을 놓는 부분
                                    {
                                        //Debug.Log("StepB2");
                                        Block pBlock = hit.collider.gameObject.GetComponent<Block>();
                                        if (m_SelectMagic_P2.m_eBlockNumber != pBlock.m_eBlockNumber)
                                        {

                                            int i = 0;
                                            for (i = 0; i < m_goBlockList.Count; i++)
                                            {
                                                if (m_goBlockList[i].GetComponent<Block>().m_stIndex.m_nX == pBlock.m_stIndex.m_nX &&
                                                    m_goBlockList[i].GetComponent<Block>().m_stIndex.m_nY == pBlock.m_stIndex.m_nY)
                                                    break;
                                            }
                                            //!< 블록 선택정보 전송
                                            //!< 블록을 놓는데 필요한 정보는? 선택된 블록 Index, 바꿀 색Index
                                            strTemp = string.Format("{0}{1}Γ{2}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_PUT_BLOCK_FROM_CLIENT).ToString(), i, (int)m_SelectMagic_P2.m_eBlockNumber);
                                            BattleMenuScene.SendMessageToServer(strTemp);
                                            Console.Write(strTemp);

                                        }
                                        //Fire.m_stPos = NewMng.New_Vector3(Fire.m_stPos.x + 5.0f, Fire.m_stPos.y + +5.0f, Fire.m_stPos.z);
                                    }

                                    //m_TargetObject.transform.position = NewMng.New_Vector3(m_TargetObject.transform.position.x, m_TargetObject.transform.position.y + 1.0f, m_TargetObject.transform.position.z);
                                    //Destroy(hit.collider.gameObject);
                                    //CalcTarget();
                                }
                            }
                        }
                    }
                    //!< 터트려 봅시다
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC)
                    {
                        if (m_fLogic_Pop_Delay <= m_fLogic_Pop_Delay_Time)
                        {
                            m_fLogic_Pop_Delay += Time.deltaTime;
                        }
                        else
                        {
                            Logic_Pop();
                            m_goSelectMagic.transform.position = new Vector3(-4.0f, m_goSelectMagic.transform.position.y, m_goSelectMagic.transform.position.z);

                            Fill_Block();
                            m_fLogic_Pop_Delay = 0.0f;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_CHECK;

                        }
                    }
                    //!< 다시 터트릴 수 있습니까? Yes or No
                    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_POP_CHECK)
                    {
                        if (m_bPoped == true)
                        {
                            if (Fill_Block_End() == true)
                            {
                                m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC;
                            }
                        }
                        else if (m_bPoped == false)
                        {
                            m_goP1_Think.active = true;
                            m_goP2_Think.active = false;
                            m_ePlayStep = E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_MAGIC;
                            g_nTurn++;
                            if (g_nTurn > g_nTargetTurn)
                            {
                                g_nTurn--;
                                Game_End();
                            }
                        }
                    }


                }

            }
            else if (m_eGameStep == E_GAME_STEP.E_GAME_STEP_END)
            {
                
            }
        }

    }

    void Game_End()
    {
        m_eGameStep = E_GAME_STEP.E_GAME_STEP_END;
        if (g_nScore_P1 > g_nScore_P2)
        {
            m_eGameResult = E_GAME_RESULT.E_GAME_RESULT_VICTORY;
            m_goEnd_Win.active = true;

            //!< 승리 골드 획득
            g_nGold += 600;
            GameSateData.SaveData();

            //
            g_SoundMng.Play(4);


        }
        else if (g_nScore_P2 > g_nScore_P1)
        {
            m_eGameResult = E_GAME_RESULT.E_GAME_RESULT_DEFEAT;
            m_goEnd_Lose.active = true;
            g_SoundMng.Play(5);
        }
        else if (g_nScore_P2 == g_nScore_P1)
        {
            m_eGameResult = E_GAME_RESULT.E_GAME_RESULT_DRAW;
            m_goEnd_Draw.active = true;
            g_SoundMng.Play(6);
        }
        
        //m_goEnd.transform.position = NewMng.New_Vector3(0.0f, m_goEnd.transform.position.y, m_goEnd.transform.position.z);

        m_goEnd_ButtonMainMenu.active = true;
        m_goEnd_ButtonBack.active = true;
        //m_goEnd_ButtonPause.active = true;
        m_goUIBlind.active = true;


    }

    void AI_Pop()
    {
        for (int r = 0; r < (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX; r++)
        {
            for (int i = 0; i < m_goBlockList.Count; i++)
            {
                Block pBlock = m_goBlockList[i].GetComponent<Block>();
                pBlock.CheckReset();
                pBlock.SetItem(r);
            }

            for (int i = 0; i < m_goBlockList.Count; i++)
            {
                CPopList pPopList = new CPopList();
                pPopList.Init(i);

                if (pPopList.m_BlockList.Count == 3)
                {
                    m_pAI_PopList_3.Add(pPopList);
                }

                if (pPopList.m_BlockList.Count == 4)
                {
                    m_pAI_PopList_4.Add(pPopList);

                }

                if (pPopList.m_BlockList.Count >= 6)
                {
                    m_pAI_PopList_6.Add(pPopList);

                }
                //if (pPopList.m_BlockList.Count == 7)
                //{
                //    m_pAI_PopList_7.Add(pPopList);
                //}
                //for (int j = 0; j < m_goBlockList.Count; j++)
                //{
                //    Block pBlock = m_goBlockList[j].GetComponent<Block>();
                //    pBlock.ItemReset();
                //}
            }
        }
    }

    void Logic_Pop()
    {
        m_bPoped = false;

        for (int r = 0; r < (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX; r++)
        {
            for (int i = 0; i < m_goBlockList.Count; i++)
            {
                Block pBlock = m_goBlockList[i].GetComponent<Block>();
                pBlock.CheckReset();
                pBlock.SetItem(r);
            }

            for (int i = 0; i < m_goBlockList.Count; i++)
            {
                CPopList pPopList = new CPopList();
                pPopList.Init(i);
                string strTemp2 = "";
                if (pPopList.m_BlockList.Count == 5)
                {
                    m_bPoped = true;


                    if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN && g_nMyPlayer == 1)
                    {

                        strTemp2 += string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_CREATE_BLOCK_FROM_CLIENT).ToString());
                    }
                    for (int j = 0; j < pPopList.m_BlockList.Count; j++)
                    {
                        //!< 터진블록의 X,Y 좌표 및 생성될 블록 색 정보 결정
                        if (g_nMyPlayer == 1)
                        {
                            int nX = pPopList.m_BlockList[j].GetComponent<Block>().m_stIndex.m_nX;
                            int nY = pPopList.m_BlockList[j].GetComponent<Block>().m_stIndex.m_nY;
                            int nBlockNumber = UnityEngine.Random.Range(0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);

                            strTemp2 += string.Format("{0}{1}{2}", nX, nY, nBlockNumber);
                            
                        }
                        PopBlock(pPopList.m_BlockList[j]);
                       

                        if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_POP_LOGIC)
                        {
                            g_nScore_P1++;
                        }
                        else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P2_POP_LOGIC)
                        {
                            g_nScore_P2++;
                        }

                        ////g_nScore++;
                        //if (j > 3)
                        //{
                        //    //g_nScore += j - 3;
                        //    if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK)
                        //    {

                        //        g_nScore_P1++;
                        //    }
                        //    else if (m_ePlayStep == E_PLAY_STEP.E_PLAY_STEP_P1_SELECT_BLOCK)
                        //    {

                        //    }

                        //}
                    }
                    if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN && g_nMyPlayer == 1)
                    {
                        BattleMenuScene.SendMessageToServer(strTemp2);
                    }
                    g_SoundMng.Play(3);
                }

                //for (int j = 0; j < m_goBlockList.Count; j++)
                //{
                //    Block pBlock = m_goBlockList[i].GetComponent<Block>();
                //    pBlock.ItemReset();
                //}
                //Block pBlock = m_goBlockList[i].GetComponent<Block>();
                //pBlock.CheckReset();
            }
        }


    }

    public void Draw(int nPlayer)
    {
        int nRandomBlockNumber = 0;
        switch (nPlayer)
        {
            case 1:
                nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);

                GameObject CreateObject = (GameObject)Instantiate(m_goMagicBlock[nRandomBlockNumber]);
                //CreateObject.transform.parent = GameObject.Find("Panel").transform;
                MagicBlock ThisBlock = CreateObject.GetComponent<MagicBlock>();
                ThisBlock.CreateBlock((E_BLOCK_NUMBER)nRandomBlockNumber);
                CreateObject.transform.position = NewMng.New_Vector3(4.0f,0.46f,-2.0f);
                ThisBlock.m_nPlayer = 1;
                m_Deck_P1.m_eDeckList.Add(ThisBlock);
                break;

            case 2:
                nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);

                GameObject CreateObject2 = (GameObject)Instantiate(m_goMagicBlock[nRandomBlockNumber]);
                //CreateObject.transform.parent = GameObject.Find("Panel").transform;
                MagicBlock ThisBlock2 = CreateObject2.GetComponent<MagicBlock>();
                ThisBlock2.CreateBlock((E_BLOCK_NUMBER)nRandomBlockNumber);
                CreateObject2.transform.position = NewMng.New_Vector3(-4.0f, 4.64f, -2.0f);
                ThisBlock2.m_nPlayer = 2;
                m_Deck_P2.m_eDeckList.Add(ThisBlock2);
                break;
        }
    }
    public void Draw(int nPlayer, int nBlockNumber)
    {
        int nRandomBlockNumber = 0;
        switch (nPlayer)
        {
            case 1:
                nRandomBlockNumber = nBlockNumber;

                GameObject CreateObject = (GameObject)Instantiate(m_goMagicBlock[nRandomBlockNumber]);
                //CreateObject.transform.parent = GameObject.Find("Panel").transform;
                MagicBlock ThisBlock = CreateObject.GetComponent<MagicBlock>();
                ThisBlock.CreateBlock((E_BLOCK_NUMBER)nRandomBlockNumber);
                CreateObject.transform.position = NewMng.New_Vector3(4.0f, 0.46f, -2.0f);
                ThisBlock.m_nPlayer = 1;
                m_Deck_P1.m_eDeckList.Add(ThisBlock);
                break;

            case 2:
                nRandomBlockNumber = nBlockNumber;

                GameObject CreateObject2 = (GameObject)Instantiate(m_goMagicBlock[nRandomBlockNumber]);
                //CreateObject.transform.parent = GameObject.Find("Panel").transform;
                MagicBlock ThisBlock2 = CreateObject2.GetComponent<MagicBlock>();
                ThisBlock2.CreateBlock((E_BLOCK_NUMBER)nRandomBlockNumber);
                CreateObject2.transform.position = NewMng.New_Vector3(-4.0f, 4.64f, -2.0f);
                ThisBlock2.m_nPlayer = 2;
                m_Deck_P2.m_eDeckList.Add(ThisBlock2);
                break;
        }
    }
    void StartDraw()
    {
        string strTemp = "";
        int nRandomBlockNumber = 0;

        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
        {
            m_fDrawTimer += Time.deltaTime;
            if (m_fDrawTimer >= m_fDrawTime)
            {
                m_fDrawTimer = 0.0f;
                if (m_nP1_DrawNumber < g_nP1_DeckMax)
                {
                    Debug.Log("P1 : " + m_nP1_DrawNumber + "," + g_nP1_DeckMax);
                    Draw(1);

                    m_nP1_DrawNumber++;
                }
                if (m_nP2_DrawNumber < g_nP2_DeckMax)
                {
                    Debug.Log("P2 : " + m_nP2_DrawNumber + "," + g_nP2_DeckMax);
                    Draw(2);
                    
                    m_nP2_DrawNumber++;
                }
            }
        }
        else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
        {
            m_fDrawTimer += Time.deltaTime;
            if (m_fDrawTimer >= m_fDrawTime)
            {
                m_fDrawTimer = 0.0f;
                if (m_nP1_DrawNumber < g_nP1_DeckMax)
                {
                    if (g_nMyPlayer == 1)
                    {
                        Debug.Log("P1 : " + m_nP1_DrawNumber + "," + g_nP1_DeckMax);
                        //Draw(1);
                        nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);


                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P1_DRAW_FROM_CLIENT).ToString(), nRandomBlockNumber);
                        BattleMenuScene.SendMessageToServer(strTemp);
                    }
                    m_nP1_DrawNumber++;
                }

                if (m_nP2_DrawNumber < g_nP2_DeckMax)
                {
                    if (g_nMyPlayer == 1)
                    {
                        Debug.Log("P2 : " + m_nP2_DrawNumber + "," + g_nP2_DeckMax);
                        //Draw(2);
                        nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);

                        strTemp = string.Format("{0}{1}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_P2_DRAW_FROM_CLIENT).ToString(), nRandomBlockNumber);
                        BattleMenuScene.SendMessageToServer(strTemp);
                    }
                    m_nP2_DrawNumber++;
                }
            }
        }

    }

    void PopBlock(stVec2i stBlockIndex) //!< 해당 블록을 찾아서 터트림
    {
	    if(stBlockIndex.m_nX < 0)
		    return;
	    if(stBlockIndex.m_nX >= UserDefine.D_MAP_WIDTH)
		    return;
	    if(stBlockIndex.m_nY < 2)
		    return;
        if (stBlockIndex.m_nY >= UserDefine.D_MAP_HEIGHT)
		    return;

	    for(int i = 0; i < m_goBlockList.Count; i++)
	    {
            Block pBlock = m_goBlockList[i].GetComponent<Block>();
            if( pBlock.m_stIndex.m_nX == stBlockIndex.m_nX && pBlock.m_stIndex.m_nY == stBlockIndex.m_nY )
		    {
                m_nMap[pBlock.m_stIndex.m_nX, pBlock.m_stIndex.m_nY] = 0;


                g_EffectMng.Insert(0, NewMng.New_Vector3(m_goBlockList[i].transform.position.x, m_goBlockList[i].transform.position.y, -1.0f));



                //if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_FIRE)
                //    m_pPoint_Red.Spot();
                //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER)
                //    m_pPoint_Blue.Spot();
                //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_METAL)
                //    m_pPoint_Yellow.Spot();
                //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_WOOD)
                //    m_pPoint_Green.Spot();

                Destroy(m_goBlockList[i]);
                m_goBlockList.Remove(m_goBlockList[i]);


			    //m_goBlockList[i].Alphaing();

			    // g_BlockList[nBlock].Die();
			    // g_Map.m_nBlockMap[stBlockIndex.m_nY][stBlockIndex.m_nX] = 0;

			    // g_EffectManager.Insert(1,g_BlockList[nBlock].m_stPos.x,g_BlockList[nBlock].m_stPos.y);
			
			    // printf("(%d,%d) 블록 터트림\n",stBlockIndex.m_nX,stBlockIndex.m_nY);
			    break;
		    }
	    }
    }

    void PopBlock(GameObject Object) //!< 해당 블록을 찾아서 터트림
    {
        Block pBlock = Object.GetComponent<Block>();
        m_nMap[pBlock.m_stIndex.m_nX, pBlock.m_stIndex.m_nY] = 0;

        g_EffectMng.Insert((int)pBlock.m_eBlockNumber, NewMng.New_Vector3(Object.transform.position.x, Object.transform.position.y, -2.1f));

        //if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_FIRE)
        //    m_pPoint_Red.Spot();
        //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER)
        //    m_pPoint_Blue.Spot();
        //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_METAL)
        //    m_pPoint_Yellow.Spot();
        //else if (pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_WOOD)
        //    m_pPoint_Green.Spot();

        Destroy(Object);
        m_goBlockList.Remove(Object);
    }

    void EraseBlock(GameObject Object) //!< 해당 블록을 찾아서 터트림
    {
        Block pBlock = Object.GetComponent<Block>();
        m_nMap[pBlock.m_stIndex.m_nX, pBlock.m_stIndex.m_nY] = 0;

        Destroy(Object);
        m_goBlockList.Remove(Object);
    }

    void Start_CreateBlock1()
    {
        if (m_bStartCreateBlockEnded == true)
            return;

        m_fCreateTimer += Time.deltaTime;

        if (m_fCreateTimer >= m_fCreateTime)
        {
            m_fCreateTimer -= m_fCreateTime;
            CreateBlock_Start(m_nCreateX, m_nCreateY);

            m_nCreateX++;
            if (m_nCreateX >= UserDefine.D_MAP_WIDTH)
            {
                m_nCreateX = 0;
                m_nCreateY++;
            }
            if (m_nCreateY >= UserDefine.D_MAP_HEIGHT)
            {
                m_nCreateX = 0;
                m_bStartCreateBlockEnded = true;
                
            }
            
        }
    }

    void Start_CreateBlock2()
    {
        if (m_bStartCreateBlockEnded == true)
            return;

        m_fCreateTimer += Time.deltaTime;

        if (m_fCreateTimer >= m_fCreateTime)
        {
            m_fCreateTimer -= m_fCreateTime;
            CreateBlock_Start2(m_nCreateX, m_nCreateY);

            m_nCreateX++;
            if (m_nCreateX >= UserDefine.D_MAP_WIDTH)
            {
                m_nCreateX = 0;
                m_nCreateY++;
            }
            if (m_nCreateY >= UserDefine.D_MAP_HEIGHT)
            {
                m_nCreateX = 0;
                m_bStartCreateBlockEnded = true;

            }

        }
    }
    void Fill_Block()
    {

        for (int j = 0; j < UserDefine.D_MAP_HEIGHT; j++)
        {
            for (int i = 0; i < UserDefine.D_MAP_WIDTH; i++)
            {
                if (m_nMap[i, j] == 0)
                {
                    RegenMagic(i, j);// CreateBlock(i, j);
                }
            }
        }

    }

    bool Fill_Block_End()
    {

        for (int i = 0; i < m_goBlockList.Count; i++)
        {
            if (m_goBlockList[i].GetComponent<Block>().m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
            {
                return false;
            }
        }

        return true;
    }
    

    void RegenCheck() //!< 리젠타임다되면 리젠시키는 마법
    {
        for (int j = 0; j < UserDefine.D_MAP_HEIGHT; j++)
        {
            for (int i = 0; i < UserDefine.D_MAP_WIDTH; i++)
            {
                if (m_fMapRegenTimer[i, j] > 0.0f)
                {
                    m_fMapRegenTimer[i, j] -= Time.deltaTime;
                    //Debug.Log("RT : " + m_fMapRegenTimer[i, j].ToString());
                    if(m_fMapRegenTimer[i, j] <= 0.0f)
                    {


                        m_fMapRegenTimer[i, j] = 0.0f;

                        if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
                        {
                            CreateBlock(i, j);
                        }
                        else if (m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
                        {
                            CreateBlock_Server(i, j);
                        }


                    }
                }
            }
        }
    }

    void RegenMagic( int nX, int nY ) //!< 리젠타임부여
    {
        if (m_fMapRegenTimer[nX, nY] != 0.0f)
            return;

        if ( m_nMap[nX, nY] == 0 )
        {
            m_fMapRegenTimer[nX, nY] = 0.24f;
        }
    }

    void CreateBlock(int nX, int nY)
    {
        if ( m_nMap[nX, nY] == 0 )
        {
            m_nMap[nX, nY] = 1;
            int nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
           
            GameObject CreateObject = (GameObject)Instantiate(m_goBlock[nRandomBlockNumber]);
            //CreateObject.transform.parent = GameObject.Find("Panel").transform;
            Block ThisBlock = CreateObject.GetComponent<Block>();
            ThisBlock.CreateBlock(nX, nY,(E_BLOCK_NUMBER)nRandomBlockNumber);
            m_goBlockList.Add(CreateObject);

        }
    }

    void CreateBlock_Server(int nX, int nY)
    {

        if (BattleMenuScene.g_P1_OrderToCreateBlockList.Count > 0)
        {
            //Draw(1, BattleMenuScene.g_P1_OrderToCreateBlockList[0].m_nBlockNumber);

            for (int i = 0; i < BattleMenuScene.g_P1_OrderToCreateBlockList.Count; i++)
            {

                if (BattleMenuScene.g_P1_OrderToCreateBlockList[i].m_nX == nX && BattleMenuScene.g_P1_OrderToCreateBlockList[i].m_nY == nY)
                {
                    if (m_nMap[nX, nY] == 0)
                    {
                        m_nMap[nX, nY] = 1;
                        int nRandomBlockNumber = (int)BattleMenuScene.g_P1_OrderToCreateBlockList[i].m_eBlockNumber;

                        GameObject CreateObject = (GameObject)Instantiate(m_goBlock[nRandomBlockNumber]);
                        //CreateObject.transform.parent = GameObject.Find("Panel").transform;
                        Block ThisBlock = CreateObject.GetComponent<Block>();
                        ThisBlock.CreateBlock(nX, nY, (E_BLOCK_NUMBER)nRandomBlockNumber);
                        m_goBlockList.Add(CreateObject);
                        BattleMenuScene.g_P1_OrderToCreateBlockList.Remove(BattleMenuScene.g_P1_OrderToCreateBlockList[i]);
                        break;
                    }
                }


            }
        }

        
        
    }

    void CreateBlock_Start2(int nX, int nY)
    {
        if (m_nMap[nX, nY] == 0)
        {
            m_nMap[nX, nY] = 1;
            int nRandomBlockNumber = 0;
            while (true)
            {

                nRandomBlockNumber = m_nbBlockMap[nX, nY];
               
                break;
            }


            GameObject CreateObject = (GameObject)Instantiate(m_goBlock[nRandomBlockNumber]);
            //CreateObject.transform.parent = GameObject.Find("Panel").transform;
            Block ThisBlock = CreateObject.GetComponent<Block>();
            ThisBlock.CreateBlock(nX, nY, (E_BLOCK_NUMBER)nRandomBlockNumber);
            m_goBlockList.Add(CreateObject);

        }
    }
    void CreateBlock_Start(int nX, int nY)
    {
        if (m_nMap[nX, nY] == 0)
        {
            m_nMap[nX, nY] = 1;
            int nRandomBlockNumber = 0;
            int nSearchBlockIndex = 0;
            while (true)
            {

                nRandomBlockNumber = UnityEngine.Random.Range((int)0, (int)E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX);
                nSearchBlockIndex = SearchBlock(nX + 1, nY + 0);
                if (nSearchBlockIndex != -1)
                {
                    if ((int)m_goBlockList[nSearchBlockIndex].GetComponent<Block>().m_eBlockNumber == nRandomBlockNumber)
                    {
                        continue;
                    }
                }
                nSearchBlockIndex = SearchBlock(nX - 1, nY + 0);
                if (nSearchBlockIndex != -1)
                {
                    if ((int)m_goBlockList[nSearchBlockIndex].GetComponent<Block>().m_eBlockNumber == nRandomBlockNumber)
                    {
                        continue;
                    }
                }
                nSearchBlockIndex = SearchBlock(nX + 0, nY + 1);
                if (nSearchBlockIndex != -1)
                {
                    if ((int)m_goBlockList[nSearchBlockIndex].GetComponent<Block>().m_eBlockNumber == nRandomBlockNumber)
                    {
                        continue;
                    }
                }
                nSearchBlockIndex = SearchBlock(nX + 0, nY - 1);
                if (nSearchBlockIndex != -1)
                {
                    if ((int)m_goBlockList[nSearchBlockIndex].GetComponent<Block>().m_eBlockNumber == nRandomBlockNumber)
                    {
                        continue;
                    }
                }
                break;
            }


            GameObject CreateObject = (GameObject)Instantiate(m_goBlock[nRandomBlockNumber]);
            //CreateObject.transform.parent = GameObject.Find("Panel").transform;
            Block ThisBlock = CreateObject.GetComponent<Block>();
            ThisBlock.CreateBlock(nX, nY, (E_BLOCK_NUMBER)nRandomBlockNumber);
            m_goBlockList.Add(CreateObject);

        }
    }

    void Game_Init()
    {
        g_nScore_P1 = 0;
        g_nScore_P2 = 0;

        g_nScore_P1_Display = 0;
        g_nScore_P2_Display = 0;

        g_nTurn = 1;

        m_fTimer = 0.0f;
        m_nTimeOverWarningCheckStep = 0;

        pTargetTurnNumber.text = (g_nTargetTurn).ToString();

        Map_Init();

        m_Deck_P1.m_eDeckList.Clear();
        m_Deck_P2.m_eDeckList.Clear();
    }

    void Map_Init()
    {
        for (int j = 0; j < UserDefine.D_MAP_HEIGHT; j++)
        {
            for (int i = 0; i < UserDefine.D_MAP_WIDTH; i++)
            {
                m_nMap[i, j] = 0;
            }
        }
    }
    void MapRegenTimer_Init()
    {
        for (int j = 0; j < UserDefine.D_MAP_HEIGHT; j++)
        {
            for (int i = 0; i < UserDefine.D_MAP_WIDTH; i++)
            {
                m_fMapRegenTimer[i, j] = 0.0f;
            }
        }
    }
    public int SearchBlock(int nX, int nY)
    {
        for (int i = 0; i < m_goBlockList.Count; i++)
        {
            Block pBlock = m_goBlockList[i].GetComponent<Block>();
            if (pBlock.m_stIndex.m_nX == nX && pBlock.m_stIndex.m_nY == nY)
            {
                return i;
            }
        }

        return -1;
    }



}
