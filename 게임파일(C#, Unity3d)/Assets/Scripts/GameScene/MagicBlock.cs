using UnityEngine;
using System.Collections;


[System.Serializable]


public class MagicBlock : MonoBehaviour {

    public E_BLOCK_NUMBER m_eBlockNumber = E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER;
    public E_ITEM_NUMBER m_eBlockItem = E_ITEM_NUMBER.E_ITEM_NUMBER_MAX;
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
        //if(m_pPtAni == null)
        //    m_pPtAni = GetComponentInChildren<ParticleAnimator>();

        m_fAlpha = 1.0f;
        m_bAlphaDelete = false;
        
        // transform.localScale = NewMng.New_Vector3(0.5f * UserDefine.D_SIZE, 0.5f * UserDefine.D_SIZE, 1.0f);

        ChangeNumber(m_eBlockNumber);

	}

	
	// Update is called once per frame
	void Update () {

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

    public void CreateBlock(E_BLOCK_NUMBER eBlockNumber)
    {
      
        m_eBlockNumber = eBlockNumber;
    }

    public void ChangeNumber(E_BLOCK_NUMBER eBlockNumber)
    {
        if (eBlockNumber > E_BLOCK_NUMBER.E_BLOCK_NUMBER_MAX)
            return;

        m_eBlockNumber = eBlockNumber;
        
        m_pColor.spriteName = "Block_" + ((int)eBlockNumber + 1).ToString();

    }


    void Gravity()
    {
      
    }

    public void AlphaDestroy()
    {
        m_bAlphaDelete = true;
        this.collider.isTrigger = true;
        GetComponentInChildren<ParticleAnimator>().active = false;
    }
}
