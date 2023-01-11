using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponAndTools/New Weapon")]
public class Weapon : ScriptableObject
{
    [InspectorName("Weapon Name")]
    public string weaponName;
    [InspectorName("Weapon Damage Output")]
    public float weaponDMG;
    [InspectorName("Weapong Durability")]
    public float weapongHealth;
    public GameObject WeaponPrefab;
}
