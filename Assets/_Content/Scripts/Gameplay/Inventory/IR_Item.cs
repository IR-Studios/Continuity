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
    public bool isArrowAmmo;
    public bool isBoltAmmo;
    public bool isShotgunAmmo;

    [Header("Spawn Variables")]
    public float RotX;
    public float RotY;
    public float RotZ;
    

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
