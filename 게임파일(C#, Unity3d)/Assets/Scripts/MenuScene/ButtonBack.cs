using UnityEngine;
using System.Collections;

public class ButtonBack : MonoBehaviour
{
    public Animation pAni = null;
    public BoxCollider pButton1Col = null;
    public BoxCollider pButton2Col = null;
    public BoxCollider pButton3Col = null;
    public BoxCollider pButton4Col = null;

    public GameObject pButton1 = null;
    public GameObject pButton2 = null;
    public GameObject pButton3 = null;
    public GameObject pButton4 = null;
    public GameObject pTitle = null;

    Vector3 stTitleVec3;
    Vector3 stButton1Vec3;
    Vector3 stButton2Vec3;
    Vector3 stButton3Vec3;
    Vector3 stButton4Vec3;

    bool bClickCheck;
    int nButtonSpeed1;
    int nButtonSpeed2;
    int nTitleSpeed;
    
    void Start()
    {
        pButton1 = GameObject.Find("StartButton");
        pButton2 = GameObject.Find("OptionButton");
        pButton3 = GameObject.Find("CreditButton");
        pButton4 = GameObject.Find("ExitButton");
        pTitle = GameObject.Find("Title");

        pAni = GetComponent<Animation>();

        pButton1Col = pButton1.GetComponent<BoxCollider>();
        pButton2Col = pButton2.GetComponent<BoxCollider>();
        pButton3Col = pButton3.GetComponent<BoxCollider>();
        pButton4Col = pButton4.GetComponent<BoxCollider>();
        
        bClickCheck = false;
        
        nTitleSpeed = 100;
        nButtonSpeed1 = 100;
        nButtonSpeed2 = 100;
    }

    void Update()
    {
        if (bClickCheck == true)
        {
            
            pButton1Col.enabled = false;            // 버튼 충돌 종료
            pButton2Col.enabled = false;
            pButton3Col.enabled = false;
            pButton4Col.enabled = false;


            if (stTitleVec3.y <= 350.0f)
            {
                stTitleVec3.y += nTitleSpeed * -10.0f * Time.deltaTime;
                pTitle.transform.localPosition = stTitleVec3;
                nTitleSpeed -= 10;
            }
            
            
            if (stButton1Vec3.x >= -700.0f && stButton1Vec3.x <= 0.0f)
            {
                stButton1Vec3.x += nButtonSpeed1 * 10.0f * Time.deltaTime;
                stButton2Vec3.x += nButtonSpeed1 * 10.0f * Time.deltaTime;
                pButton1.transform.localPosition = stButton1Vec3;
                pButton2.transform.localPosition = stButton2Vec3;
                nButtonSpeed1 -= 20;
            }

            if (stButton3Vec3.x <= 700.0f && stButton3Vec3.x >= 0.0f)
            {
                stButton3Vec3.x += nButtonSpeed2 * -10.0f * Time.deltaTime;
                stButton4Vec3.x += nButtonSpeed2 * -10.0f * Time.deltaTime;
                pButton3.transform.localPosition = stButton3Vec3;
                pButton4.transform.localPosition = stButton4Vec3;
                nButtonSpeed2 -= 20;
            }
        }
        
    }

    void OnClick()
    {
        bClickCheck = true;
        stTitleVec3 = pTitle.transform.localPosition;
        stButton1Vec3 = pButton1.transform.localPosition;
        stButton2Vec3 = pButton2.transform.localPosition;
        stButton3Vec3 = pButton3.transform.localPosition;
        stButton4Vec3 = pButton4.transform.localPosition;
        //pAni.playAutomatically = true;
    }
}