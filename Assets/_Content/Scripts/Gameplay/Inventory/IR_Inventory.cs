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
        //getItems();
        
    }

    public void Update() 
    {
        UpdateSlotInfo();
    }

    //Adds an Item to the Inventory via Inventory Slots.
    public void AddItem(IR_Item item, int weaponHealth) 
    {
        int count = 0;
        for (int i = 0; i < InventorySlots.Count; i++) 
        {
            if (InventorySlots[i].item == null && count == 0 && !item.isStackable) 
            {
                InventorySlots[i].item = item;  
                InventorySlots[i].weaponHealth = weaponHealth; //For Weapons Only
                Debug.Log("Item has been picked up! " + item.name);
                count = 1;
            } 
            else if (InventorySlots[i].item == null && count == 0 && item.isStackable && InventorySlots[i].amount == 0) 
            {
                InventorySlots[i].item = item;  
                InventorySlots[i].amount++;
                Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                count = 1;
                
            }
            else if (InventorySlots[i].item == item && count == 0 && item.isStackable && InventorySlots[i].amount < item.stackableLimit) 
            { 
                InventorySlots[i].amount++;
                Debug.Log(item.name + " has been acquired. Amount: " + InventorySlots[i].amount);
                count = 1;
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
        slot.amount--;
        if (slot.amount < 1) 
        {
            slot.item = null;
            slot.itemIcon.gameObject.SetActive(false);
            slot.itemAmountText.gameObject.SetActive(false);
        }

        if (slot.amount < 0) 
        {
            slot.amount = 0;
        }
    }

    public void UpdateItemList() 
    {
        
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
