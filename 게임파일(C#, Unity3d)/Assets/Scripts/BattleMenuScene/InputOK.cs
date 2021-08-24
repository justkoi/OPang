using UnityEngine;
using System.Collections;

public class InputOK : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnSubmit()
    {
        BattleMenuScene.Send();
    }
}
