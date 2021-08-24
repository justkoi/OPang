using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour {

	// Use this for initialization
    tk2dAnimatedSprite AnimationSprite;

	void Start () {

        AnimationSprite = GetComponent<tk2dAnimatedSprite>();


	}
	
	// Update is called once per frame
	void Update () {

        if (AnimationSprite.isPlaying() == false)
            Destroy(this.gameObject);

	}
}
