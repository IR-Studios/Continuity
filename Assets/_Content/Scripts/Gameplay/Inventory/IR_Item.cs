using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum typeOfItem { Medical, Food, Material, Weapon }
public class IR_Item : ScriptableObject
{
    public typeOfItem item;

    public string shortName;
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;
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
