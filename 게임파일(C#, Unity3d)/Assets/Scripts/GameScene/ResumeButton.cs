using UnityEngine;
using System.Collections;

public class ResumeButton : MonoBehaviour
{

    public GameScene g_GameScene = null;

    // Use this for initialization
    void Start()
    {
        g_GameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        g_GameScene.Resume();
    }
}
