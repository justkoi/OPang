using UnityEngine;
using System.Collections;

public class Buy_Item1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (GameScene.g_nGold - StageMenuScene.g_nMinusGold >= 100)
        {
            bool m_bResult = GameScene.g_P1_Inventory.Input_Item((int)E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW, 1);
            if (m_bResult == true)
            {
                StageMenuScene.g_nMinusGold += 100;


                GameObject.Find("GoldMinusNumber").GetComponentInChildren<UILabel>().text = "( -" + StageMenuScene.g_nMinusGold.ToString() + ")";

            }
        }
    }
}
