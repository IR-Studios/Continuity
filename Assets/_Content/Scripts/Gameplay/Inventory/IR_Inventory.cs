using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IR_Inventory : MonoBehaviour
{
    public static IR_Inventory instance;
    [Header("Item Manager")]
    public List<IR_Item> items = new List<IR_Item>();
    [Header("Inventory Slot Manager")]
    public List<IR_InventorySlot> InventorySlots = new List<IR_InventorySlot>();
    public int maxInventorySlots;
    [SerializeField]
    private GameObject backpackObj;
    public void Awake () 
    {
        instance = this;
    }
    public void Start() 
    {
        //backpackObj = GameObject.Find("Backpack");
        InventorySlots.Capacity = maxInventorySlots;
        getInventorySlots();
        getItems();
        
    }

    public void Update() 
    {
        UpdateSlotInfo();
    }

    bool CheckForItem(IR_Item item) 
    {
        for (int i = 0; i < InventorySlots.Count; i++) 
        {
            if (InventorySlots[i].item == item) {
                return true;
            } 
        } 
        return false;
    }

    //Adds an Item to the Inventory via Inventory Slots.
    public void AddItem(IR_Item item, int weaponHealth, int itemAmount) 
    {
        int count = 0;
        for (int i = 0; i < InventorySlots.Count; i++) 
        {
            if (InventorySlots[i].item == null && count == 0 && !item.isStackable) 
            {
                InventorySlots[i].item = item;  
                InventorySlots[i].weaponHealth = weaponHealth; //For Weapons Only
                InventorySlots[i].amount = itemAmount;

                items[i] = item;

                Debug.Log("Item has been picked up! " + item.name);
                count = 1;
                break;
            } 
        #region old Code
           /* else if (InventorySlots[i].item == null && count == 0 && item.isStackable && InventorySlots[i].amount == 0 & !CheckForItem(item)) 
            {
                InventorySlots[i].item = item;  
                InventorySlots[i].amount += itemAmount;
                items[i] = item;
                Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                count = 1;
                
            }
            else if (InventorySlots[i].item == item && count == 0 && item.isStackable && InventorySlots[i].amount < item.stackableLimit) 
            { 
                //InventorySlots[i].amount++;
                Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                count = 1;
            } */
            #endregion

            if (item.isStackable) 
            {
                if (CheckForItem(item)) 
                {
                    if (InventorySlots[i].item == item && InventorySlots[i].amount < item.stackableLimit) 
                    {
                        int amountOver;
                        if ((InventorySlots[i].amount + itemAmount > item.stackableLimit)) 
                        {
                            InventorySlots[i].amount += itemAmount;
                            amountOver = InventorySlots[i].amount - item.stackableLimit;
                            InventorySlots[i].amount = item.stackableLimit;
                            AddItem(item, weaponHealth, amountOver);
                            break;
                        } else if ((InventorySlots[i].amount + itemAmount <= item.stackableLimit)) 
                        {
                            InventorySlots[i].amount += itemAmount;
                            Debug.Log(itemAmount);
                            break;
                        }   
                    } else if (InventorySlots[i].item == null && InventorySlots[i].amount == 0) 
                    {
                        InventorySlots[i].item = item;  
                        InventorySlots[i].amount += itemAmount;
                        Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                        break;
                    }

                    if (InventorySlots[i].item == item && InventorySlots[i].amount == item.stackableLimit) 
                    {
                        continue;
                    }
                } else if (!CheckForItem(item)) 
                {
                    if (InventorySlots[i].item == null && count == 0 && InventorySlots[i].amount == 0) 
                    {
                        InventorySlots[i].item = item;  
                        InventorySlots[i].amount += itemAmount;
                        Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                        count = 1;
                        break;
                    }
                }
            }
        }
    }

    public void ClickItem(IR_InventorySlot slot) 
    {
        if (slot.item.isWeapon) 
        {
            int count = 0;
            for (int i = 0; i < WeaponManager.instance.WeaponSlots.Count; i++) 
            {
                if (WeaponManager.instance.WeaponSlots[i].item == null && count == 0) 
                {
                    WeaponManager.instance.WeaponSlots[i].item = slot.item;
                    WeaponManager.instance.WeaponSlots[i].weaponHealth = slot.weaponHealth;
                    WeaponManager.instance.CurrentWeapons[i] = slot.item.weapon;
                    slot.item = null;
                    slot.amount = 0;
                    slot.itemIcon.gameObject.SetActive(false);
                    slot.itemAmountText.gameObject.SetActive(false);

                    
                    count = 1;
                }
            }
        } else 
        {
            //DropItem(slot);
            slot.item.Use();
            slot.amount--;

            if (slot.amount < 1) 
            {      
                items[slot.slotIndex] = null;

                slot.item = null;
                slot.itemIcon.gameObject.SetActive(false);
                slot.itemAmountText.gameObject.SetActive(false);
            }

            if (slot.amount < 0) 
            {
                slot.amount = 0;
            }
        }
    }
    //Spawns Item prefab into the game world and removes item from the slot it was in.
    public void DropItem(IR_InventorySlot slot) 
    {
        //Drop an item from the inventory back into the game world.
        GameObject DropPoint = GameObject.Find("ItemDropPoint");
        
        GameObject itemDropped = Instantiate(slot.item.worldObject, DropPoint.transform.position, Quaternion.identity);
        itemDropped.GetComponent<ItemInWorld>().WorldWeaponHealth = (int)slot.weaponHealth; //Sets weaponhealth of weapon dropped
        itemDropped.transform.SetParent(GameObject.Find("ItemsInWorld").transform); //Sets dropped item parent to an item holder in the world. Hierarchy CleanUp

        itemDropped.GetComponent<ItemInWorld>().amount = 1;
        slot.amount--;
        if (slot.amount < 1) 
        {
            items[slot.slotIndex] = null;

            slot.item = null;
            slot.itemIcon.gameObject.SetActive(false);
            slot.itemAmountText.gameObject.SetActive(false);
        }

        if (slot.amount < 0) 
        {
            slot.amount = 0;
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

        slot.amount = 0;

         if (slot.amount < 1) 
        {
            items[slot.slotIndex] = null;

            slot.item = null;
            slot.itemIcon.gameObject.SetActive(false);
            slot.itemAmountText.gameObject.SetActive(false);
        }

        if (slot.amount < 0) 
        {
            slot.amount = 0;
        }

    } 

    public void ReturnWeaponToInventory(IR_InventorySlot slot) 
    {
        AddItem(slot.item, (int)slot.weaponHealth, slot.amount);
        slot.item = null;
        slot.weaponHealth = 0;
        slot.amount = 0;
    }

    public void AddItemFromChest(IR_InventorySlot slot) 
    {
        if (slot.item != null) 
        {
             AddItem(slot.item, (int)slot.weaponHealth, slot.amount);

            slot.LC.ItemsInChest[slot.slotIndex].item = null;
            slot.LC.ItemsInChest[slot.slotIndex].itemAmount = 0;
            slot.LC.ItemsInChest[slot.slotIndex].icon = null;

            slot.item = null;
            slot.weaponHealth = 0;
            slot.amount = 0;

            slot.itemIcon.gameObject.SetActive(false);
            slot.itemAmountText.gameObject.SetActive(false);
        } else 
        {
            //do nothing
        }
        
       

    }

    public void UpdateSlotInfo() 
    {
        foreach (IR_InventorySlot slot in InventorySlots) 
        {
            if (slot.item != null) //Checks if an item is in the slot. 
            {
                slot.itemIcon.sprite = slot.item.itemIcon;
                slot.itemIcon.gameObject.SetActive(true); //Sets image gameobject to active.
                if (slot.amount > 0) //Checks if this item is stackable and has an amount more than 0 in order to activate amount text.
                {
                    slot.itemAmountText.gameObject.SetActive(true);
                    slot.itemAmountText.text = slot.amount.ToString();
                }

                if (!slot.item.isStackable) 
                {
                    slot.itemAmountText.gameObject.SetActive(false);
                }
            } else if (slot.item == null) //Checks if there is no item in the slot and disables slot information.
            {
                slot.itemIcon.gameObject.SetActive(false);
                slot.itemAmountText.gameObject.SetActive(false);
            }
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

    public void getItems() 
    {
        foreach(IR_InventorySlot Slot in InventorySlots) 
        {
            items.Add(Slot.item);
        }
    }

    
}
