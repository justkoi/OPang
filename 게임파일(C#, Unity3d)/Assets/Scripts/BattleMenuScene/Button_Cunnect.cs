using UnityEngine;
using System.Collections;

public class Button_Cunnect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if(BattleMenuScene.g_bCunnected == false)
            BattleMenuScene.Cunnect();
    }
}
