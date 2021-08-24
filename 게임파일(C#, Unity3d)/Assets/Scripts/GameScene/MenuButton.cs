using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

    //blic GameScene g_GameScene = null;

	// Use this for initialization
	void Start () {
        //GameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnClick()
    {

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        if(GameScene.m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_AI)
            Application.LoadLevel("StageMenuScene");
        else if (GameScene.m_eGameMode == E_GAME_MODE.E_GAME_MODE_VS_MAN)
            Application.LoadLevel("BattleMenuScene");
    }
}
