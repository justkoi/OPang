using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public static Manager m_Instance = null;

    public static Manager I
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(Manager)) as Manager;
                if (m_Instance == null)
                {
                    Debug.Log("Manager New Failed!");
                }
            }

            return m_Instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        //Screen.SetResolution(480, 800, true);

        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Application.runInBackground = true;

        Application.targetFrameRate = 60;

    }
	// Use this for initialization
	void Start () {
        Awake();
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    
}
