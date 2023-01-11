using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> CurrentWeapons = new List<Weapon>();
    public Weapon activeWeapon;
    public GameObject activeWeaponObj;
    public GameObject CamHolder;

    public void Start() 
    {
        CamHolder = GameObject.Find("CamHolder");
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
        }

        //transform.rotation = CamHolder.transform.rotation;
    }

    public void swap(Weapon W1, Weapon W2) 
    {
        Weapon temp = W1;
        W1 = W2;
        W2 = temp;
        CurrentWeapons[0] = W1;
        CurrentWeapons[1] = W2;
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

    public void spawnInHand() 
    {
        GameObject Cam = GameObject.Find("Camera");
        GameObject Player = GameObject.Find("Player");
        Debug.Log(Cam.transform.rotation.x);
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        activeWeaponObj = Instantiate(activeWeapon.WeaponPrefab, spawnLocation, Quaternion.Euler(Cam.transform.rotation.x, -Player.transform.rotation.y, Cam.transform.rotation.z));
        activeWeaponObj.transform.SetParent(CamHolder.transform);
        

    }
    
}
