using UnityEngine;
using System.Collections;

public enum E_APPEAR_STATE
{
    E_APPEAR_STATE_NONE,
    E_APPEAR_STATE_SHOW,
    E_APPEAR_STATE_HIDE,
};

public enum E_APPEAR_ORDER
{
    E_APPEAR_ORDER_NONE,
    E_APPEAR_ORDER_SHOW,
    E_APPEAR_ORDER_HIDE,
};


public class MenuAppear : MonoBehaviour {

    public UISlicedSprite m_pSprite = null;
    public UILabel m_pLabel = null;

    public float m_fTimer = 0.0f;
    public float m_fTime = 1.0f;

    public float m_fAppearTimer = 0.0f;
    public float m_fAppearTime = 1.0f;

    public bool m_bAppeared = false;

    public bool m_bPlaying = false;

    public float m_fSpeed = 10.0f;

    public float m_fSaveX = 0.0f;

    public E_APPEAR_STATE m_eState = E_APPEAR_STATE.E_APPEAR_STATE_NONE;
    //public E_APPEAR_ORDER m_eOrderState = E_APPEAR_ORDER.E_APPEAR_ORDER_NONE;


	// Use this for initialization
	void Start () {

        //m_eOrderState = E_APPEAR_ORDER.E_APPEAR_ORDER_SHOW;
        m_eState = E_APPEAR_STATE.E_APPEAR_STATE_SHOW;

        m_fSpeed = 0.1f;

        m_fTimer = 0.0f;
        m_fAppearTimer = 0.0f;
        m_bAppeared = false;
        m_bPlaying = false;

        m_pSprite = GetComponentInChildren<UISlicedSprite>();
        m_pLabel = GetComponentInChildren<UILabel>();

        //collider.isTrigger = false;
        //if (GetComponent<UIButton>() != null)
        //{
            
        //}
        GetComponent<BoxCollider>().center = NewMng.New_Vector3(99999, GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z);
        m_pSprite.alpha = 0;
        m_pLabel.alpha = 0;

	}
	
	// Update is called once per frame
	void Update () {

        m_fAppearTimer += Time.deltaTime;
        if (m_bAppeared == false && m_fAppearTimer >= m_fAppearTime)
        {
            m_bAppeared = true;
            m_bPlaying = true;
            //collider.enabled = false;
            //if (GetComponent<UIButton>() != null)
            //{
            //    GetComponent<UIButton>().enabled = false;
            //    GetComponent<UIButtonOffset>().enabled = false;
            //    GetComponent<UIButtonScale>().enabled = false;

            //}
            GetComponent<BoxCollider>().center = NewMng.New_Vector3(99999, GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z);
        
        }

        if (m_bPlaying == true)
        {
            if (m_eState == E_APPEAR_STATE.E_APPEAR_STATE_HIDE)
            {
                m_fTimer += Time.deltaTime;
                m_pSprite.alpha = m_fTime - m_fTimer / m_fTime;
                m_pLabel.alpha = m_fTime - m_fTimer / m_fTime;

                if (m_fTimer >= m_fTime)
                {
                    m_fTimer = m_fTime;
                    m_bPlaying = false;
                }


                transform.position = NewMng.New_Vector3(transform.position.x, transform.position.y - (m_fSpeed * Time.deltaTime), transform.position.z);

            }
            else if (m_eState == E_APPEAR_STATE.E_APPEAR_STATE_SHOW)
            {
                m_fTimer += Time.deltaTime;
                m_pSprite.alpha = 1.0f - (m_fTime - m_fTimer / m_fTime);
                m_pLabel.alpha = 1.0f - (m_fTime - m_fTimer / m_fTime);

                if (m_fTimer >= m_fTime)
                {
                    m_fTimer = m_fTime;
                    m_bPlaying = false;
                    ////collider.enabled = true;
                    //if (GetComponent<UIButton>() != null)
                    //    GetComponent<UIButton>().enabled = true;


                    GetComponent<BoxCollider>().center = NewMng.New_Vector3(m_fSaveX, GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z);
                }

                transform.position = NewMng.New_Vector3(transform.position.x, transform.position.y + (m_fSpeed * Time.deltaTime), transform.position.z);

            }
        }


	}

    public void Show(float fTime, float fAppearTime)
    {
        m_fTime = fTime;
        m_fTimer = 0.0f;
        m_fAppearTimer = 0.0f;
        m_fAppearTime = fAppearTime;
        m_bAppeared = false;
        m_eState = E_APPEAR_STATE.E_APPEAR_STATE_SHOW;
    }

    public void Hide(float fTime, float fAppearTime)
    {
        m_fTime = fTime;
        m_fTimer = 0.0f;
        m_fAppearTimer = 0.0f;
        m_fAppearTime = fAppearTime;
        m_bAppeared = false;
        m_eState = E_APPEAR_STATE.E_APPEAR_STATE_HIDE;
    }

    //bool Playing()
    //{


    //    return false;
    //}
}
