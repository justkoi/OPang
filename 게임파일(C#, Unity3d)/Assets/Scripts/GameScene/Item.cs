using UnityEngine;
using System.Collections;


[System.Serializable]


public class Item : MonoBehaviour
{

    public E_ITEM_NUMBER m_eBlockNumber = E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW;

    public UISprite m_pColor = null;
    //public ParticleAnimator m_pPtAni = null;

    public int m_nPlayer = 0;
    public float m_fAlpha = 1.0f;

    public bool m_bAlphaDelete = false;


    // Use this for initialization
    void Start()
    {

        if (m_pColor == null)
            m_pColor = transform.FindChild("UISprite").gameObject.GetComponentInChildren<UISprite>();
        

        m_fAlpha = 1.0f;
        m_bAlphaDelete = false;

        // transform.localScale = NewMng.New_Vector3(0.5f * UserDefine.D_SIZE, 0.5f * UserDefine.D_SIZE, 1.0f);

    }


    // Update is called once per frame
    void Update()
    {

        if (m_bAlphaDelete == true)
        {
            m_fAlpha -= Time.deltaTime;
            m_pColor.alpha = m_fAlpha;
            if (m_fAlpha <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }

    }


    public void AlphaDestroy()
    {
        m_bAlphaDelete = true;
        this.collider.isTrigger = true;
        GetComponentInChildren<ParticleAnimator>().active = false;
    }
}
