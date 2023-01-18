using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInWorld : MonoBehaviour
{
   public IR_Item item;
   public GameObject WorldObj;
   public string PromptMessage; 

   [Header("For Weapons Only")]
   public int WorldWeaponHealth;
}
