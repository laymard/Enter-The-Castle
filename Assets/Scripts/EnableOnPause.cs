using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//test
using UnityEngine.UI;

public class EnableOnPause : MonoBehaviour {

    private GameManager m_GameManager;

	// Use this for initialization
	void Start () {
        m_GameManager = GameManager.Instance;
        m_GameManager.OnStartPause += ShowObject;
        m_GameManager.OnResumeGame += HideObject;

    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartPause -= ShowObject;
        GameManager.Instance.OnResumeGame -= HideObject;
        
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStartPause += ShowObject;
        GameManager.Instance.OnResumeGame += HideObject;
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
       
       
    }


}
