using UnityEngine;
using System.Collections;

public class BattleMenuBack : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        //bClickCheck = true;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        Application.LoadLevel("MenuScene");



    }
}
