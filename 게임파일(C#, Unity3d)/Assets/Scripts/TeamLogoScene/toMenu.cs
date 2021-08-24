using UnityEngine;
using System.Collections;

public class toMenu : MonoBehaviour {
    float fTime;
    bool bAlphaCheck;
    public UISprite TeamSprite = null;
    GameObject m_TeamLogo = null;

	// Use this for initialization
	void Start ()  
    {
        fTime = 0.0f;
        m_TeamLogo = GameObject.Find("TeamLogoo");
        TeamSprite = m_TeamLogo.GetComponent<UISprite>();
        TeamSprite.alpha = 0.0f;
        bAlphaCheck = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        fTime += Time.deltaTime;
        if (fTime >= 1.5f && bAlphaCheck == false)
        {
            TeamSprite.alpha = 1.4f;
            bAlphaCheck = true;
        }
        if (bAlphaCheck == true)
            TeamSprite.alpha -= 0.7f * Time.deltaTime;

        if (TeamSprite.alpha <= 0.0f && bAlphaCheck == true)
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            Application.LoadLevel("MenuScene");
        }
        //if (fTime >= 2.5f)
        //    Application.LoadLevel("MenuScene");
	}
}
