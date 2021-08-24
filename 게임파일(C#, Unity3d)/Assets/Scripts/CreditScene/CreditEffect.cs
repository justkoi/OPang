using UnityEngine;
using System.Collections;

public class CreditEffect : MonoBehaviour {

    public GameObject g_Maker = null;
    public GameObject g_Maker2 = null;
    public GameObject TeamLogo = null;
    ParticleEmitter m_Particle = null;
    ParticleEmitter m_Particle2 = null;
    UISprite m_Maker = null;
    UISprite m_Maker2 = null;
    UISprite m_TeamLogo = null;
    float fTime;
    bool m_bCreditCheck;
	void Start () 
    {
        g_Maker = GameObject.Find("Programmer");
        g_Maker2 = GameObject.Find("Graphic");
        TeamLogo = GameObject.Find("TeamLogo");
        m_Maker = g_Maker.GetComponent<UISprite>();
        m_Maker2 = g_Maker2.GetComponent<UISprite>();
        m_TeamLogo = TeamLogo.GetComponent<UISprite>();
        m_Particle = g_Maker.GetComponentInChildren<ParticleEmitter>();
        m_Particle2 = g_Maker2.GetComponentInChildren<ParticleEmitter>();
        fTime = 0.0f;
        m_bCreditCheck = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        fTime += Time.deltaTime;
        if (m_bCreditCheck == false)
        {
            if (fTime >= 1.3f)
            {
                m_Particle.emit = true;
                m_Maker.alpha = 0.0f;
            }
            if (fTime > 2.2f)
                m_Particle.emit = false;
            if (fTime >= 3.2f)
            {
                m_Particle2.emit = true;
                m_Maker2.alpha = 0.0f;
            }
            if (fTime > 3.6f)
            {
                m_Particle2.emit = false;
                m_bCreditCheck = true;
            }
        }
        if (m_bCreditCheck == true)
            m_TeamLogo.alpha += 0.5f * Time.deltaTime;
        if (fTime > 6.0f)
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            Application.LoadLevel("MenuScene");
        }
	}
}
