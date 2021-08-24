using UnityEngine;
using System.Collections;

public class PointScript : MonoBehaviour {


    public UISprite pSprite = null;
    public float m_fSpotTime = 0.5f;

    public float m_fSpotTimer = 0.0f;

	// Use this for initialization
	void Start () {
        m_fSpotTimer = m_fSpotTime;
        pSprite = GetComponentInChildren<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_fSpotTimer < m_fSpotTime)
        {
            m_fSpotTimer += Time.deltaTime;
            if (m_fSpotTimer >= m_fSpotTime)
            {
                m_fSpotTimer = m_fSpotTime;
            }

        }
        pSprite.alpha = 1.0f - (m_fSpotTimer  / m_fSpotTime);
	}

    public void Spot()
    {
        m_fSpotTimer = 0.0f;
    }
}
