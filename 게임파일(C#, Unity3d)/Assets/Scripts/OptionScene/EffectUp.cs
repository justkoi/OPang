using UnityEngine;
using System.Collections;

public class EffectUp : MonoBehaviour{

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
        if (OptionScene.g_fEffectVolume < 1.0f)
            OptionScene.g_fEffectVolume += 0.2f;
    }
}
