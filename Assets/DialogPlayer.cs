using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPlayer : MonoBehaviour {

    public bool ShowWorldSubtitle;

    private AudioSource m_AudioSource;


	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayDialog(DialogScriptableObject _DialogToPlay)
    {
        m_AudioSource.clip = _DialogToPlay.VoiceDialog;
        m_AudioSource.Play();
    }
}
