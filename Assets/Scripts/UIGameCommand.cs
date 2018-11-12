using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameCommand : MonoBehaviour {

    public static UIGameCommand Singleton;
    public static bool IsCreated = false;

    private GameManager m_GameManager;

	// Use this for initialization
	void Start () {
        m_GameManager = GameManager.Singleton;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeScene(string _sSceneName)
    {
        m_GameManager.LoadNewLevel(_sSceneName);
    }

    public void RequestResumeGame()
    {
        m_GameManager.RequestResumeGame();
    }

    public void RequestCursorVisibility(bool _bVisibility)
    {
       if(_bVisibility)
        {
            m_GameManager.ShowCursor();
        }
        else
        {
            m_GameManager.HideCursor();
        }
    }
}
