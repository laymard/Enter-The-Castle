using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour {

    private NavMeshAgent m_NavMeshAgent;
    private Rigidbody m_RigidBody;
    private Animator m_Animator;
    private Vector3 m_fCurrentGoToTarget;
    private NavMeshPath m_CurrentPath;

    public bool AllowRoaming;
    public float TimeToNewRoaming;
    public float RangeToNextDestination;

    private float m_fRoamingTimer;
	// Use this for initialization
	void Start () {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_fRoamingTimer = TimeToNewRoaming;
        m_Animator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateRoaming();
        UpdateAnimationParam();
    }

    void UpdateRoaming()
    {
        m_fRoamingTimer = m_fRoamingTimer > 0.0f ? m_fRoamingTimer - Time.deltaTime : m_fRoamingTimer;

        if (m_fRoamingTimer < 0.0f && AllowRoaming)
        {
            Vector3 vNewDestination = ComputeRandomDest();
            m_NavMeshAgent.SetDestination(vNewDestination);
            m_fRoamingTimer = TimeToNewRoaming;
        }

        if((transform.position - m_NavMeshAgent.destination).sqrMagnitude > Mathf.Pow(0.2f,2))
        {
            m_NavMeshAgent.Move(Vector3.zero);
        }
        
    }

    private void CalculateNewPath(Vector3 vDestination)
    {
        m_NavMeshAgent.CalculatePath(vDestination, m_CurrentPath);
        if(m_CurrentPath.status == NavMeshPathStatus.PathPartial)
        {
            m_fCurrentGoToTarget = m_CurrentPath.corners[m_CurrentPath.corners.Length - 1];
        }
    }

    private Vector3 ComputeRandomDest()
    {
        Vector3 vAgentPosition = transform.position;
        Vector2 vRandomDir = Random.insideUnitCircle;
        vRandomDir *= RangeToNextDestination;

        Vector3 vDest = vAgentPosition;
        vDest.x += vRandomDir.x;
        vDest.z += vRandomDir.y;

        return vDest;
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < m_NavMeshAgent.path.corners.Length - 1; i++)
        {
            Debug.DrawLine(m_NavMeshAgent.path.corners[i], m_NavMeshAgent.path.corners[i + 1], Color.red);
        }
    }

    private void UpdateAnimationParam()
    {
        float fSpeed = m_NavMeshAgent.velocity.magnitude;
        float fAngle = 0.0f;
        Vector3 fAxis = Vector3.up;
        Quaternion.FromToRotation(transform.forward, m_NavMeshAgent.destination - transform.position).ToAngleAxis(out fAngle, out fAxis);
        
        fAngle = Mathf.Clamp(fAngle / 180.0f  , - 1.0f, 1.0f);

        float fCurrentSpeed = m_RigidBody.velocity.magnitude;
        if (Mathf.Abs(fCurrentSpeed - fSpeed) > 0.1f)
        {

            Mathf.SmoothDamp(fCurrentSpeed, fSpeed, ref fCurrentSpeed, 1.0f, 1.0f);
        }
        else
        {
            fCurrentSpeed = fSpeed;
        }

        m_Animator.SetFloat("Forward", fCurrentSpeed);
        m_Animator.SetFloat("Turn", fAngle);
    }

}
