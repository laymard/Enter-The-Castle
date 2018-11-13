using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;


[Serializable]
public class AudioData
{
    public AudioClip AudioClip;
    public bool Loop;

    public void SetToAudioSource(ref AudioSource _audioSource)
    {
        _audioSource.clip = AudioClip;
        _audioSource.loop = Loop;
    }
}


[CreateAssetMenu()]
public class WeaponAudio : ScriptableObject {




    public AudioData ImpactData;

    public AudioData GettingBackData;
    public AudioData MeleeData;
    public AudioData WooshLoopData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
