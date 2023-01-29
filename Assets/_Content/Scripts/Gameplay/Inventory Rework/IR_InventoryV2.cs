using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

namespace Continuity.Inventory {

public class IR_InventoryV2 : MonoBehaviour
{
    public static IR_InventoryV2 instance;

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
        instance = this;
        CreateInventoryItemList();
    }

    public void Update() 
    {
        UpdateInventorySlots();
    }
    public void MakeNewStack(IR_Item item, int ItemAmount, int weaponHealth) 
    {
        for (int j = 0; j < InventoryItems.Count; j++) 
        {
            if (InventoryItems[j].item == null && InventoryItems[j].stackAmount <= 0) 
            {
                InventoryItems[j].item = item;
                InventoryItems[j].stackAmount += ItemAmount;
                InventoryItems[j].weaponHealth = weaponHealth;
                break;
            }
        }
    }

    public void AddItem(IR_Item item, int ItemAmount, int weaponHealth)
    {
        List<InventoryItem> InvItems = new List<InventoryItem>();

        for (int i = 0; i < InventoryItems.Count; i++) 
        {
            if (!item.isStackable) 
            {
                if (InventoryItems[i].item == null) 
                {
                    InventoryItems[i].item = item;
                    InventoryItems[i].stackAmount = ItemAmount;
                    InventoryItems[i].weaponHealth = weaponHealth;

                    break;
                }
            }
            if (item.isStackable) 
            {
                if (CheckForItem(item)) 
                {
                    if (InventoryItems[i].item == item) 
                    {
                        InvItems.Add(InventoryItems[i]);
                    }
                } else if (!CheckForItem(item)) 
                {
                    if (InventoryItems[i].item == null && InventoryItems[i].stackAmount <= 0) 
                    {
                        InventoryItems[i].item = item;
                        InventoryItems[i].stackAmount = ItemAmount;
                        InventoryItems[i].weaponHealth = weaponHealth;
                        Debug.Log("FIRST PICK UP");
                        break;
                    }
                }
            }
        }
        //InvItems.Sort();
        int AmountOver = 0;
        int count = 0;
        bool allItemsFull = false;

        for (int i = 0; i < InvItems.Count; i++) 
        {
            if (InvItems[i].stackAmount == item.stackableLimit) 
            {
                count++;
            }
            if (count >= InvItems.Count) 
            {
                allItemsFull = true;
                Debug.Log("All Items Are Full!");
                break;
            }
        }

        if (allItemsFull) 
        {
            for (int j = 0; j < InventoryItems.Count; j++) 
            {
                if (InventoryItems[j].item == null && InventoryItems[j].stackAmount <= 0) 
                {
                    InventoryItems[j].item = item;
                    InventoryItems[j].stackAmount += ItemAmount;
                    InventoryItems[j].weaponHealth = weaponHealth;
                    InvItems.Add(InventoryItems[j]);

                    break;
                }
            }
        } else if (!allItemsFull) 
        {
            for (int i = 0; i < InvItems.Count; i++) 
            {
                if (InvItems[i].stackAmount < item.stackableLimit) 
                {
                    if (InvItems[i].stackAmount + ItemAmount > item.stackableLimit) 
                    {
                        InvItems[i].stackAmount += ItemAmount;
                        AmountOver = InvItems[i].stackAmount - item.stackableLimit;
                        InvItems[i].stackAmount = item.stackableLimit;
                        MakeNewStack(item, AmountOver, weaponHealth);
                        break;
                    } else if (InvItems[i].stackAmount + ItemAmount <= item.stackableLimit) 
                    {
                        InvItems[i].stackAmount += ItemAmount;
                        break;
                    }
                }
            }

        /* for (int i = 0; i < InvItems.Count; i++) 
        {
            if (InvItems[i].stackAmount < item.stackableLimit) 
            {
                if (InvItems[i].stackAmount + ItemAmount > item.stackableLimit) 
                {
                    InvItems[i].stackAmount += ItemAmount;
                    AmountOver = InvItems[i].stackAmount - item.stackableLimit;
                    InvItems[i].stackAmount = item.stackableLimit;
                    MakeNewStack(item, AmountOver, weaponHealth);
                    break;
                } else if (InvItems[i].stackAmount + ItemAmount <= item.stackableLimit) 
                {
                    InvItems[i].stackAmount += ItemAmount;
                    break;
                }
            }

            if (InvItems != null && InvItems[0].stackAmount == item.stackableLimit) 
            {
                for (int j = 0; j < InventoryItems.Count; j++) 
                {
                    if (InventoryItems[j].item == null && InventoryItems[j].stackAmount <= 0) 
                    {
                        InventoryItems[j].item = item;
                        InventoryItems[j].stackAmount += ItemAmount;
                        InventoryItems[j].weaponHealth = weaponHealth;
                        InvItems.Add(InventoryItems[j]);

                        break;
                    }
                }
                break;
            } 
        } */

       

        for (int i = 0; i < InvItems.Count; i++) 
        {
            print("INDEX: " + InvItems[i].Index + "ITEM: " + InvItems[i].item + " AMOUNT: " + InvItems[i].stackAmount);
        }   
    }
    }

#region Inventory Interactions
    public void ClickItem(IR_InventorySlot slot) 
    {
        if (slot.item == null) 
        {
            return;
        }

        if (slot.item.isWeapon) 
        {
            for (int i = 0; i < WeaponManager.instance.WeaponSlots.Count; i++) 
            {
                if (WeaponManager.instance.WeaponSlots[i].item == null) 
                {
                    WeaponManager.instance.WeaponSlots[i].item = slot.item;
                    WeaponManager.instance.WeaponSlots[i].weaponHealth = slot.weaponHealth;
                    WeaponManager.instance.CurrentWeapons[i] = slot.item.weapon;

                    InventoryItems[slot.slotIndex].item = null;
                    InventoryItems[slot.slotIndex].stackAmount = 0;
                    break;
                }
            }
        } else {
            slot.item.Use();
            InventoryItems[slot.slotIndex].stackAmount--;

            if (InventoryItems[slot.slotIndex].stackAmount < 1) 
            {
                InventoryItems[slot.slotIndex].item = null;
            }

            if (InventoryItems[slot.slotIndex].stackAmount < 0) 
            {
                InventoryItems[slot.slotIndex].stackAmount = 0;
            }
        }
    }
    public void DropItem(IR_InventorySlot slot) 
    {
         //Drop an item from the inventory back into the game world.
        GameObject DropPoint = GameObject.Find("ItemDropPoint");
        
        GameObject itemDropped = Instantiate(slot.item.worldObject, DropPoint.transform.position, Quaternion.identity);
        itemDropped.GetComponent<ItemInWorld>().WorldWeaponHealth = (int)slot.weaponHealth; //Sets weaponhealth of weapon dropped
        itemDropped.transform.SetParent(GameObject.Find("ItemsInWorld").transform); //Sets dropped item parent to an item holder in the world. Hierarchy CleanUp

        itemDropped.GetComponent<ItemInWorld>().amount = 1;

        InventoryItems[slot.slotIndex].stackAmount--;

            if (InventoryItems[slot.slotIndex].stackAmount < 1) 
            {
                InventoryItems[slot.slotIndex].item = null;
            }

            if (InventoryItems[slot.slotIndex].stackAmount < 0) 
            {
                InventoryItems[slot.slotIndex].stackAmount = 0;
            }
    
    }
    public void DropStack(IR_InventorySlot slot) 
    {
        GameObject DropPoint = GameObject.Find("ItemDropPoint");
        GameObject itemDropped;

        itemDropped = Instantiate(slot.item.worldObject, DropPoint.transform.position, Quaternion.identity);
        itemDropped.GetComponent<ItemInWorld>().WorldWeaponHealth = (int)slot.weaponHealth; //Sets weaponhealth of weapon dropped
        itemDropped.transform.SetParent(GameObject.Find("ItemsInWorld").transform); //Sets dropped item parent to an item holder in the world. Hierarchy CleanUp

        itemDropped.GetComponent<ItemInWorld>().amount = slot.amount;

         InventoryItems[slot.slotIndex].stackAmount = 0;

            if (InventoryItems[slot.slotIndex].stackAmount < 1) 
            {
                InventoryItems[slot.slotIndex].item = null;
            }

            if (InventoryItems[slot.slotIndex].stackAmount < 0) 
            {
                InventoryItems[slot.slotIndex].stackAmount = 0;
            }
    }
    public void ReturnWeaponToInventory(IR_InventorySlot slot) 
    {
        AddItem(slot.item, slot.amount, (int)slot.weaponHealth);
        slot.item = null;
        slot.weaponHealth = 0;
        slot.amount = 0;
    }
    public void AddItemFromChest(IR_InventorySlot slot) 
    {
        if (slot.item != null) 
        {
            AddItem(slot.item, slot.amount, (int)slot.weaponHealth);

            slot.LC.ItemsInChest[slot.slotIndex].item = null;
            slot.LC.ItemsInChest[slot.slotIndex].itemAmount = 0;
            slot.LC.ItemsInChest[slot.slotIndex].icon = null;
            slot.item = null;
            slot.weaponHealth = 0;
            slot.amount = 0;

            slot.itemIcon.gameObject.SetActive(false);
            slot.itemAmountText.gameObject.SetActive(false);
        } else {

        }
    }   
#endregion
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

    public void GetItem(string ItemName) 
    {


    }
    public void UpdateInventorySlots() 
    {
        for (int i = 0; i < InventorySlots.Count; i++) 
        {
            InventorySlots[i].item = InventoryItems[i].item;
            InventorySlots[i].amount = InventoryItems[i].stackAmount;
            
            //UI Information
            if (InventorySlots[i].item != null) 
            {
                InventorySlots[i].itemIcon.gameObject.SetActive(true);
                if (InventorySlots[i].item.isStackable) 
                {
                    InventorySlots[i].itemAmountText.gameObject.SetActive(true);
                }
                

                InventorySlots[i].itemIcon.sprite = InventoryItems[i].item.itemIcon;
                InventorySlots[i].itemAmountText.text = InventoryItems[i].stackAmount.ToString();
            } else {
                InventorySlots[i].itemIcon.gameObject.SetActive(false);
                InventorySlots[i].itemAmountText.gameObject.SetActive(false);
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
    bool CheckForItem(IR_Item item)
    {
        foreach(InventoryItem i in InventoryItems) 
        {
            if(i.item == item) 
            {
                return true;
            }
        }
        return false;
    }

}

[System.Serializable]
public class InventoryItem : IComparable<InventoryItem>
{
    public int Index;
    public IR_Item item;
    public int stackAmount;
    public int weaponHealth;

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
