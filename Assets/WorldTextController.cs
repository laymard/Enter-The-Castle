using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextController : MonoBehaviour {



    private Animator UIAnimator;
    private Interactible LinkedInteractible;
    private RectTransform m_RectTransform;
    private Player m_mainPlayer;
    public bool AllowDebugInteraction;

	// Use this for initialization
	void Start () {
        UIAnimator = GetComponent<Animator>();
        m_mainPlayer = FindObjectOfType<Player>();
        m_mainPlayer.OnCurrentInteractibleChanged.AddListener(ChangedCurrentInteractible);
        m_RectTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if(AllowDebugInteraction)
        {
            SetTriggerByInput(KeyCode.H, "Hide");
            SetTriggerByInput(KeyCode.V, "Validate");
            SetTriggerByInput(KeyCode.M, "Show");
            SetTriggerByInput(KeyCode.W, "Denied");
        }

        transform.forward = Camera.main.transform.forward;
        m_RectTransform.forward = Camera.main.transform.forward;
    }

    private void ChangedCurrentInteractible(Interactible _interactible)
    {
        // delete delegate from previous Interactible
        if (LinkedInteractible != null)
        {
            LinkedInteractible.OnInteractionFailed.RemoveListener(DeniedInteraction);
            LinkedInteractible.OnInteractionSuccess.RemoveListener(ValidateInteraction);
        }
        if(_interactible == null)
        {
            HideInteraction();
        }
        else
        {

            ResetAllAnimatorParam();
            LinkedInteractible = _interactible;
            ShowInteraction();
            LinkedInteractible.OnInteractionSuccess.AddListener(ValidateInteraction);
            LinkedInteractible.OnInteractionFailed.AddListener(DeniedInteraction);
            ChangePositionFromInteractible(_interactible);

        }
    }


    private void SetTriggerByInput(KeyCode _eKeyCode,string _sTrigger)
    {
        if (Input.GetKeyUp(_eKeyCode))
        {
            UIAnimator.SetTrigger(_sTrigger);

        }
    }

    public void DeniedInteraction()
    {
        UIAnimator.SetTrigger("Denied");
    }
    
    public void ShowInteraction()
    {
        UIAnimator.SetTrigger("Show");

    }

    public void HideInteraction()
    {
        UIAnimator.SetTrigger("Hide");

    }

    public void ValidateInteraction()
    {
        UIAnimator.SetTrigger("Validate");

    }

    private void ChangePositionFromInteractible(Interactible _interactible)
    {
        Vector3 vInteractiblePosition = _interactible.transform.position;

        transform.position = vInteractiblePosition + Vector3.up * 0.15f;
    }

    private void ResetAllAnimatorParam()
    {
        UIAnimator.ResetTrigger("Hide");
    }
}
