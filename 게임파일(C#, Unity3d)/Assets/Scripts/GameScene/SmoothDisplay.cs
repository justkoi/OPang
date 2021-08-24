using UnityEngine;
using System.Collections;

public class SmoothDisplay : MonoBehaviour {


    public float m_fTimer = 0.0f;
    public float m_fTime = 1.0f;
    public UILabel m_pSpirte = null;

	// Use this for initialization
	void Start () {
        m_fTimer = m_fTime;
        m_pSpirte = GetComponentInChildren<UILabel>();
        m_pSpirte.alpha = (m_fTime - m_fTimer) / m_fTime;
    }

	
	// Update is called once per frame
	void Update () {

        if (m_fTimer < m_fTime)
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > m_fTime)
            {
                m_fTimer = m_fTime;
            }
            m_pSpirte.alpha = (m_fTime - m_fTimer) / m_fTime;
        }


	}

    public void Play(float fTime)
    {
        m_fTime = fTime;
        m_fTimer = 0.0f;
    }
}
