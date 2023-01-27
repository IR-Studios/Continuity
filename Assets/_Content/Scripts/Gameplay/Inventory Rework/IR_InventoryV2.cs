using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

namespace Continuity.Inventory {

public class IR_InventoryV2 : MonoBehaviour
{
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public List<IR_InventorySlot> InventorySlots = new List<IR_InventorySlot>();
    [SerializeField]
    private GameObject backpackObj;

    public void Awake() 
    {
        getInventorySlots();
    }
    public void Start() 
    {
        CreateInventoryItemList();
    }

    public void Update() 
    {
        UpdateInventorySlots();
        if (Rebind.GetInputDown("Interact")) 
        {
            SubtractAmmoFromStack("arrow");
        }
        if (Rebind.GetInputDown("Reload")) 
        {
            SubtractAmmoFromStack("Assault");
        }
    }

    public void AddItem(IR_Item item, int ItemAmount, int weaponHealth) 
    {
        for (int i = 0; i < InventoryItems.Count; i++) 
        {
            if (!item.isStackable) 
            {
                if (InventoryItems[i].item == null) 
                {

                }
            }

            if (item.isStackable) 
            {

            }
        }
    }

#region Ammunition Inventory
    public int GetAmmo(string AmmoType) 
    {
        int ammo = 0;
        if (AmmoType == "arrow" || AmmoType == "Arrow") 
        {
            foreach(InventoryItem item in InventoryItems) 
            {
                if (item.item != null && item.item.itemID == 0) 
                {
                    ammo += item.stackAmount;
                }
            }
            print(ammo);
            return ammo;
        }

        return 1;
    }

    public void SubtractAmmoFromStack(string AmmoType) 
    {
        List<InventoryItem> slots = new List<InventoryItem>();

        #region Arrow Ammo
        if (AmmoType == "arrow" || AmmoType == "Arrow") 
        {
           foreach (InventoryItem i in InventoryItems) 
           {
                if (i.item != null && i.item.itemID == 0 && i.stackAmount > 0) 
                {
                    slots.Add(i);
                }
           }
           slots.Sort();

           slots[0].stackAmount--;
           if (slots[0].stackAmount <= 0) 
           {
                slots[0].stackAmount = 0;
                InventoryItems[slots[0].Index].item = null;
                slots.Remove(slots[0]);
           }
        }
        #endregion
        #region Assault Ammo
        if (AmmoType == "Assault" || AmmoType == "Assault") 
        {
            foreach (InventoryItem i in InventoryItems) 
            {
                if (i.item != null && i.item.itemID == 8 && i.stackAmount > 0) 
                {
                    slots.Add(i);
                }
            }
            slots.Sort();

            slots[0].stackAmount--;
            if (slots[0].stackAmount <= 0) 
            {
                slots[0].stackAmount = 0;
                InventoryItems[slots[0].Index].item = null;
                slots.Remove(slots[0]);
            }
        }
        #endregion
    }
#endregion

    public void UpdateInventorySlots() 
    {
        for (int i = 0; i < InventorySlots.Count; i++) 
        {
            InventorySlots[i].item = InventoryItems[i].item;
            InventorySlots[i].amount = InventoryItems[i].stackAmount;
            
            //UI Information
            if (InventorySlots[i].item != null) 
            {
                InventorySlots[i].itemIcon.sprite = InventoryItems[i].item.itemIcon;
                InventorySlots[i].itemAmountText.text = InventoryItems[i].stackAmount.ToString();
            }
        }
    }

    public void CreateInventoryItemList() 
    {  
        int i = 0;
        foreach(IR_InventorySlot slot in InventorySlots) 
        {
            InventoryItems.Add(new InventoryItem {Index = i });
            i++;
        }
    }

    public void getInventorySlots() 
    {
        //Gets all the slots and adds them into the List.
        foreach (Transform children in backpackObj.transform) 
        {
            if (children.tag == "ItemBox") 
            {
                InventorySlots.Add(children.GetComponent<IR_InventorySlot>());
                children.GetComponent<IR_InventorySlot>().slotIndex = InventorySlots.Count - 1;
            }
        }
    }
}

[System.Serializable]
public class InventoryItem : IComparable<InventoryItem>
{
    public int Index;
    public IR_Item item;
    public int stackAmount;

    public int CompareTo(InventoryItem item) 
    {
        if (item == null) 
        {
            return 1;
        } 
        else 
        {
            return this.stackAmount.CompareTo(item.stackAmount);
        }
    }
}
}
