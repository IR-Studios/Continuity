using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfItem { Medical, Food, Material, Weapon }
public class IR_Item : ScriptableObject
{
    [Header("Item Index Information")]
    public int itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public typeOfItem item;
    public Sprite itemIcon;

    [Header("Item Stat Information")]
    public int stackableLimit;
    public int itemHealth;
    public bool isStackable;
    public GameObject worldObject;

    [Header("Weapon Information")]
    public Item_Weapon weapon;
    public bool isWeapon;
    

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
