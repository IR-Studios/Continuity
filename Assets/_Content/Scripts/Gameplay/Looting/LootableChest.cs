using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableChest : MonoBehaviour
{

    public List<ChestItems> ItemsInChest = new List<ChestItems>();
    [HideInInspector]
    public List<IR_InventorySlot> ChestInventorySlots = new List<IR_InventorySlot>();

    public bool ChestFilled = false;

    [Header("UI Elements")]
    public GameObject container;
    public GameObject ChestUI;

    public void Start() 
    {
        getChestSlots();
        ChestUI.gameObject.SetActive(false);
        
    }

    public void DisplayChest() 
    {
        for (int i = 0; i < ItemsInChest.Count; i++) 
        {
            ChestInventorySlots[i].item = ItemsInChest[i].item;
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
            /*ChestInventorySlots[i].item = ItemsInChest[i].item;
            ChestInventorySlots[i].amount =  ItemsInChest[i].itemAmount;

            ChestInventorySlots[i].itemIcon.sprite = ItemsInChest[i].icon;
            ChestInventorySlots[i].itemIcon.gameObject.SetActive(true);

            if (ChestInventorySlots[i].item.isStackable) 
            {
                ChestInventorySlots[i].itemAmountText.gameObject.SetActive(true);
                ChestInventorySlots[i].itemAmountText.text = ChestInventorySlots[i].amount.ToString();
            }*/

            ChestFilled = true;
        }
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
