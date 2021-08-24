using UnityEngine;
using System.Collections;

public class toGame : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
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
        //    //System.GC.Collect();
        //    //Resources.UnloadUnusedAssets();
        //    //GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_AI;
        //    //Application.LoadLevel("GameScene");
        //}
	
	}

    void OnClick()
    {
        //bClickCheck = true;
        DontDestroyOnLoad(GameObject.Find("SoundManager").GetComponent<SoundManager>().Play(11));
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        GameScene.m_eGameMode = E_GAME_MODE.E_GAME_MODE_VS_AI;
        Application.LoadLevel("GameScene");
    }
}
