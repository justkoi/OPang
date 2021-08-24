using UnityEngine;
using System.Collections;

public class StageMenuScene : MonoBehaviour
{
    static public SoundManager g_SoundMng = null;
    public UILabel m_GoldNumber = null;
    public UILabel m_GoldNumberMinus = null;

    static private int m_nStage = 7;

    public BoxCollider[] m_ButtonCollider = new BoxCollider[m_nStage];

    public GameObject m_pUIBlind = null;

    public GameObject m_pItemStore = null;
    public GameObject m_pItemSlot_1 = null;
    public GameObject m_pItemSlot_2 = null;
    public GameObject m_pItemButton_1 = null;
    public GameObject m_pItemButton_2 = null;

    public int m_nStep = 0;

    static public int g_nMinusGold = 0;

    
    // Use this for initialization
    void Start()
    {
        GameSateData.ManageData();
        GameSateData.LoadData();
        GameScene.g_nGold = GameSateData.myData.manage.nGold;

        m_nStep = 0;
        g_SoundMng = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        g_SoundMng.Play(10, true, 0.6f);

        m_GoldNumber = GameObject.Find("GoldNumber").GetComponentInChildren<UILabel>();
        m_GoldNumber.text = GameScene.g_nGold.ToString();

        m_GoldNumberMinus = GameObject.Find("GoldMinusNumber").GetComponentInChildren<UILabel>();
        m_GoldNumberMinus.text = "( -" + g_nMinusGold.ToString() + ")";


        for (int i = 0; i < m_nStage; i++)
        {
            m_ButtonCollider[i] = GameObject.Find("Button_Stage_" + (i+1).ToString()).GetComponent<BoxCollider>();
        }

        m_pUIBlind = GameObject.Find("UIBlind");
        m_pUIBlind.active = false;

        m_pItemStore = GameObject.Find("ItemStore");

        for (int i = 0; i<m_pItemStore.transform.childCount; i++)
        {
            m_pItemStore.transform.GetChild(i).gameObject.active = false;
        }


        m_pItemSlot_1 = GameObject.Find("ItemSlot_1");
        m_pItemSlot_2 = GameObject.Find("ItemSlot_2");
        m_pItemButton_1 = GameObject.Find("ItemButton_1");
        m_pItemButton_2 = GameObject.Find("ItemButton_2");

        m_pItemStore.active = false;
        m_pItemSlot_1.active = false;
        m_pItemSlot_2.active = false;
        m_pItemButton_1.active = false;
        m_pItemButton_2.active = false;

        m_GoldNumberMinus.transform.gameObject.active = false;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("[1] " + GameScene.g_P1_Inventory.Inv_kind[0, 0]);
        Debug.Log("[2] " + GameScene.g_P1_Inventory.Inv_kind[0, 1]);
        if (GameScene.g_P1_Inventory.Inv_kind[0, 0] == -1)
        {
            m_pItemSlot_1.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "null";
        }
        else
        {
            m_pItemSlot_1.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "Item_" + (GameScene.g_P1_Inventory.Inv_kind[0, 0]+1);
        }

        if (GameScene.g_P1_Inventory.Inv_kind[0, 1] == -1)
        {
            m_pItemSlot_2.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "null";
        }
        else
        {
            m_pItemSlot_2.transform.FindChild("Item").gameObject.GetComponent<UISprite>().spriteName = "Item_" + (GameScene.g_P1_Inventory.Inv_kind[0, 1]+1);
        }
    }

    public void Open_ItemStore()
    {
        m_pUIBlind.active = true;

        for (int i = 0; i < m_nStage; i++)
        {
            m_ButtonCollider[i].center = new Vector3(99999, 0, 0);
        }
        m_pItemStore.active    = true;

        for (int i = 0; i < m_pItemStore.transform.childCount; i++)
        {
            m_pItemStore.transform.GetChild(i).gameObject.active = true;
        }

        m_pItemSlot_1.active   = true;
        m_pItemSlot_2.active   = true;
        m_pItemButton_1.active = true;
        m_pItemButton_2.active = true;
        m_GoldNumberMinus.transform.gameObject.active = true;
        m_nStep = 1;
    }
    public void Close_ItemStore()
    {
        m_pUIBlind.active = false;

        for (int i = 0; i < m_nStage; i++)
        {
            m_ButtonCollider[i].center = new Vector3(0, 0, 0);
        }
        m_pItemStore.active     = false;

        for (int i = 0; i < m_pItemStore.transform.childCount; i++)
        {
            m_pItemStore.transform.GetChild(i).gameObject.active = false;
        }
        m_pItemSlot_1.active    = false;
        m_pItemSlot_2.active    = false;
        m_pItemButton_1.active = false;
        m_pItemButton_2.active = false;
        m_GoldNumberMinus.transform.gameObject.active = false;
        m_nStep = 0;

        g_nMinusGold = 0;

        GameScene.g_P1_Inventory.Inv_kind[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_kind[0, 1] = -1;

        GameScene.g_P1_Inventory.Inv_num[0, 0] = -1;
        GameScene.g_P1_Inventory.Inv_num[0, 1] = -1;


        m_GoldNumberMinus.text = "( -" + StageMenuScene.g_nMinusGold.ToString() + ")";

        
    }

    void OnDestroy()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

}
