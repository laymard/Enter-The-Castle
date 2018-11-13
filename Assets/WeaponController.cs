using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {


    private Animator m_Animator;
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;
    private Transform m_WeaponContainer;
    private AudioSource m_AudioSource;

    public float ThrowSpeed;
    public float ComingBackTiming;
    public float ComingBackSpeed;

    //rad per seconds
    public float RotationSpeed;
    public bool DebugShootEnabled;

    private bool m_bIsOnRangeAttack;
    private bool m_bIsComingBack;


    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_WeaponContainer = transform.parent;
        m_bIsOnRangeAttack = false;
        m_bIsComingBack = false;
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

        float sqrDist = (transform.position - m_WeaponContainer.transform.position).sqrMagnitude;
        if(m_bIsOnRangeAttack && sqrDist>1.0f)
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

        if(m_bIsOnRangeAttack)
        {
            m_Rigidbody.isKinematic = true;
            m_bIsOnRangeAttack = false;
            m_AudioSource.Stop();
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
        m_Rigidbody.angularVelocity = new Vector3(0.0f, -RotationSpeed, 0.0f);
        // apply velocity
        m_Rigidbody.velocity = Camera.main.transform.forward * ThrowSpeed;
        m_Rigidbody.isKinematic = false;

        //deactivate animator
        m_Animator.enabled = false;
        m_bIsOnRangeAttack = true;

        m_AudioSource.Play();

    }

    private IEnumerator ComingBackToPlayer()
    {
        Vector3 vStartPosition = transform.position;
        Quaternion qStartOrientation = transform.rotation;
        float fCoveredDist = 0.0f;
        SafeSetParent(null);
        Vector3 vWeaponToTarget = -ComputeWeaponToWeaponContainer();
        while (vWeaponToTarget.sqrMagnitude>0.01f)
        {
            Vector3 vDirToTarget = vWeaponToTarget.normalized;
            float fDistToCover = Vector3.Distance(vStartPosition, m_WeaponContainer.transform.position);
            transform.position += vDirToTarget * ComingBackSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(qStartOrientation, m_WeaponContainer.transform.rotation, fCoveredDist/ fDistToCover);

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
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.isKinematic = true;

        //deactivate animator
        m_Animator.enabled = false;
        m_bIsOnRangeAttack = false;
        m_bIsComingBack = true;

        m_AudioSource.Play();
    }

    private void OnGettingBack()
    {

        transform.position = m_WeaponContainer.transform.position;
        transform.rotation = m_WeaponContainer.transform.rotation;
        SafeSetParent(m_WeaponContainer.transform);
        
        // apply velocity
        m_Rigidbody.isKinematic = true;

        //deactivate animator
        m_Animator.enabled = true;
        m_bIsOnRangeAttack = false;
        m_bIsComingBack = false;

        m_AudioSource.Stop();
    }

    private bool CanThrowWeapon()
    {
        return !m_bIsOnRangeAttack && !m_bIsComingBack;
    }

    public Vector3 ComputeWeaponToWeaponContainer()
    {
        return transform.position - m_WeaponContainer.transform.position;
    }
}
