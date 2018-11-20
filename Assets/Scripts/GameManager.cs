using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using GameInterface;
using System.Linq;
public class GameManager : MonoBehaviour {

    private static bool created = false;
    public event Action OnStartPause;
    public event Action OnResumeGame;
    public static GameManager Singleton;
    private int m_iCountDebugPressed;
    void Awake()
    {
       
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
            Singleton = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    // Use this for initialization
    void Start () {
        OnStartPause += ShowCursor;
        OnResumeGame += HideCursor;
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Quitting game with escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OnStartPause != null)
            {
                OnStartPause();
            }

            SetPauseElement(true);
        }
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNewLevel(string _SceneName)
    {
        OnStartPause = null;
        OnResumeGame = null;
        SceneManager.LoadScene(_SceneName, LoadSceneMode.Single);
    }

    public  void RequestResumeGame()
    {
        if (OnResumeGame != null)
        {
            OnResumeGame();
        }
        SetPauseElement(false);
    }

    private void SetCursorVisible(bool _bVisible)
    {
        Cursor.visible = _bVisible;
        
    }

    public void ShowCursor() { SetCursorVisible(true); Cursor.lockState = CursorLockMode.None; }
    public void HideCursor() { SetCursorVisible(false); Cursor.lockState = CursorLockMode.Confined; }

    private void SetPauseElement(bool _bIsOnPause)
    {
        foreach ( IPausable pausable in FindObjectsOfType<MonoBehaviour>().OfType<IPausable>())
        {
            if(_bIsOnPause)
            {
                pausable.OnPause();
            }
            else
            {
                pausable.OnResume();
            }
        }
    }
    
}
