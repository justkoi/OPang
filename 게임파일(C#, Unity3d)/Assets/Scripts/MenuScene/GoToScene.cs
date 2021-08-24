using UnityEngine;
using System.Collections;

public class GoToScene : MonoBehaviour
{

    public string strScene;



	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {

        //bClickCheck = true;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        Application.LoadLevel(strScene);

        GameScene.g_nGold -= StageMenuScene.g_nMinusGold;
        StageMenuScene.g_nMinusGold = 0;
    }
}
