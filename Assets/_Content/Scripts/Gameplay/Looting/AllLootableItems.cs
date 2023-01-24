using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AllLootableItems : MonoBehaviour
{
    public static AllLootableItems instance;
    public List<IR_Item> Worlditems = new List<IR_Item>(); //List of all the items in the game.
    public List<IR_Item> MedicalItems = new List<IR_Item>(); //List of all Medical Items in the game.
    public List<IR_Item> ConsumableItems = new List<IR_Item>(); //List of all Consumable Items in the game.

    public void Awake() 
    {
        instance = this;
    }

    void Start() 
    {
        LoadItemList();
    }

    public void LoadItemListToFile() 
    {
        
    }

    public void LoadItemList() 
    {
        Object[] items = Resources.LoadAll("Items", typeof(IR_Item));
        
        if (items == null || items.Length == 0) {

        }

        foreach(Object item in items) 
        {
            IR_Item i = (IR_Item)item;
            //Worlditems.Add(i);
            if (i.item == typeOfItem.Medical) 
            {
                MedicalItems.Add(i);
            }
        }
        //LoadMedicalItems();
        LoadConsumableItems();
    }

    public void LoadMedicalItems() 
    {
        foreach(IR_Item item in Worlditems) 
        {
            if (item.item == typeOfItem.Medical) 
            {
                
            }
        }
    }

    public void LoadConsumableItems()
     {
         foreach(IR_Item item in Worlditems) 
        {
            if (item.item == typeOfItem.Food) 
            {
                ConsumableItems.Add(item);
            }
        }
    }
}
