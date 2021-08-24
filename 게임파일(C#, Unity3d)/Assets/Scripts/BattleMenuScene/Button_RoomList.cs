using UnityEngine;
using System.Collections;
using System.Text;

public class Button_RoomList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnClick()
    {

        string strTemp;

        BattleMenuScene.g_ChatWindow.ChangeMode(E_CHATMODE.E_CHATMODE_ROOMLIST);
        BattleMenuScene.g_ChatWindow.Clear(E_CHATMODE.E_CHATMODE_ROOMLIST);


        strTemp = string.Format("{0}", ((int)E_NETWORK_COMMAND.E_NETWORK_COMMAND_ROOM).ToString());
        BattleMenuScene.sock.Send(Encoding.Unicode.GetBytes(strTemp));
    }
}
