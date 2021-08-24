using UnityEngine;
using System.Collections;

public class GoToGame : MonoBehaviour {

    public int nStage;
    public StageMenuScene g_StageMenu = null;

    public int m_nP1_DeckMax = 2;
    public int m_nP2_DeckMax = 2;

    // Use this for initialization
    void Start()
    {
        g_StageMenu = GameObject.Find("StageMenuScene").GetComponent<StageMenuScene>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        //bClickCheck = true;

        GameScene.g_nP1_DeckMax = m_nP1_DeckMax;
        GameScene.g_nP2_DeckMax = m_nP2_DeckMax;

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_AI;

        g_StageMenu.Open_ItemStore();

        GameScene.g_nStage = nStage;
    }
}
