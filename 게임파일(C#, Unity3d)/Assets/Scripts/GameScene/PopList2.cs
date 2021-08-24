using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

[System.Serializable]

enum E_POP_TYPE
{
    E_POP_TYPE_LINE,
    E_POP_TYPE_ROW,
    E_POP_TYPE_MAX
};


class CPopList2
{
	public List<GameObject> m_BlockList = new List<GameObject>(); //!< 터트릴 블록 보관
    public int m_nIndex = 0;
    public GameScene g_GameScene = null;
    public void Init(int nIndex, E_POP_TYPE ePopType)
    {
        g_GameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
        m_nIndex = 0;
        
        if (ePopType == E_POP_TYPE.E_POP_TYPE_LINE)
        {
            int nSearchBlock = g_GameScene.SearchBlock(0, nIndex);

            if(nSearchBlock == -1)
                return;
			

            GameObject goObject = GameScene.m_goBlockList[ nSearchBlock ];
			
			E_BLOCK_LIGHT eNow = goObject.GetComponent<Block>().m_eBlockLight;
			
            m_nIndex = 0;
            m_BlockList.Add( goObject );

            for (int i = 0; i < UserDefine.D_MAP_WIDTH - 1; i++)
            {
                int nSearchBlockIndex = g_GameScene.SearchBlock(i+1, nIndex);
                if(nSearchBlock == -1)
                    return;

                GameObject goObjectTemp = GameScene.m_goBlockList[ nSearchBlock ];
				
				E_BLOCK_LIGHT eNow2 = goObjectTemp.GetComponent<Block>().m_eBlockLight;
				
				
                m_BlockList.Add( goObjectTemp );
				
				E_BLOCK_LIGHT eFirst = m_BlockList[i].GetComponent<Block>().m_eBlockLight;
				E_BLOCK_LIGHT eSecond = m_BlockList[i + 1].GetComponent<Block>().m_eBlockLight;

                if (eFirst != eSecond)
                {
                    break;
                }
            }
        }
        else if (ePopType == E_POP_TYPE.E_POP_TYPE_ROW)
        {
            g_GameScene.SearchBlock(nIndex, 0);


        }
    }
};