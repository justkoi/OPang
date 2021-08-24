using UnityEngine;
using System.Collections;

public class toCredit : MonoBehaviour {
    bool bClickCheck;
    Vector3 stVec3;
    GameObject m_Title = null;
	// Use this for initialization
	void Start () 
    {
        m_Title = GameObject.Find("Title");
        bClickCheck = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (bClickCheck == true)
        //{
        //    stVec3 = m_Title.transform.localPosition;
        //}
        //if (stVec3.y >= 240.0f)
        //{
        //    System.GC.Collect();
        //    Resources.UnloadUnusedAssets();
        //    GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_MAN;
        //    Application.LoadLevel("GameScene");
        //}
	}
    void OnClick()
    {
       // bClickCheck = true;
        DontDestroyOnLoad(GameObject.Find("SoundManager").GetComponent<SoundManager>().Play(11));
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_MAN;
        Application.LoadLevel("BattleMenuScene");


    }
}
