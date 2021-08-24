using UnityEngine;
using System.Collections;

public class BacktoMenu : MonoBehaviour {
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            Application.LoadLevel("MenuScene");
        }
	}
    void OnClick()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        Application.LoadLevel("MenuScene");
    }
}
