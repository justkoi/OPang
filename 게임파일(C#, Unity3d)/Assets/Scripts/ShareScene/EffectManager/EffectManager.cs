using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {
    public GameObject[] m_EffectList = new GameObject[100];
	// Use this for initialization
	void Start () {
        m_EffectList[0] = (GameObject)Resources.Load("Effect_0");
        m_EffectList[1] = (GameObject)Resources.Load("Effect_1");
        m_EffectList[2] = (GameObject)Resources.Load("Effect_2");
        m_EffectList[3] = (GameObject)Resources.Load("Effect_3");
        m_EffectList[4] = (GameObject)Resources.Load("Effect_4");
        m_EffectList[6] = (GameObject)Resources.Load("Effect_6");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Insert(int nIndex, Vector3 stPos)
    {
        Instantiate(m_EffectList[nIndex], stPos, NewMng.New_Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
    }
}
