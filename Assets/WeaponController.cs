using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterface;



public class WeaponController : MonoBehaviour, IPausable
{
    enum E_WeaponState
    {
        HOLDED,
        THROWING,
        LANDED,
        COMING_BACK
    }

    private Animator m_Animator;
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;
    private Transform m_WeaponContainer;
    private AudioSource m_AudioSource;
    
    public float ThrowSpeed;
    public float ComingBackTiming;
    public float ComingBackSpeed;
    public WeaponAudio WeaponAudio;
    public float DistanceToGoToNormalRotation;
    public float FallingTiming;

    //rad per seconds
    public float RotationSpeed;
    public bool DebugShootEnabled;

    private bool m_bIsOnPause = false;
    private E_WeaponState m_WeaponState;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_WeaponContainer = transform.parent;
        m_WeaponState = E_WeaponState.HOLDED;
    }

    // Update is called once per frame
    void Update()
    {
        if(DebugShootEnabled)
        {
            if(CanThrowWeapon())
            {
                if(Input.GetKeyDown(KeyCode.P))
                {
                    StartRangeAttack();
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartComingBack();
                StartCoroutine("ComingBackToPlayer");
            }
        }

        if (CanThrowWeapon())
        {
            if (Input.GetAxis("Fire3")>0.9f)
            {
                StartRangeAttack();
            }
        }

        if(CanTakeWeaponBack())
        {
            if (Input.GetAxis("Fire3") > 0.9f )
            {
                StartComingBack();
                StartCoroutine("ComingBackToPlayer");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_WeaponState==E_WeaponState.THROWING)
        {
            m_Rigidbody.isKinematic = true;
            m_WeaponState = E_WeaponState.LANDED;
            StopCoroutine("Rotate");
            StopCoroutine("FallingDelayed");
            WeaponAudio.ImpactData.SetToAudioSource(ref m_AudioSource);
            m_AudioSource.Play();
        }
        else
        {
            GoToIdle();
        }
    }

    private void GoToIdle()
    {

        m_Animator.SetTrigger("OnCollide");

    }

    public void StartMeleeAttack()
    {
        m_Animator.SetTrigger("StartMeleeAttack");
    }

    public void StartRangeAttack()
    {
        // remove from parent
        transform.parent=null;
        transform.forward = Vector3.up;

        // apply rotation speed
        StartCoroutine("Rotate");
        StartCoroutine("FallingDelayed");
        // apply velocity
        m_Rigidbody.velocity = Camera.main.transform.forward * ThrowSpeed;
        m_Rigidbody.isKinematic = false;

        //deactivate animator
        m_Animator.enabled = false;
        m_WeaponState = E_WeaponState.THROWING;

        WeaponAudio.WooshLoopData.SetToAudioSource(ref m_AudioSource);
        m_AudioSource.Play();

    }

    private IEnumerator ComingBackToPlayer()
    {
        Vector3 vStartPosition = transform.position;
        Quaternion qStartOrientation = transform.rotation;
        float fCoveredDist = 0.0f;
        SafeSetParent(null);
        Vector3 vWeaponToTarget = -ComputeWeaponToWeaponContainer();
        bool bTriggeredStopRotation = false;
        while (vWeaponToTarget.sqrMagnitude>0.05f)
        {
            Vector3 vDirToTarget = vWeaponToTarget.normalized;
            float fDistToCover = Vector3.Distance(vStartPosition, m_WeaponContainer.transform.position);
            transform.position += vDirToTarget * ComingBackSpeed * Time.deltaTime;
            if(!bTriggeredStopRotation && vWeaponToTarget.sqrMagnitude < Mathf.Pow(DistanceToGoToNormalRotation, 2) )
            {

                StopCoroutine("Rotate");
                StopCoroutine("FallingDelayed");
                StartCoroutine("RotateToNormalPos");
                bTriggeredStopRotation = true;
            }
            fCoveredDist += ComingBackSpeed * Time.deltaTime;
            vWeaponToTarget = -ComputeWeaponToWeaponContainer();
            yield return null;
        }

        OnGettingBack();
        yield return null;
    }

    // [not working on scaled objects]adjust scale in case of strange parent scale
    private void SafeSetParent(Transform _newParent)
    {
        transform.parent = null;
        Vector3 lossyScale = transform.lossyScale;
        if(_newParent == null)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {

            transform.localScale = new Vector3(1.0f / _newParent.localScale.x, 1.0f / _newParent.localScale.y, 1.0f / _newParent.localScale.z);
        }
        transform.parent = _newParent;
        transform.localScale = lossyScale;
    }

    private void StartComingBack()
    {
        // apply rotation speed
        StopCoroutine("Rotate");
        StartCoroutine("Rotate");
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
        //deactivate animator
        m_Animator.enabled = false;
        m_WeaponState = E_WeaponState.COMING_BACK;

        WeaponAudio.WooshLoopData.SetToAudioSource(ref m_AudioSource);
        m_AudioSource.Play();
    }

    private void OnGettingBack()
    {
        StopCoroutine("RotateToNormalPos");
        transform.position = m_WeaponContainer.transform.position;
        transform.rotation = m_WeaponContainer.transform.rotation;
        SafeSetParent(m_WeaponContainer.transform);
        
        // apply velocity
        m_Rigidbody.isKinematic = true;

        //deactivate animator
        m_Animator.enabled = true;
        m_WeaponState = E_WeaponState.HOLDED;

        WeaponAudio.GettingBackData.SetToAudioSource(ref m_AudioSource);
        m_AudioSource.Play();
    }

    private bool CanThrowWeapon()
    {
        return m_WeaponState==E_WeaponState.HOLDED && !m_bIsOnPause;
    }

    public Vector3 ComputeWeaponToWeaponContainer()
    {
        return transform.position - m_WeaponContainer.transform.position;
    }

    private void ChangeSound(AudioClip _audioClip)
    {
        m_AudioSource.clip = _audioClip;
    }

    IEnumerator Rotate()
    {
        Vector3 vRotationAngle = new Vector3(0f, -RotationSpeed * Time.deltaTime, 0f);
        while(true)
        {
            transform.Rotate(vRotationAngle, Space.World);
            yield return null;
        }
    }

    IEnumerator RotateToNormalPos()
    {
        Vector3 vStartPosition = transform.position;
        Quaternion qStartOrientation = transform.rotation;
        float fCoveredDist = 0.0f;
        SafeSetParent(null);
        Vector3 vWeaponToTarget = -ComputeWeaponToWeaponContainer();
        while (vWeaponToTarget.sqrMagnitude > 0.05f)
        {
            Vector3 vDirToTarget = vWeaponToTarget.normalized;
            float fDistToCover = Vector3.Distance(vStartPosition, m_WeaponContainer.transform.position);

            transform.rotation = Quaternion.Lerp(qStartOrientation, m_WeaponContainer.transform.rotation, fCoveredDist/ fDistToCover);

            fCoveredDist += ComingBackSpeed * Time.deltaTime;
            vWeaponToTarget = -ComputeWeaponToWeaponContainer();
            yield return null;
        }

        yield return null;
    }

    public bool CanTakeWeaponBack()
    {
        float sqrDist = (transform.position - m_WeaponContainer.transform.position).sqrMagnitude;
        if (( m_WeaponState==E_WeaponState.THROWING && sqrDist > 1.0f || m_WeaponState == E_WeaponState.LANDED)    && !m_bIsOnPause)
            return true;

        return false;
    }

    public void OnPause()
    {
        m_bIsOnPause = true;
    }

    public void OnResume()
    {
        m_bIsOnPause = false;
    }

    IEnumerator FallingDelayed()
    {
        float fTimer = FallingTiming;
        while(fTimer>0.0f)
        {
            fTimer -= Time.deltaTime;
            yield return null;

        }

        OnStartFalling();

        yield return null;
    }

    public void OnStartFalling()
    {
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
    }
    
}

