using UnityEngine;
using System.Collections;

public class KGLogo : MonoBehaviour {

    float fTime;
	// Use this for initialization
	void Start () 
    {
        fTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        fTime += Time.deltaTime;
        if (fTime > 1.4f)
        {
            System.GC.Collect();                        //신 넘어갈일잇거나할때 꼭호출!
            Resources.UnloadUnusedAssets();         
            Application.LoadLevel("TeamLogoScene");
        }
	}
}
