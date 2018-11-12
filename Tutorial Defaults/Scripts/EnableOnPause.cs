using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//test
using UnityEngine.UI;

public class EnableOnPause : MonoBehaviour {

    private GameManager m_GameManager;

	// Use this for initialization
	void Start () {
        m_GameManager = GameManager.Singleton;
        m_GameManager.OnStartPause += ShowObject;
        m_GameManager.OnResumeGame += HideObject;

    }

    private void OnDisable()
    {
        m_GameManager.OnStartPause -= ShowObject;
        m_GameManager.OnResumeGame -= HideObject;
        
    }

    private void OnEnable()
    {
        if(m_GameManager == null)
        {
            m_GameManager = GameManager.Singleton;

        }
        m_GameManager.OnStartPause += ShowObject;
        m_GameManager.OnResumeGame += HideObject;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void ShowObject()
    {
        foreach (Transform transfrom in GetComponentsInChildren<Transform>())
        {

                transfrom.gameObject.SetActiveRecursively(true);
            
        }
    }

    void HideObject()
    {
        foreach(Transform transfrom in GetComponentsInChildren<Transform>())
        {

                transfrom.gameObject.SetActiveRecursively(false);
            
        }
        gameObject.SetActive(true);
       
    }


}
