using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ItemRequirement
{
    public string ItemName;
    public int ItemQuantity;
}

public class Interactible : MonoBehaviour {

    public ItemRequirement[] ItemsRequirement;

    public UnityEvent OnInteractionSuccess;
    public UnityEvent OnInteractionFailed;
    public Inventory InventoryToCheck;

    public bool NeedPlayerInFront;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LaunchInteraction(Player _pPlayer)
    {
        if(HasAllConditionToInteract(_pPlayer))
        {
            OnInteractionSuccess.Invoke();
        }
        else
        {
            OnInteractionFailed.Invoke();
        }
    }


    public bool CanBeTriggeredByPlayer(Player _player)
    {
        // for now can trigger only if player is in front
        if(!NeedPlayerInFront)
        {
            return true;
        }

        bool bIsInfront = false;

        Vector3 vPlayerPosition = _player.transform.position;
        Vector3 vFront = transform.forward;
        Vector3 vInteractToPlayer = vPlayerPosition - transform.position;

        bIsInfront = Vector3.Dot(vInteractToPlayer, vFront) >= 0.0f;
        

        return bIsInfront;
        
    }

    public bool HasAllConditionToInteract(Player _player)
    {
        bool bHasAllNecessaryItems = HasAllItemQuantity(_player);

        return bHasAllNecessaryItems;
    }

    public bool HasAllItemQuantity(Player _player)
    {
        if (InventoryToCheck == null)
            return true;

        foreach(ItemRequirement item in ItemsRequirement)
        {
            int iQuantity = 0;
            bool bHasObject = InventoryToCheck.TryGetItemQuantity(item.ItemName, ref iQuantity);
            if (bHasObject)
            {
                if (iQuantity < item.ItemQuantity)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }
        return true;
    }
}
