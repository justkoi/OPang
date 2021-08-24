using UnityEngine;
using System.Collections;

public class Button_Send : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnClick()
    {
        BattleMenuScene.Send();
    }
}
