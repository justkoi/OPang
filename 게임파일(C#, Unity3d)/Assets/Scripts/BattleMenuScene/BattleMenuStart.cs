using UnityEngine;
using System.Collections;

public class BattleMenuStart : MonoBehaviour
{

    public BattleMenuScene g_BattleMenu = null;
    // Use this for initialization
    void Start()
    {
        g_BattleMenu = GameObject.Find("BattleMenuScene").GetComponent<BattleMenuScene>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        //bClickCheck = true;
        if (g_BattleMenu.m_nStep == 0)
        {

            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            Application.LoadLevel("MenuScene");

        }
        else if (g_BattleMenu.m_nStep == 1)
        {
            g_BattleMenu.Close_ItemStore();
        }


    }
}
