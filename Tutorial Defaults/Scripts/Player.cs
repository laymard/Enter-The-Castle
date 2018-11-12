using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float InteractionRange;
    public AimCursor aimcursor;

    private Dictionary<string, int> Inventory;
    private Camera PlayerCamera;
    private Loot CurrentAimedLoot;


	// Use this for initialization
	void Start () {
        PlayerCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        bool bIsHittingLoot = false;

        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, InteractionRange))
        {
            Loot lootHit = hit.transform.GetComponent<Loot>();
            if (lootHit != null)
            {
                CurrentAimedLoot = lootHit;
                bIsHittingLoot = true;
            }
        }
        else
        {
            CurrentAimedLoot = null;
        }

        aimcursor.ChangeCursorStatus(bIsHittingLoot);

        if (Input.GetAxis("Interact")>0.8f && CurrentAimedLoot !=null && CurrentAimedLoot.isActiveAndEnabled)
        {
            LootObject(CurrentAimedLoot);
            
        }

    }


    public void AddItemQuantity(string _sItemName,int _iQuantity)
    {
        Inventory[_sItemName] += _iQuantity;
    }

    public bool  TryGetItemQuantity(string _sItemName, ref int _iRetQuantity)
    {
        int iQuantity = 0;
        if(Inventory.TryGetValue(_sItemName,out iQuantity))
        {
            _iRetQuantity = iQuantity;
            return true;
        }


        return false;
    }

    public void LootObject(Loot _LootObject)
    {
        //AddItemQuantity(_LootObject.GetLootName(), _LootObject.GetLootQuantity());
        _LootObject.OnLootedByPlayer(this);

    }
}
