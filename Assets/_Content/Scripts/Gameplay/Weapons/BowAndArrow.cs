using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

public class BowAndArrow : MonoBehaviour
{
    [SerializeField]
    private int InitialDamage;
    [SerializeField]
    private float DamageFallOff;
    [SerializeField]
    private float ChargeUp;
    [SerializeField]
    private float ChargeUpRate;

    public int ArrowAmmo;

    public GameObject FakeArrow;
    public GameObject FakeArrowPrefab;
    public GameObject ArrowRigidbody;
    public GameObject ArrowSpawnPoint;

    private GameObject Camera;
    private Camera cam;

    public void Start() 
    {
        Camera = GameObject.Find("Camera");
        cam = Camera.GetComponent<Camera>();

    }


    public void Update() 
    {
        if (Rebind.GetInput("Attack")) 
        {
            ChargeShot();
        }
        if (Rebind.GetInputUp("Attack") && ArrowAmmo > 0) 
        {
            ShootArrow();
            if (ArrowAmmo > 0) 
            {
                FakeArrow.SetActive(true);
            }
        } else if (Rebind.GetInputUp("Attack") && ArrowAmmo <= 0) {
            ChargeUp = 0;
        }

        if (Rebind.GetInputDown("ADS") ) 
        {
            cam.fieldOfView -= 10;
        } else if (Rebind.GetInputUp("ADS"))
        {
            cam.fieldOfView += 10;
        }
    }
    public void ShootArrow() 
    {
        Debug.Log("Arrow Shot with a power of " + ChargeUp);
        GameObject SpawnedArrow = Instantiate(ArrowRigidbody, FakeArrow.transform.position, FakeArrow.transform.rotation);
        FakeArrow.SetActive(false);
        Rigidbody rb = SpawnedArrow.GetComponent<Rigidbody>();
        rb.AddForce(Camera.gameObject.transform.forward * ChargeUp, ForceMode.Force);
        ArrowAmmo--;
        ChargeUp = 0;
    }

    public void ChargeShot() 
    {
        ChargeUp += ChargeUpRate * Time.deltaTime;
    }

    public void CheckAmmo() 
    {
       
    }
}
