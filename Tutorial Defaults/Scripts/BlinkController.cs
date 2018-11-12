using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class BlinkController : MonoBehaviour {
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

    // Use this for initialization
    void Start () {
        m_Player = GetComponent<Player>();
        m_fBlinkTimer = -1.0f;
        m_eBlinkStatus = E_BlinkStatus.NONE;
        m_mainCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(m_fBlinkTimer < 0.0f)
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
                    m_fBlinkTimer = BlinkCoolDown;
                    m_eBlinkStatus = E_BlinkStatus.NONE;
                }
            }


        }
        else
        {
            m_fBlinkTimer -= Time.deltaTime;
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
        return Mathf.Lerp(1.0f, 0.0f, m_fBlinkTimer / BlinkCoolDown);
        
    }


}
