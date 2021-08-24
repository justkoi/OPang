using UnityEngine;
using System.Collections;

public class ChangeMode : MonoBehaviour {

    public GameObject m_pButton_Battle = null;
    public GameObject m_pButton_Scenario = null;

    public GameObject m_pButton_GameStart = null;
    public GameObject m_pButton_Credit = null;
    public GameObject m_pButton_Option = null;
    public GameObject m_pButton_Exit = null;

    public GameObject m_pButton_Back = null;
	// Use this for initialization
	void Start () {

        m_pButton_Battle = GameObject.Find("Button_Battle");
        m_pButton_Scenario = GameObject.Find("Button_Scenario");

        m_pButton_GameStart = GameObject.Find("Button_GameStart");
        m_pButton_Credit = GameObject.Find("Button_Credit");
        m_pButton_Option = GameObject.Find("Button_Option");
        m_pButton_Exit = GameObject.Find("Button_Exit");


        m_pButton_Back = GameObject.Find("Button_Back");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        m_pButton_GameStart.GetComponent<MenuAppear>().Hide(1.0f, 0.0f);
        m_pButton_Credit.GetComponent<MenuAppear>().Hide(1.0f, 0.4f);
        m_pButton_Option.GetComponent<MenuAppear>().Hide(1.0f, 0.8f);
        m_pButton_Exit.GetComponent<MenuAppear>().Hide(1.0f, 1.2f);

        m_pButton_Battle.active = true;
        m_pButton_Scenario.active = true;

        m_pButton_Battle.GetComponent<MenuAppear>().Show(1.0f, 1.6f);
        m_pButton_Scenario.GetComponent<MenuAppear>().Show(1.0f, 2.0f);


        m_pButton_Back.active = true;

        m_pButton_Back.GetComponent<MenuAppear>().Show(1.0f, 2.0f);

    }
}
