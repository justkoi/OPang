using UnityEngine;
using System.Collections;
using System.Text;

public class Button_UserList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        string strTemp;

        BattleMenuScene.g_ChatWindow.ChangeMode(E_CHATMODE.E_CHATMODE_USERLIST);
        BattleMenuScene.g_ChatWindow.Clear(E_CHATMODE.E_CHATMODE_USERLIST);


        strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_LIST).ToString());
        BattleMenuScene.sock.Send(Encoding.Unicode.GetBytes(strTemp));

    }
}
