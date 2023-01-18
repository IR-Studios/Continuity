using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponAndTools/New Weapon Item")]
public class Item_Weapon : IR_Item
{
    [Header("Weapon Information")]

    [InspectorName("Weapon Damage Output")]
    public float weaponDMG; //Base Weapon Damage
    [InspectorName("Weapon Durability")]
    public float weaponHealth; //Base Weapon Health
    public GameObject WeaponPrefab;
}
