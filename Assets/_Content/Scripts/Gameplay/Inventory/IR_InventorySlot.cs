using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IR_InventorySlot : MonoBehaviour
{
    [Tooltip("If Item is a weapon, store information here.")]
    public Weapon weapon;
    [Tooltip("If item is an item, store information here.")]
    public IR_Item item;

    [Header("Slot Information")]

    public bool isStackable; //Is this item stackable? yes or no.
    public int amount; //Amount of this item in the stack.

    [Header("Weapon Stat Information")]
    public float weaponHealth;
    public float weaponDMG;

}
