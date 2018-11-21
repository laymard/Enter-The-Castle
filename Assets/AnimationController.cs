using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour {

    public Transform CurrentTarget;
    private bool m_bGoingToTarget;

    public string TurnParamName;

    private float TurnParam
    {
        get { return m_TurnParam; }
        set
        {
            if( !Mathf.Approximately(value,m_TurnParam))
            {
                m_TurnParam = value;
                StopCoroutine(SmoothTurn);
                SmoothTurn = SetAnimatorParam(TurnParamName, TurnParam);
                StartCoroutine(SmoothTurn);
            }
        }

    }

    private float m_TurnParam;
    public string ForwardParamName;
    private float ForwardParam
    {
        get { return m_fForwardParam; }
        set
        {
            if (!Mathf.Approximately(value, m_fForwardParam))
            {
                m_fForwardParam = value;
                StopCoroutine(SmoothForward);
                SmoothForward = SetAnimatorParam(ForwardParamName, ForwardParam);
                StartCoroutine(SmoothForward);

            }
        }
    }
    private float m_fForwardParam;
    public UnityEvent OnTargetReach;
    public float ReachDistanceThreshold;
    public float ReachAngleThreshold;


    public float MaxForwardSpeed;
    public float MaxTurnSpeed;

    // components
    private Animator m_Animator;

    // coroutine 
    private IEnumerator SmoothForward;
    private IEnumerator SmoothTurn;

    // Use this for initialization
    void Start () {
        m_Animator = GetComponent<Animator>();
        m_bGoingToTarget = false;
        SmoothForward = SetAnimatorParam(ForwardParamName, ForwardParam);
        SmoothTurn = SetAnimatorParam(TurnParamName, TurnParam);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 vPositionToTarget = CurrentTarget.position - transform.position;
        Vector3 vForward = transform.forward;
        Quaternion qRotationToTarget = Quaternion.FromToRotation(vForward, vPositionToTarget);
        float fHorizontalRotation = qRotationToTarget.eulerAngles.y;

        bool bInFrontOfTransform = Vector3.Dot(vForward,vPositionToTarget)>0.0f;
        bool bInRange = vPositionToTarget.sqrMagnitude < Mathf.Pow(ReachDistanceThreshold, 2);

        float fNextTurnValue = 0.0f; ;
        float fNextForwardValue=0.0f;

        if(bInRange)
        {
            m_bGoingToTarget = false;
            fNextForwardValue = 0.0f;
            fNextTurnValue = 0.0f;
            OnTargetReach.Invoke();
        }

        if(m_bGoingToTarget)
        {

            if (!bInFrontOfTransform)
            {
                fNextForwardValue = 0.0f;

            }
            else
            {
                fNextForwardValue = MaxForwardSpeed;
            }

            if(Mathf.Abs(fHorizontalRotation)< ReachAngleThreshold)
            {
                fNextTurnValue = 0.0f;
            }
            else
            {
                if (Vector3.Dot(transform.right, vPositionToTarget)<0.0f)
                {
                    fNextTurnValue = -MaxTurnSpeed;

                }
                else
                {
                    fNextTurnValue = MaxTurnSpeed;
                }
            }
        }

        TurnParam = fNextTurnValue;
        ForwardParam = fNextForwardValue;

    }

    public void GoToTarget(Vector3 vTarget)
    {
        CurrentTarget.position = vTarget;
        m_bGoingToTarget = true;
    }

    private void ApplyParams()
    {
        m_Animator.SetFloat(TurnParamName, TurnParam);
        m_Animator.SetFloat(ForwardParamName, ForwardParam);
    }

    IEnumerator SetAnimatorParam(string ParamName,float fTargetValue)
    {
        float fTimePassed = 0.0f;
        float fStartValue = m_Animator.GetFloat(ParamName);
        while (Mathf.Abs(m_Animator.GetFloat(ParamName) - fTargetValue) > Mathf.Epsilon)
        {
            float value = Mathf.Lerp(fStartValue, fTargetValue, fTimePassed);
            m_Animator.SetFloat(ParamName, value);
            fTimePassed += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
        
}
