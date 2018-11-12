using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Capacity : ScriptableObject
{

    // Time coolDown need to go from 0 to 100 percent
    public float FullRecoverTiming;
    private float m_fInvFullRecoverTiming;

    // 
    public bool EmptyGaugeOnTrigger;

    // Rate consumed on capacity usage (in percent)
    public float ConsumationRateOnTrigger;

    public float GaugeCompletionOnStart;

    private float m_fCurrentGaugeCompletion;
    
    

    public void OnEnable()
    {
        m_fCurrentGaugeCompletion = GaugeCompletionOnStart;
        m_fInvFullRecoverTiming = 1.0f/FullRecoverTiming;
    }

    public void UpdateCurrentCompletion(float _fDTime)
    {
        if(!Mathf.Approximately(m_fCurrentGaugeCompletion,1.0f))
        {
            float fCompletionToAdd = ComputeCompletionFromDeltaTime(_fDTime);
            m_fCurrentGaugeCompletion = Mathf.Min(1.0f, m_fCurrentGaugeCompletion + fCompletionToAdd);
        }
    }

    public void TriggerCapacity()
    {
        if(EmptyGaugeOnTrigger)
        {
            m_fCurrentGaugeCompletion = 0.0f;
        }
        else
        {
            m_fCurrentGaugeCompletion -= ConsumationRateOnTrigger;
        }
    }

    private float ComputeCompletionFromDeltaTime(float _fdTime)
    {
        return _fdTime * m_fInvFullRecoverTiming;
    }

    public bool CanTriggerCapacity()
    {
        if(EmptyGaugeOnTrigger)
        {
            return Mathf.Approximately(m_fCurrentGaugeCompletion, 1.0f);
        }
        else
        {
            return m_fCurrentGaugeCompletion > ConsumationRateOnTrigger;
        }
    }

    public float GetCooldownCompletion()
    {
        return m_fCurrentGaugeCompletion;
    }
}
