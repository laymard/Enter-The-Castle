using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using GameInterface;
using System.Linq;
public class GameManager : MonoBehaviour {

    public event Action OnStartPause;
    public event Action OnResumeGame;
    private static GameManager Singleton;

    public static GameManager Instance
    {
        get
        {
            if (Singleton == null)
            {
                Singleton = Instantiate(FindObjectOfType<GameManager>());
                DontDestroyOnLoad(Singleton);
            }
            return Singleton;
        }
    }
    private int m_iCountDebugPressed;

    // Use this for initialization
    void Start () {

        if (Singleton == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Start: " + this.gameObject);
            Singleton = Instantiate(this);
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            gameObject.SetActive(false);
        }
        Singleton.OnStartPause += ShowCursor;
        Singleton.OnResumeGame += HideCursor;
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Quitting game with escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Singleton.OnStartPause != null)
            {
                Singleton.OnStartPause.Invoke();
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
        if (Singleton.OnResumeGame != null)
        {
            Singleton.OnResumeGame.Invoke();
        }
        SetPauseElement(false);
    }

    private void SetCursorVisible(bool _bVisible)
    {
        Cursor.visible = _bVisible;
        
    }

    public void ShowCursor()
    {
        SetCursorVisible(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void HideCursor()
    {
        SetCursorVisible(false);
        Cursor.lockState = CursorLockMode.Confined;
    }

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
