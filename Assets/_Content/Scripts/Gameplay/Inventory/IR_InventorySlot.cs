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
    public Image slotBorder;
    public TextMeshProUGUI itemAmountText;
    Vector2 IconSize;
    Vector2 BorderSize;

    [Header("Chest Information")]
    public LootableChest LC;

    void Awake() 
    {
        slotBorder = this.gameObject.GetComponent<Image>();
    }

    public void Start() 
    {


        IconSize = itemIcon.rectTransform.sizeDelta;
        BorderSize = slotBorder.rectTransform.sizeDelta;
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

    public void SlotHoverEnter(IR_InventorySlot slot) 
    {
        slot = this;
        slot.slotBorder.rectTransform.sizeDelta = new Vector2(BorderSize.x + 10 , BorderSize.y + 10);
        slot.itemIcon.rectTransform.sizeDelta = new Vector2(IconSize.x + 10 ,IconSize.y + 10);
        if (slot.item != null) 
        {
            DisplayInformation(true);
        }
       
    }
    public void SlotHoverExit(IR_InventorySlot slot) 
    {
        slot = this;
        slot.slotBorder.rectTransform.sizeDelta = new Vector2(BorderSize.x, BorderSize.y); 
        slot.itemIcon.rectTransform.sizeDelta = new Vector2(IconSize.x,IconSize.y);
        DisplayInformation(false);
    }

    public void DisplayInformation(bool isDisplaying) 
    {
        if (isDisplaying) 
        {
            HUDManager.instance.ItemInfo(true);
            HUDManager.instance.itemInfoName.text = item.itemName;
            HUDManager.instance.itemInfoIcon.sprite = item.itemIcon;
            HUDManager.instance.itemInfoDescription.text = item.itemDescription;
        } else if (!isDisplaying) 
        {
            HUDManager.instance.ItemInfo(false);
        }
    }

}
