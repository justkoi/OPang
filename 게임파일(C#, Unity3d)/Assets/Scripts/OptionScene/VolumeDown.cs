using UnityEngine;
using System.Collections;

public class VolumeDown : MonoBehaviour {
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnClick()
    {
        if (OptionScene.g_fBGMVolume > 0.0f)
            OptionScene.g_fBGMVolume -= 0.2f;
    }
}
