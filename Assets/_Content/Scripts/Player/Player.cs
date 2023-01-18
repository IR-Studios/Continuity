using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;
using Continuity.UI;
using Continuity.Movement;

public class Player : MonoBehaviour
{
    public static Player instance;

    public PlayerSettings settings;
    MouseLook ML;
    [HideInInspector]
    protected GameObject Cam { get; set; }

    [Header("Vitals")]
    public float _health; //Current Health
    public float _stamina; //Current Stamina
    public float _hunger; //Current Hunger
    public float _thirst; //Current Thirst

    [HideInInspector]
    public PlayerMovement movement;
    public HUDManager HUD;

    public void Start()
    {
        instance = this;
        Cam = GameObject.Find("CamHolder");
        ML = Cam.GetComponent<MouseLook>();
        movement = GetComponent<PlayerMovement>();
        HUD = GetComponent<HUDManager>();

        _stamina = settings.MaxStamina;
        HUD.UpdateStamina(settings.MaxStamina, _stamina);

        _hunger = settings.maxHunger;
        _thirst = settings.maxThirst;
    }

    private void Update()
    {
        Interact();
        DepleteVitals();

        if (Rebind.GetInputDown("Inventory") && !HUD.InvOpen) 
        {
            HUD.openInventory();
            ML.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            movement.rb.constraints = RigidbodyConstraints.FreezeAll;
        } else if (Rebind.GetInputDown("Inventory") && HUD.InvOpen) 
        {
            HUD.closeInventory();
            ML.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            movement.rb.constraints = RigidbodyConstraints.None;
            movement.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void FixedUpdate()
    {
       
    }

    public void DepleteVitals()
    {
        _hunger -= settings.hungerFallRate * Time.deltaTime;
        _thirst -= settings.thirstFallRate * Time.deltaTime;
        HUD.UpdateVitals(_hunger, _thirst);
    }

    public void Heal(int amount) 
    {
        _health += amount;
        if (_health >= settings.MaxHealth) 
        {
            _health = settings.MaxHealth;
        }
    }

    public void Drink(int amount) 
    {
        _thirst += amount;
        if (_thirst >= settings.maxThirst) 
        {
            _thirst = settings.maxThirst;
        }
    }

    public void Eat(int amount) 
    {
        _hunger += amount;
        if (_hunger >= settings.maxHunger) 
        {
            _hunger = settings.maxHunger;
        }
    }

    #region Interactions
    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 4.0f))
        {
            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * hit.distance, Color.yellow);

            //Find what it's hitting.
            if (hit.transform.tag == "item")
            {
                if (hit.transform.GetComponent<Objective>() != null)
                {
                    Objective obj = hit.transform.GetComponent<Objective>();
                    if (obj.ObjNum == obj.OBJM.CurrentObjective)
                    {
                        HUD.InteractOn("Press " + Rebind.GetEntry("Interact") + " To Set Up Tent");
                        if (Rebind.GetInputDown("Interact"))
                        {
                            Destroy(hit.transform.gameObject);
                            obj.OBJM.Objectives[obj.ObjNum].isComplete = true;
                        }
                    }
                } else if (hit.transform.GetComponent<ItemInWorld>() != null)  
                {
                    ItemInWorld IW = hit.transform.GetComponent<ItemInWorld>();
                    HUD.InteractOn("Press " + Rebind.GetEntry("Interact") + " To Pick Up " + IW.item.itemName);
                    if (Rebind.GetInputDown("Interact"))
                        {
                            IR_Inventory.instance.AddItem(IW.item, IW.WorldWeaponHealth);
                            Destroy(hit.transform.gameObject);
                        }
                } else
                {
                    HUD.InteractOn("Press " + Rebind.GetEntry("Interact") + " To Set Up Tent");
                    if (Rebind.GetInputDown("Interact"))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
             
            } 
            if (hit.transform.tag == "tree") 
            {
                Tree t = hit.transform.GetComponentInParent<Tree>();
                HUD.InteractOn("Press " + Rebind.GetEntry("Interact") + " To Destroy Tree");
                if (Rebind.GetInputDown("Interact"))
                {
                    t.SetTreeHealth(0.0f);
                }
            }

           
        } else
        {
            HUD.InteractOff();
        }
        
    }
    #endregion



}
