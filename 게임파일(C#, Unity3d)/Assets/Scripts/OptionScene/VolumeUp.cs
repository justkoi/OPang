using UnityEngine;
using System.Collections;

public class VolumeUp : MonoBehaviour {
	
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
    void OnClick()
    {
        if(OptionScene.g_fBGMVolume < 1.0f)
            OptionScene.g_fBGMVolume += 0.2f;
    }
}
