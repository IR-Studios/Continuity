using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

public class BowAndArrow : MonoBehaviour
{
    [Header("Bow Stats")]
    [SerializeField]
    private int InitialDamage;
    [SerializeField]
    private float DamageFallOff;
    [SerializeField]
    private float ChargeUp;
    [SerializeField]
    private float ChargeUpRate;
    [SerializeField]
    private float maxCharge;
    [Header("Ammo")]
    public int ArrowAmmoReserve;
    public int ArrowLoaded;
    
    [Header("Bow GameObjects")]
    public GameObject FakeArrow;
    public GameObject FakeArrowPrefab;
    public GameObject ArrowRigidbody;
    public GameObject ArrowSpawnPoint;

    private GameObject Camera;
    private Camera cam;

    List<IR_InventorySlot> Arrows = new List<IR_InventorySlot>();

    float fov;

    public void Start() 
    {
        Camera = GameObject.Find("Camera");
        cam = Camera.GetComponent<Camera>();

        fov = cam.fieldOfView;
    }
    public void Update() 
    {
        HUDManager.instance.AmmoCount.text = ArrowLoaded + " | " + ArrowAmmoReserve;

        GetInput();
        CheckAmmo();
    }

    public void GetInput() 
    {
        if (Rebind.GetInput("Attack") && ArrowLoaded == 1) 
        {
            if (ChargeUp <= maxCharge) 
            {
                ChargeShot();
            } 
        } else if (Rebind.GetInputUp("Attack") && ArrowLoaded == 1) 
        {
            ShootArrow();
        }

        if (Rebind.GetInputDown("Reload") && ArrowLoaded <= 0) 
        {
            ReloadArrow();
        }

        if (Rebind.GetInput("ADS")) 
        {   
            if (cam.fieldOfView > fov - 20) 
            {
                cam.fieldOfView -= 20 * Time.deltaTime;
            }
        }
        if (Rebind.GetInputUp("ADS"))
        {
            cam.fieldOfView = fov;
            ChargeUp = 0;
        }
    }

    public void ReloadArrow() 
    {
        ArrowLoaded = 1;
        GetAmmoFromInventory();

        FakeArrow.SetActive(true);
    }
    public void ShootArrow() 
    {
        Debug.Log("Arrow Shot with a power of " + ChargeUp);
        GameObject SpawnedArrow = Instantiate(ArrowRigidbody, FakeArrow.transform.position, FakeArrow.transform.rotation);
        FakeArrow.SetActive(false);
        Rigidbody rb = SpawnedArrow.GetComponent<Rigidbody>();
        rb.AddForce(Camera.gameObject.transform.forward * ChargeUp, ForceMode.Force);
        ArrowLoaded--;
        ChargeUp = 0;
    }
    public void ChargeShot() 
    {
        ChargeUp += ChargeUpRate * Time.deltaTime;
    }

    public void CheckAmmo() 
    {
       if (ArrowAmmoReserve <= 0) 
        {
            ArrowAmmoReserve = 0;
        }
    }

    public void GetAmmoFromInventory() 
    {
        foreach (IR_InventorySlot slot in IR_Inventory.instance.Arrows) 
        {
            ArrowAmmoReserve += slot.amount;
        }
    }
}
