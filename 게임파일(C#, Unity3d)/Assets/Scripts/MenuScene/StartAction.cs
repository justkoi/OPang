using UnityEngine;
using System.Collections;

public class StartAction : MonoBehaviour {

  //  public UISlicedSprite m_pSprite = null;
   // public UILabel m_pLabel = null;



    public float m_fStartScale = 0.6f;
    public float m_fTargetScale = 1.3f;
    public float m_fJumpPower = 0.6f;
    public float m_fJumpPlusPower = 1.3f;

    public int m_nLevel = 0;

    public Vector3 m_vFirstScale;

    public float m_fNowScale = 0.0f;
    public bool m_bTargeted = false;
    public bool m_bEnded = false;
    public bool m_bBounce = false;
    public bool m_bHover = true;

	// Use this for initialization
	void Start () {


        m_nLevel = 0;

        m_fNowScale = m_fStartScale;

        //m_pSprite = GetComponentInChildren<UISlicedSprite>();
        //m_pLabel = GetComponentInChildren<UILabel>();

        m_vFirstScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

        this.transform.localScale = NewMng.New_Vector3(m_vFirstScale.x * m_fStartScale, m_vFirstScale.y * m_fStartScale, m_vFirstScale.z * m_fStartScale);

	}
    void OnHover()
    {
        if (m_bHover == true)
        {
            m_bEnded = true;
            m_fNowScale = m_fStartScale;
            this.transform.localScale = NewMng.New_Vector3(m_vFirstScale.x * m_fNowScale, m_vFirstScale.y * m_fNowScale, m_vFirstScale.z * m_fNowScale);
        }

    }
    //void OnDisable()
    //{
    //    m_bEnded = false;
    //}
	// Update is called once per frame
	void Update () {

        if (m_bEnded == false)
        {
            if (m_bTargeted == false)
            {
                m_fNowScale += m_fJumpPower * Time.deltaTime;
                m_fJumpPower += m_fJumpPlusPower * Time.deltaTime;
                if (m_fNowScale >= m_fTargetScale)
                {
                    m_fNowScale = m_fTargetScale;
                    m_fJumpPower = 0.0f;
                    m_bTargeted = true;
                }
                if (m_bBounce == false)
                {
                    if (m_fNowScale <= m_fStartScale)
                    {
                        m_fNowScale = m_fStartScale;
                        m_bTargeted = false;
                    }
                }
            }
            else
            {
                m_fNowScale += m_fJumpPower * Time.deltaTime;
                m_fJumpPower -= m_fJumpPlusPower * Time.deltaTime;

                if (m_fNowScale >= m_fTargetScale)
                {
                    m_fNowScale = m_fTargetScale;
                }

                if (m_fNowScale <= m_fStartScale)
                {
                    m_fNowScale = m_fStartScale;
                    m_bTargeted = false;
                }
            }

            this.transform.localScale = NewMng.New_Vector3(m_vFirstScale.x * m_fNowScale, m_vFirstScale.y * m_fNowScale, m_vFirstScale.z * m_fNowScale);
        }

	}
}
