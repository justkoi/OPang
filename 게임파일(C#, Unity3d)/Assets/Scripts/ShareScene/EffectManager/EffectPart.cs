using UnityEngine;
using System.Collections;

public class EffectPart : MonoBehaviour {

	// Use this for initialization

    public float m_fTimer = 0.0f;
    public float m_fTime = 1.4f;
	void Start () {

        m_fTimer = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
        m_fTimer += Time.deltaTime;

        if(m_fTimer >= m_fTime)
            Destroy(this.gameObject);

	}
}
