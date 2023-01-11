using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IR_Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;
    public int itemAmount;
    public bool isStackable;

    public GameObject worldObject;

    public virtual void Use() 
    {

    }

    public virtual void Equip() 
    {

    }
    public virtual void Drop() 
    {

    }
}
