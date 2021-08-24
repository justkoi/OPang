using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour {


    static public SoundManager g_SoundMng = null;

    public GameObject m_pButton_Battle = null;
    public GameObject m_pButton_Scenario = null;

   // public GameObject m_pButton_GameStart = null;
   // public GameObject m_pButton_Credit = null;
    public GameObject m_pButton_Option = null;
    //public GameObject m_pButton_Exit = null;

   // public GameObject m_pButton_Back = null;


	// Use this for initialization
	void Start () {

        g_SoundMng = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        g_SoundMng.Play(9, true, 0.6f);

        m_pButton_Battle = GameObject.Find("Button_Battle");
        m_pButton_Scenario = GameObject.Find("Button_Scenario");

        //m_pButton_GameStart = GameObject.Find("Button_GameStart");
        //m_pButton_Credit = GameObject.Find("Button_Credit");
        m_pButton_Option = GameObject.Find("Button_Option");
       // m_pButton_Exit = GameObject.Find("Button_Exit");

        //m_pButton_Back = GameObject.Find("Button_Back");


       // m_pButton_Battle.active = false;
       // m_pButton_Scenario.active = false;

       // m_pButton_Back.active = false;


    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnDestroy()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

}
