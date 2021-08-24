using UnityEngine;
using System.Collections;

public class ButtonSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        DontDestroyOnLoad(GameObject.Find("SoundManager").GetComponent<SoundManager>().Play(11));
    }
}
