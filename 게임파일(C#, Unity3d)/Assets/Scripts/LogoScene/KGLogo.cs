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
            System.GC.Collect();                        //�� �Ѿ���հų��Ҷ� ��ȣ��!
            Resources.UnloadUnusedAssets();         
            Application.LoadLevel("TeamLogoScene");
        }
	}
}
