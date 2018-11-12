using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeController : MonoBehaviour {
    public enum E_GaugeOrientation
    {
        LEFT,
        RIGHT
    };
    public E_GaugeOrientation GaugeOrientation;
    public Color GaugeColor;
    public Color FilledGaugeColor;
    public RectTransform GaugeRectTransform;
    public BlinkController PlayerBlinkController;

    private float m_fGaugeCompletion;

    

	// Use this for initialization
	void Start () {
        m_fGaugeCompletion = 1.0f;

    }
	
	// Update is called once per frame
	void Update () {

        m_fGaugeCompletion = PlayerBlinkController.GetCoolDownCompletion();
        float fSizeFromCompletion = GetTargetSizeFromCompletion();
        UpdateGaugeSize(fSizeFromCompletion);
        UpdateGaugeColor();

    }

    float GetTargetSizeFromCompletion()
    {
        // 1 completion =>  0 . 0 completion => fparentwidth;
        RectTransform parent = GaugeRectTransform.parent.GetComponent<RectTransform>();

        float fParentWidth = parent.rect.width;
        float fWidth = Mathf.Lerp(0.0f, fParentWidth, m_fGaugeCompletion);


        return fWidth;
    }


    void UpdateGaugeSize(float _fGaugeCompletion)
    {


        //if (GaugeOrientation == E_GaugeOrientation.LEFT)
        //{
        //    GaugeRectTransform.anchorMax = new Vector2(1.0f, GaugeRectTransform.anchorMax.y);
        //    GaugeRectTransform.anchorMin = new Vector2(1.0f, GaugeRectTransform.anchorMin.y);
        //    GaugeRectTransform.pivot = new Vector2(1.0f, GaugeRectTransform.pivot.y);

        //}
        //else if  (GaugeOrientation == E_GaugeOrientation.RIGHT)
        //{
        //    //GaugeRectTransform.anchorMax = new Vector2(0.0f, GaugeRectTransform.anchorMax.y);
        //    //GaugeRectTransform.anchorMin = new Vector2(0.0f, GaugeRectTransform.anchorMin.y);
        //    GaugeRectTransform.pivot = new Vector2(0.0f, GaugeRectTransform.pivot.y);


        //}
        GaugeRectTransform.sizeDelta = new Vector2(_fGaugeCompletion, GaugeRectTransform.sizeDelta.y);

    }

    void UpdateGaugeColor()
    {
        if(Mathf.Approximately(m_fGaugeCompletion,1.0f))
        {
            GaugeRectTransform.GetComponent<Image>().color = FilledGaugeColor;
        }
        else
        {
            GaugeRectTransform.GetComponent<Image>().color = GaugeColor;
        }
    }
}
