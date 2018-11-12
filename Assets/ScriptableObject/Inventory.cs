using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{

    public List<string> InitialisedItemName;
    public List<int> InitialisedItemValue;
    public int ItemCounter;

    [NonSerialized]
    private List<string> runTimeNames;

    [NonSerialized]
    private List<int> runTimeValue;

    [NonSerialized]
    private int runTimeItemCounter;


    public void OnAfterDeserialize()
    {
        runTimeNames = new List<string>(InitialisedItemName);

        runTimeValue = new List<int>(InitialisedItemValue);

        ItemCounter = runTimeItemCounter;
    }

    public void OnBeforeSerialize()
    {

    }

    public void AddItemQuantity(string _sItemName, int _iQuantity)
    {
        int Index = runTimeNames.IndexOf(_sItemName);
        if (Index>=0)
        {
            runTimeValue[Index] += _iQuantity;
        }
        else
        {
            runTimeItemCounter++;
            runTimeNames.Add(_sItemName);
            runTimeValue.Add(_iQuantity);
        }
    }

    public bool TryGetItemQuantity(string _sItemName, ref int _iRetQuantity)
    {
        int Index = runTimeNames.IndexOf(_sItemName);
        if (Index >= 0)
        {
            _iRetQuantity  = runTimeValue[Index];
            return true;
        }
        else
        {
            return false;
        }
    }
}
