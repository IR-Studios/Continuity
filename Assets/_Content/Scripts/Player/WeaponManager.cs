using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<Item_Weapon> CurrentWeapons = new List<Item_Weapon>();
    public Item_Weapon activeWeapon;
    public GameObject activeWeaponObj;
    public GameObject CamHolder;
    [Header("Weapon Inventory")]
    public List<IR_InventorySlot> WeaponSlots = new List<IR_InventorySlot>();
    public GameObject weaponHolster;

    public void Start() 
    {
        instance = this;
        CamHolder = GameObject.Find("CamHolder");
        getWeaponSlots();
    }

    public void Update() 
    {
        if (Rebind.GetInputDown("Weapon1")) 
        {
            Equip(0);
        } else if (Rebind.GetInputDown("Weapon2")) 
        {
            Equip(1);
        }
        if (Rebind.GetInputDown("Swap")) 
        {
            swap(CurrentWeapons[0], CurrentWeapons[1]);
            swapSlots(WeaponSlots[0], WeaponSlots[1]);
            //unEquip();
        }

        UpdateSlotInfo();
        //transform.rotation = CamHolder.transform.rotation;
    }

    public void swap(Item_Weapon W1, Item_Weapon W2) 
    {
        Item_Weapon temp = W1;
        W1 = W2;
        W2 = temp;
        CurrentWeapons[0] = W1;
        CurrentWeapons[1] = W2;
    }

    public void swapSlots(IR_InventorySlot I1, IR_InventorySlot I2) 
    {
        IR_Item temp = I1.item;
        I1.item = I2.item;
        I2.item = temp;
    }

    public void Equip(int WeaponNum) 
    {
        if (activeWeaponObj != null) 
        {
            Destroy(activeWeaponObj); //Destroys the current weapon from the screen. 
        }
        activeWeapon = CurrentWeapons[WeaponNum];
        spawnInHand();
        
    }

    public void unEquip() 
    {
        Destroy(activeWeaponObj);
        activeWeapon = null;
    }

    public void spawnInHand() 
    {
        GameObject Cam = GameObject.Find("Camera");
        GameObject Player = GameObject.Find("Player");
        //Debug.Log(Cam.transform.rotation.x);
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        activeWeaponObj = Instantiate(activeWeapon.WeaponPrefab, spawnLocation, CamHolder.transform.rotation);
        activeWeaponObj.transform.SetParent(this.transform);

        activeWeaponObj.transform.rotation = CamHolder.transform.rotation;
        

    }

    #region WeaponInventory

     public void getWeaponSlots() 
    {
        //Gets all the slots and adds them into the List.
        foreach (Transform children in weaponHolster.transform) 
        {
            if (children.tag == "ItemBox") 
            {
                WeaponSlots.Add(children.GetComponent<IR_InventorySlot>());
            }
        }
    }

     public void UpdateSlotInfo() 
    {
        foreach (IR_InventorySlot slot in WeaponSlots) 
        {
            
            if (slot.item != null) //Checks if an item is in the slot. 
            {
                slot.itemIcon.sprite = slot.item.itemIcon;
                slot.itemIcon.gameObject.SetActive(true); //Sets image gameobject to active.
                if (slot.amount > 0) //Checks if this item is stackable and has an amount more than 0 in order to activate amount text.
                {
                    //slot.itemAmountText.gameObject.SetActive(true);
                    //slot.itemAmountText.text = slot.amount.ToString();
                }
            } else if (slot.item == null) //Checks if there is no item in the slot and disables slot information.
            {
                slot.itemIcon.gameObject.SetActive(false);
                //slot.itemAmountText.gameObject.SetActive(false);
            }
        }
    }

    #endregion
    
}
