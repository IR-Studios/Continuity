using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLootableItems : MonoBehaviour
{
    public static AllLootableItems instance;
    public List<IR_Item> items = new List<IR_Item>();

    public IR_Item[] allMedicalItems;

    public void Awake() 
    {
        instance = this;
    }

    void Start() 
    {
        //allMedicalItems = Resources.Load<IR_Item>("Assets/Resources/Items") as IR_Item[];
    }
}
