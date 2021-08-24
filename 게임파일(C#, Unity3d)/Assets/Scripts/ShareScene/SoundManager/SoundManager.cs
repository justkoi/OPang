using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class SoundManager : MonoBehaviour {
	
	public GameObject[] m_SoundList = new GameObject[100];
	public GameObject m_go;
	public static int m_nPlayIndex = -1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
    public GameObject Play(int nIndex)
    {
        if (m_SoundList[nIndex] != null)
            return (GameObject)Instantiate(m_SoundList[nIndex]);

        return null;
    }

    public GameObject Play(int nIndex, bool bLoop)
	{
        if (m_SoundList[nIndex] != null)
        {
            GameObject Sound = (GameObject)Instantiate(m_SoundList[nIndex]);
            Sound.GetComponent<AudioSource>().loop = bLoop;
            return Sound;
        }
        return null;
	}

    public GameObject Play(int nIndex, bool bLoop, float fVolume)
    {
        if (m_SoundList[nIndex] != null)
        {
            //m_SoundList[nIndex].GetComponent<AudioSource>().loop = bLoop;
            GameObject Sound = (GameObject)Instantiate(m_SoundList[nIndex]);
            Sound.GetComponent<AudioSource>().loop = bLoop;

            if (fVolume > 1.0f)
                fVolume = 1.0f;
            if (fVolume < 0.0f)
                fVolume = 0.0f;

            Sound.GetComponent<AudioSource>().volume = fVolume;
            return Sound;
        }
        return null;
    }
	
}
