using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {


    private Animator m_Animator;
    private Collider m_Collider;


    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        GoToIdle();
    }

    private void GoToIdle()
    {

        m_Animator.SetTrigger("OnCollide");

    }

    public void StartMeleeAttack()
    {
        m_Animator.SetTrigger("StartMeleeAttack");
    }


}
