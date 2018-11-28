using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using GameInterface;

public class BlinkController : MonoBehaviour, IPausable {
    enum E_BlinkStatus
    {
        NONE,
        POSITIONING
    };
    public Transform BlinkCursor;
    public float BlinkMaxRange;
    public float BlinkCoolDown;
    public AudioSource BlinkSound;

    private Player m_Player;
    private Vector3 m_vCurrentBlinkPosition;
    private float m_fBlinkTimer;
    private E_BlinkStatus m_eBlinkStatus;
    private Camera m_mainCamera;

    public Capacity Capacity;
    private bool m_bIsOnPause;

    // Use this for initialization
    void Start () {
        m_Player = GetComponent<Player>();
        m_fBlinkTimer = -1.0f;
        m_eBlinkStatus = E_BlinkStatus.NONE;
        m_mainCamera = Camera.main;
        m_bIsOnPause = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Capacity.CanTriggerCapacity() && !m_bIsOnPause)
        {
            BlinkCursor.gameObject.SetActive(false);
            if (Input.GetAxis("Capacity")>0.9f)
            {
                BlinkCursor.gameObject.SetActive(true);
                m_eBlinkStatus = E_BlinkStatus.POSITIONING;
                UpdateBlinkPosition();
            }
            else
            {
                if(m_eBlinkStatus == E_BlinkStatus.POSITIONING)
                {
                    StartBlink();
                    Capacity.TriggerCapacity();
                    m_eBlinkStatus = E_BlinkStatus.NONE;
                }
            }


        }
        else
        {
            Capacity.UpdateCurrentCompletion( Time.deltaTime);
        }


	}

    void StartBlink()
    {
        m_Player.transform.position = m_vCurrentBlinkPosition;
        BlinkSound.Play();
    }

    void UpdateBlinkPosition()
    {
        Vector3 vCameraFront = m_mainCamera.transform.forward;
        Vector3 vCameraPosition = m_mainCamera.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(vCameraPosition, vCameraFront, out hit, BlinkMaxRange))
        {
            m_vCurrentBlinkPosition = hit.point;
        }
        else
        {
            m_vCurrentBlinkPosition = vCameraPosition + vCameraFront * BlinkMaxRange;
        }

        BlinkCursor.transform.position = m_vCurrentBlinkPosition;
    }

    public float GetCoolDownCompletion()
    {
        return Capacity.GetCooldownCompletion();
        
    }

    public void OnPause()
    {
        m_bIsOnPause = true;
    }

    public void OnResume()
    {
        m_bIsOnPause = false;
    }

}
