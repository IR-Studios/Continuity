using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int WeaponDMG;
    public int weaponHealth;


     [HideInInspector]
    protected GameObject Cam { get; set; }

    void Start() 
    {
        Cam = GameObject.Find("CamHolder");
    }

    public void Update() 
    {
        if (Rebind.GetInputDown("Attack")) 
        {
            Attack();
        }
    }

    public void Attack() 
    {
         RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 2.0f))
        {
            //Do Something.
        }
    }
}
