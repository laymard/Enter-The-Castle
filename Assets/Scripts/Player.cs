using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameInterface;

[System.Serializable]
public class InteractibleEvent : UnityEvent<Interactible>
{

}

public class Player : MonoBehaviour, IPausable {

    public float InteractionRange;
    public Inventory PlayerInventory;
    public AimCursor aimcursor;

    private Dictionary<string, int> Inventory;
    private Camera PlayerCamera;
    private Loot CurrentAimedLoot;

    public float InteractionCoolDown;
    private float m_fInteractionTimer;

    private Interactible m_InteractibleAimed;
    public InteractibleEvent OnCurrentInteractibleChanged ;

    private WeaponController m_WeaponController;

    public Capacity MeleeAttackCapacity;

    private bool m_bOnPause = false;

	// Use this for initialization
	void Start () {
        PlayerCamera = Camera.main;
        Inventory = new Dictionary<string, int>();
        m_fInteractionTimer = -1;

        m_WeaponController = GetComponentInChildren<WeaponController>();
    }
	
	// Update is called once per frame
	void Update () {
        MeleeAttackCapacity.UpdateCurrentCompletion(Time.deltaTime);
        bool bIsHittingLoot = false;

        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, InteractionRange))
        {
            Loot lootHit = hit.transform.GetComponent<Loot>();
            Interactible interactible = hit.transform.GetComponent<Interactible>();
            if (lootHit != null)
            {
                CurrentAimedLoot = lootHit;
                bIsHittingLoot = true;
            }


            SetCurrentInteractible(interactible);
            

        }
        else
        {
            CurrentAimedLoot = null;
            SetCurrentInteractible(null);
        }

        aimcursor.ChangeCursorStatus(bIsHittingLoot);
        if(m_InteractibleAimed != null && !bIsHittingLoot)
        {
            aimcursor.ChangeCursorStatus(m_InteractibleAimed.CanBeTriggeredByPlayer(this));
        }

        if(m_fInteractionTimer<0)
        {
            if(!m_bOnPause)
            {
                if (Input.GetAxis("Interact")>0.8f && CurrentAimedLoot !=null && CurrentAimedLoot.isActiveAndEnabled)
                {
                    LootObject(CurrentAimedLoot);
                    m_fInteractionTimer = InteractionCoolDown;
                }


                if (Input.GetAxis("Interact") > 0.8f && m_InteractibleAimed != null && m_InteractibleAimed.CanBeTriggeredByPlayer(this))
                {
                    m_InteractibleAimed.LaunchInteraction(this);
                    m_fInteractionTimer = InteractionCoolDown;

                }
            }


        }
        else
        {
            m_fInteractionTimer -= Time.deltaTime;
        }

        if(!m_bOnPause)
        {
            if(Input.GetAxis("MeleeAttack") > 0.9f && MeleeAttackCapacity.CanTriggerCapacity())
            {
                m_WeaponController.StartMeleeAttack();
                MeleeAttackCapacity.TriggerCapacity();
            }
        }
    }

    public void LootObject(Loot _LootObject)
    {
        PlayerInventory.AddItemQuantity(_LootObject.GetLootName(), _LootObject.GetLootQuantity());
        _LootObject.OnLootedByPlayer(this);

    }

    private void SetCurrentInteractible(Interactible _interactible)
    {
        if(m_InteractibleAimed != _interactible)
        {
            m_InteractibleAimed = _interactible;
            OnCurrentInteractibleChanged.Invoke(m_InteractibleAimed);
        }

        
    }

    public void OnPause()
    {
        m_bOnPause = true;
    }

    public void OnResume()
    {
        m_bOnPause = false;
    }
}
