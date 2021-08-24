using UnityEngine;
using System.Collections;


[System.Serializable]

public enum E_BLOCK_NUMBER
{
    E_BLOCK_NUMBER_WATER,
    E_BLOCK_NUMBER_FIRE,
    E_BLOCK_NUMBER_METAL,
    E_BLOCK_NUMBER_WOOD,
    E_BLOCK_NUMBER_EARTH,
    E_BLOCK_NUMBER_MAX,
    E_BLOCK_NUMBER_ITEM_RAINBOW,
    E_BLOCK_NUMBER_ITEM_RESET,
};

public enum E_BLOCK_LIGHT
{
    E_BLOCK_LIGHT_BLACK,
    E_BLOCK_LIGHT_WHITE,
    E_BLOCK_LIGHT_MAX
};

public enum E_BLOCK_STATE
{
    E_BLOCK_STATE_NONE,
    E_BLOCK_STATE_CREATING,
    E_BLOCK_STATE_ACT,
    E_BLOCK_STATE_CHANGING,
};

public enum E_BLOCK_MODE
{
    E_BLOCK_MODE_COLOR,
    E_BLOCK_MODE_RUNE,
    E_BLOCK_MODE_MAX
}

public class Block : MonoBehaviour {


    public float m_fG = 9.8f; //!< 중력상수
    public float m_fT = 0.01f; //!< 시간계수

    public stVec2i m_stIndex;

    public int X;
    public int Y;

    public Vector3 m_vCreatePos;

    public float m_fCreateTimer = 0.0f;
    public float m_fCreateTime = 1.0f;

    public float m_fChangeTimer = 0.0f;
    public float m_fChangeTime = 0.3f;

    public bool m_bChecked = false;
    public bool m_bFocused = false;


    public E_BLOCK_NUMBER m_eBlockNumber = E_BLOCK_NUMBER.E_BLOCK_NUMBER_WATER;
    public E_BLOCK_STATE m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_NONE;
    public E_BLOCK_LIGHT m_eBlockLight = E_BLOCK_LIGHT.E_BLOCK_LIGHT_BLACK;

    //public UISprite m_pLight = null;
    public UISprite m_pColor = null;

    public float m_fBackGroundAlpha = 0.3f;

    public E_ITEM_NUMBER m_eItemNumber = E_ITEM_NUMBER.E_ITEM_NUMBER_MAX;

	// Use this for initialization
    void Start()
    {
        //if (m_pLight == null)
        //    m_pLight = transform.FindChild("UISpriteLight").gameObject.GetComponentInChildren<UISprite>();
        if (m_pColor == null)
            m_pColor = transform.FindChild("UISprite").gameObject.GetComponentInChildren<UISprite>();


        Init_Light();
        
        transform.localScale = NewMng.New_Vector3(0.5f * UserDefine.D_SIZE, 0.5f * UserDefine.D_SIZE, 1.0f);
        //transform.position = m_nCreatePos;

   
        m_fCreateTimer = 0.0f;
        m_fChangeTimer = 0.0f;
        m_fChangeTime = 0.3f;
        m_bChecked = false;
        m_bFocused = false;

        

	}

    void Init_Light()
    {
        m_eBlockLight = (E_BLOCK_LIGHT)Random.Range(0, (int)E_BLOCK_LIGHT.E_BLOCK_LIGHT_MAX);



        //if (m_eBlockLight == E_BLOCK_LIGHT.E_BLOCK_LIGHT_BLACK)
        //    m_pLight.spriteName = "Light_1";
        //else if (m_eBlockLight == E_BLOCK_LIGHT.E_BLOCK_LIGHT_WHITE)
        //    m_pLight.spriteName = "Light_2";

        m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_CHANGING;
        m_fChangeTimer = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        X = m_stIndex.m_nX;
        Y = m_stIndex.m_nY;
        if (m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
        {
            m_fCreateTimer += Time.deltaTime;
            transform.localScale = NewMng.New_Vector3((0.5f + ((m_fCreateTimer / m_fCreateTime) * 0.5f)) * UserDefine.D_SIZE, (0.5f + ((m_fCreateTimer / m_fCreateTime) * 0.5f)) * UserDefine.D_SIZE, 1.0f);
            
            if (m_fCreateTimer >= m_fCreateTime)
            {
                transform.localScale = NewMng.New_Vector3(1.0f * UserDefine.D_SIZE, 1.0f * UserDefine.D_SIZE, 1.0f);
            
                m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_ACT;
                m_fCreateTimer = 0.0f;
            }
        }

        if (m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CHANGING)
        {
            m_fChangeTimer += Time.deltaTime;
            transform.localScale = NewMng.New_Vector3((0.5f + ((m_fChangeTimer / m_fChangeTime) * 0.5f)) * UserDefine.D_SIZE, (0.5f + ((m_fChangeTimer / m_fChangeTime) * 0.5f)) * UserDefine.D_SIZE, 1.0f);

            if (m_fChangeTimer >= m_fChangeTime)
            {
                transform.localScale = NewMng.New_Vector3(1.0f * UserDefine.D_SIZE, 1.0f * UserDefine.D_SIZE, 1.0f);

                m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_ACT;
                m_fChangeTimer = 0.0f;
            }
        }

        
        

    }

    public void CheckReset()
    {
        m_bChecked = false;
        m_bFocused = false;
    }

    public void SetItem(int nColor)
    {
        if (m_eItemNumber == E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW)
        {
            m_eBlockNumber = (E_BLOCK_NUMBER)nColor;
        }
    }

    public void ResetItem()
    {
        if (m_eItemNumber == E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW)
        {
            m_eBlockNumber = E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW;
        }
    }
    public void ItemReset()
    {
        if (m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
        {
            m_bChecked = false;
            m_bFocused = false;
        }
    }
    public void CreateBlock(int nX, int nY, E_BLOCK_NUMBER eBlockNumber)
    {
        m_stIndex.m_nX = nX;
        m_stIndex.m_nY = nY;


        m_vCreatePos = NewMng.New_Vector3((UserDefine.D_START_X + (UserDefine.D_BLOCK_WIDTH * nX)) , (UserDefine.D_START_Y - (UserDefine.D_BLOCK_HEIGHT * nY))  , transform.position.z);
        transform.position = NewMng.New_Vector3((UserDefine.D_START_X + (UserDefine.D_BLOCK_WIDTH * nX)) * UserDefine.D_SIZE, (UserDefine.D_START_Y - (UserDefine.D_BLOCK_HEIGHT * nY)) * UserDefine.D_SIZE, transform.position.z);

        m_eBlockNumber = eBlockNumber;
        m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_CREATING;

        m_fCreateTimer = 0.0f;

    }

    public void ChangeNumber(E_BLOCK_NUMBER eBlockNumber)
    {
        m_eBlockNumber = eBlockNumber;
        
        m_pColor.spriteName = "Block_" + ((int)eBlockNumber + 1).ToString();

        m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_CHANGING;
        m_fChangeTimer = 0.0f;

        if (m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
        {
            m_eItemNumber = E_ITEM_NUMBER.E_ITEM_NUMBER_RAINBOW;
        }
        //GameScene.g_SoundMng.Play(1);
        //Debug.Log("SoundPlay1");
    }

    public void ChangeLight(E_BLOCK_LIGHT eBlockLight)
    {
        m_eBlockLight = eBlockLight;

       // if(eBlockLight == E_BLOCK_LIGHT.E_BLOCK_LIGHT_BLACK)
           // m_pLight.spriteName = "Light_1";
        //else if (eBlockLight == E_BLOCK_LIGHT.E_BLOCK_LIGHT_WHITE)
           // m_pLight.spriteName = "Light_2";

        m_eBlockState = E_BLOCK_STATE.E_BLOCK_STATE_CHANGING;
        m_fChangeTimer = 0.0f;
        GameScene.g_SoundMng.Play(1);
        //Debug.Log("SoundPlay1");
    }

    void Gravity()
    {
      
    }

    public void ChangeMode(E_BLOCK_MODE eBlockMode)
    {

        m_pColor = transform.FindChild("UISprite").gameObject.GetComponentInChildren<UISprite>();
        //m_pLight = transform.FindChild("UISpriteLight").gameObject.GetComponentInChildren<UISprite>();
        
        if (eBlockMode == E_BLOCK_MODE.E_BLOCK_MODE_COLOR)
        {
            m_pColor.alpha = 1.0f;
            //m_pLight.alpha = m_fBackGroundAlpha;
        }
        else if (eBlockMode == E_BLOCK_MODE.E_BLOCK_MODE_RUNE)
        {
            //m_pLight.alpha = 1.0f;
            m_pColor.alpha = m_fBackGroundAlpha;

        }

    }
}
