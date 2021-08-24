using UnityEngine;
using System.Collections;

public class EffectDown : MonoBehaviour {
    public float m_Volume;
	// Use this for initialization
	void Start () 
    {
        m_Volume = OptionScene.g_fEffectVolume;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnClick()
    {
        if (OptionScene.g_fEffectVolume > 0.0f)
            OptionScene.g_fEffectVolume -= 0.2f;
    }
}
