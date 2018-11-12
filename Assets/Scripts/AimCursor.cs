using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimCursor : MonoBehaviour {

    private Camera camera;
    public Image cursorImage;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {


    }

    public void ChangeCursorStatus(bool _bIsHittingLoot)
    {
        Color cursorColor = _bIsHittingLoot ? Color.red : Color.grey;
        cursorImage.color = cursorColor;
    }

    private float GetPlayerLootRange()
    {
        return 5.0f;
    }
}
