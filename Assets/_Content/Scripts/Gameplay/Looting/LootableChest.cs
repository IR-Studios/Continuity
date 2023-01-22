using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestType { Medical, Food, Material, Weapon }
public class LootableChest : MonoBehaviour
{
    [InspectorName("Type Of Chest")]
    public ChestType TypeOfChest;


    public List<ChestItems> ItemsInChest = new List<ChestItems>();
    [HideInInspector]
    public List<IR_InventorySlot> ChestInventorySlots = new List<IR_InventorySlot>();

    public bool ChestFilled = false;
    public bool ChestOpened = false;
    public bool isSearching = false;
    public float secondsToSearch;

    [Header("UI Elements")]
    public GameObject container;
    public GameObject ChestUI;

    public void Start() 
    {
        getChestSlots();
        ChestUI.gameObject.SetActive(false);
        
    }

    public void Update() 
    {
        if (HUDManager.instance.ChestOpen) 
        {

        } else if (!HUDManager.instance.ChestOpen) 
        {
            ClearDisplay();
        }
    }   

    public void ClearDisplay() 
    {
        for (int i = 0; i < ChestInventorySlots.Count; i++) 
        {
            if (ChestInventorySlots[i].item != null) 
            {
                if (ChestInventorySlots[i].item.isStackable) 
            {
                ChestInventorySlots[i].itemAmountText.gameObject.SetActive(false);
            }

                ChestInventorySlots[i].item = null;
                ChestInventorySlots[i].amount = 0;
                ChestInventorySlots[i].itemIcon.sprite = null;
                ChestInventorySlots[i].itemIcon.gameObject.SetActive(false);
                ChestInventorySlots[i].LC = null;
            } else {

            }
        }
    }

    public void DisplayChest() 
    {
        for (int i = 0; i < ItemsInChest.Count; i++) 
        {
            ChestInventorySlots[i].LC = this;

            ChestInventorySlots[i].item = ItemsInChest[i].item;
            if (ChestInventorySlots[i].LC.ItemsInChest[i].item == null) 
            {
                continue;
            }
            ChestInventorySlots[i].amount =  ItemsInChest[i].itemAmount;

            ChestInventorySlots[i].itemIcon.sprite = ItemsInChest[i].icon;
            ChestInventorySlots[i].itemIcon.gameObject.SetActive(true);

            if (ChestInventorySlots[i].item.isStackable) 
            {
                ChestInventorySlots[i].itemAmountText.gameObject.SetActive(true);
                ChestInventorySlots[i].itemAmountText.text = ChestInventorySlots[i].amount.ToString();
            }
        }
    }

    public void FillChest() 
    {
        int randomNum = (int)Random.Range(1, 8);
        for (int i = 0; i < randomNum; i++) 
        {
            int random = Random.Range(0, AllLootableItems.instance.items.Count);

            ItemsInChest.Add(new ChestItems());
            ItemsInChest[i].item = AllLootableItems.instance.items[random];
            ItemsInChest[i].itemAmount = Random.Range(1, AllLootableItems.instance.items[random].stackableLimit);
            ItemsInChest[i].icon = AllLootableItems.instance.items[random].itemIcon;

            DisplayChest();
            ChestFilled = true;
        }
    }

    public IEnumerator SearchChest() 
    {
        isSearching = true;
        yield return new WaitForSeconds(secondsToSearch);
        FillChest();

        HUDManager.instance.openInventory();
        HUDManager.instance.OpenChestInventory();
        isSearching = false;
    }

    public void getChestSlots() 
    {
        //Gets all the slots and adds them into the List.
        foreach (Transform children in container.transform) 
        {
            if (children.tag == "ItemBox") 
            {
                ChestInventorySlots.Add(children.GetComponent<IR_InventorySlot>());
                children.GetComponent<IR_InventorySlot>().slotIndex = ChestInventorySlots.Count - 1;
            }
        }
    }
}

[System.Serializable]
public class ChestItems
{
    public IR_Item item;
    public int itemAmount;
    public Sprite icon;
}
