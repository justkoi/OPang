using UnityEngine;
using System.Collections;

public class toOption : MonoBehaviour {
    bool bClickCheck;
    public Vector3 stVec3;
    public GameObject pTitle= null; 
	// Use this for initialization
	void Start () 
    {
        pTitle = GameObject.Find("Title");
        bClickCheck = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (bClickCheck == true)
        {
            stVec3 = pTitle.transform.localPosition;
        }
        if (stVec3.y >= 240.0f)
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            Application.LoadLevel("OptionScene");
        }
	}
    void OnClick()
    {
        bClickCheck = true;
    }
}
