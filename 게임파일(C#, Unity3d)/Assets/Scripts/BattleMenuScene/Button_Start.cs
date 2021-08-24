using UnityEngine;
using System.Collections;

public class Button_Start : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnClick()
    {
        if (BattleMenuScene.m_eState == E_READY_STATE.E_READY_STATE_NONE)
            BattleMenuScene.AutoMatch();
    }
}
