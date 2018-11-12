using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSender : MonoBehaviour {

    public UnityEvent OnEnableEvent;
    public UnityEvent OnStartEvent;

    // Use this for initialization
    private void OnEnable()
    {
        OnEnableEvent.Invoke();
    }

    void Start () {
        OnStartEvent.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
