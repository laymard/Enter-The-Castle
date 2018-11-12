using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
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
        }
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNewLevel(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName, LoadSceneMode.Single);
    }

    public  void RequestResumeGame()
    {
        if (OnResumeGame != null)
        {
            OnResumeGame();
        }
    }

    private void SetCursorVisible(bool _bVisible)
    {
        Cursor.visible = _bVisible;
    }

    public void ShowCursor() { SetCursorVisible(true); Cursor.lockState = CursorLockMode.None; }
    public void HideCursor() { SetCursorVisible(false); Cursor.lockState = CursorLockMode.Confined; }
}
