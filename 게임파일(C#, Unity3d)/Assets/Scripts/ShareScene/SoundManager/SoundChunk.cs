using UnityEngine;
using System.Collections;

public class SoundChunk : MonoBehaviour {
	
	// Use this for initialization
	private AudioSource Source;

	void Start () {
		Source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Source.isPlaying == false)
		{
			Destroy(this.gameObject);
		}
	}
}
