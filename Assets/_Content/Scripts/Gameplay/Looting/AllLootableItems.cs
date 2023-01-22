using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLootableItems : MonoBehaviour
{
    public static AllLootableItems instance;
    public List<IR_Item> items = new List<IR_Item>();

    public void Awake() 
    {
        instance = this;
    }
}
