using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;


public struct stVec2i
{
    public int m_nX;
    public int m_nY;
    stVec2i(int nX, int nY)
    {
        m_nX = nX;
        m_nY = nY;
    }

};

[System.Serializable]
public class CPopList
{
	public List<GameObject> m_BlockList = new List<GameObject>(); //!< 터트릴 블록 보관
    GameObject m_FocusBlock; //!< 포커싱된 블록


    public void Init(int nIndex)
    {

        if (GameScene.m_goBlockList[nIndex].GetComponent<Block>().m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
        {
            Debug.Log("RAINBOW1");
            return;
        }

        if (GameScene.m_goBlockList[nIndex].GetComponent<Block>().m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
            return;
        if (GameScene.m_goBlockList[nIndex].GetComponent<Block>().m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CHANGING)
            return;

        m_FocusBlock = null;


        GameScene.m_goBlockList[nIndex].GetComponent<Block>().m_bChecked = true;
        m_BlockList.Add(GameScene.m_goBlockList[nIndex]);


        while (true)
        {
            m_FocusBlock = getNextFocusBlock();

            if (m_FocusBlock == null)
                break;

            CheckAround();
        }

    }

    void CheckAround() //!< 상하좌우 블록을 체크하여 포함 //!< 이미 포함된 블록은 포함시키지 않는다
    {
        stVec2i stBlockIndex;
        stBlockIndex = m_FocusBlock.GetComponent<Block>().m_stIndex;

        SelectBlock(stBlockIndex.m_nX - 1, stBlockIndex.m_nY);
        SelectBlock(stBlockIndex.m_nX + 1, stBlockIndex.m_nY);
        SelectBlock(stBlockIndex.m_nX, stBlockIndex.m_nY - 1);
        SelectBlock(stBlockIndex.m_nX, stBlockIndex.m_nY + 1);
    }

    void SelectBlock(int nX, int nY)
    {
        if (nX < 0)
            return;
        if (nX >= UserDefine.D_BLOCK_WIDTH)
            return;
        if (nY < 0)
            return;
        if (nY >= UserDefine.D_BLOCK_HEIGHT)
            return;

        for (int i = 0; i < GameScene.m_goBlockList.Count; i++)
        {
            Block pBlock = GameScene.m_goBlockList[i].GetComponent<Block>();

            //if (pBlock.m_stIndex.m_nX == nX && pBlock.m_stIndex.m_nY == nY && pBlock.m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
            //{
            //    if (pBlock.m_bChecked == true)
            //    {
            //        Debug.Log(m_BlockList.Count + "  /RAINBOW2-4(레인보우 중복 비포함) : ");
            //        break;
            //    }



            //    if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
            //        break;
            //    if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CHANGING)
            //    {
            //        Debug.Log("Break! Change");
            //        break;
            //    }

            //    Debug.Log(m_BlockList.Count + "  /RAINBOW2(레인보우 포함됨) : " + m_FocusBlock.GetComponent<Block>().m_eBlockNumber);
            //    GameScene.m_goBlockList[i].GetComponent<Block>().m_bChecked = true;
            //    //GameScene.m_goBlockList[i].GetComponent<Block>().m_bFocused = true;
            //    m_BlockList.Add(GameScene.m_goBlockList[i]);
            //    break;
            //}
            //if (m_FocusBlock.GetComponent<Block>().m_eBlockNumber == E_BLOCK_NUMBER.E_BLOCK_NUMBER_ITEM_RAINBOW)
            //{
            //    if (pBlock.m_stIndex.m_nX == nX && pBlock.m_stIndex.m_nY == nY && pBlock.m_eBlockNumber == m_BlockList[0].GetComponent<Block>().m_eBlockNumber)
            //    {
            //        if (pBlock.m_bChecked == true)
            //        {
            //            Debug.Log(m_BlockList.Count + "  /RAINBOW3-4(레인보우 중복 비포함) : ");
            //            break;
            //        }
                   


            //        if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
            //            break;
            //        if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CHANGING)
            //        {
            //            Debug.Log("Break! Change");
            //            break;
            //        }


            //        Debug.Log(m_BlockList.Count + "  /RAINBOW3(레인보우에서 포함함) : " + m_BlockList[0].GetComponent<Block>().m_eBlockNumber);
            //        GameScene.m_goBlockList[i].GetComponent<Block>().m_bChecked = true;
            //        m_BlockList.Add(GameScene.m_goBlockList[i]);
            //        break;
            //    }
            //}
            //else 
                
            if (pBlock.m_stIndex.m_nX == nX && pBlock.m_stIndex.m_nY == nY && pBlock.m_eBlockNumber == m_FocusBlock.GetComponent<Block>().m_eBlockNumber)
            {
                if (pBlock.m_bChecked == true)
                    break;


                if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CREATING)
                    break;
                if (pBlock.m_eBlockState == E_BLOCK_STATE.E_BLOCK_STATE_CHANGING)
                {
                    Debug.Log("Break! Change");
                    break;
                }


                GameScene.m_goBlockList[i].GetComponent<Block>().m_bChecked = true;
                m_BlockList.Add(GameScene.m_goBlockList[i]);
                break;
            }
        }
    }

    GameObject getNextFocusBlock() //!< 가장 다음 포커싱 될 블록 반환
    {
        for (int i = 0; i < m_BlockList.Count; i++)
        {
            if (m_BlockList[i].GetComponent<Block>().m_bFocused == false)
            {
                m_BlockList[i].GetComponent<Block>().m_bFocused = true; //!< 포커싱체크
                //Debug.Log("This Block" + i.ToString());
                return m_BlockList[i];
            }
        }
        return null;
    }


};