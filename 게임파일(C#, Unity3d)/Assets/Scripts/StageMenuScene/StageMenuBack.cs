using UnityEngine;
using System.Collections;

public class StageMenuBack : MonoBehaviour {

    public StageMenuScene g_StageMenu = null;
	// Use this for initialization
	void Start () {
        g_StageMenu = GameObject.Find("StageMenuScene").GetComponent<StageMenuScene>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        //bClickCheck = true;
        if (g_StageMenu.m_nStep == 0)
        {

            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            Application.LoadLevel("MenuScene");

        }
        else if (g_StageMenu.m_nStep == 1)
        {
            g_StageMenu.Close_ItemStore();
        }


    }
}
