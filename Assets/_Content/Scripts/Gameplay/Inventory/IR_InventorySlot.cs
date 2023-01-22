using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IR_InventorySlot : MonoBehaviour
{
    public int slotIndex;
    
    [Tooltip("If item is an item, store information here.")]
    public IR_Item item;

    [Header("Slot Information")]
    public bool isEmpty; //Is this slot empty? yes or no.
    public bool isStackable; //Is this item stackable? yes or no.
    public int amount; //Amount of this item in the stack.

    [Header("Weapon Stat Information")]
    public float weaponHealth;
    public float weaponDMG;

    [Header("Slot UI Elements")]
    public Image itemIcon;
    public TextMeshProUGUI itemAmountText;

    [Header("Chest Information")]
    public LootableChest LC;

    public void Start() 
    {
        
        
    }

    public void Update() 
    {
        if (item == null) 
        {
            isEmpty = true;
        } else {
            isEmpty = false;
        }


      
    }

}
