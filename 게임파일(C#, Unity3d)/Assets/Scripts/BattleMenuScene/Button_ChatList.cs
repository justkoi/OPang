using UnityEngine;
using System.Collections;
using System.Text;

public class Button_ChatList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {

        BattleMenuScene.g_ChatWindow.ChangeMode(E_CHATMODE.E_CHATMODE_CHATLIST);
        BattleMenuScene.UpdateChat();

    }
}
