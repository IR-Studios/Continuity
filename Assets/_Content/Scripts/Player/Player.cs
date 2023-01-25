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
            HUD.closeChestInventory();
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

            if (hit.transform.tag == "Chest") 
            {
                LootableChest LC = hit.transform.GetComponent<LootableChest>();
                if (!LC.ChestFilled && !LC.isSearching) 
                {
                    HUD.InteractOn("Press <b><color=yellow>" + Rebind.GetEntry("Interact") + "</color></b> To Open Chest (Untouched)");
                } else if (LC.ChestFilled) 
                {
                    HUD.InteractOn("Press <b><color=yellow>" + Rebind.GetEntry("Interact") + "</color></b> To Open Chest");
                } else if (!LC.ChestFilled && LC.isSearching) 
                {
                    HUDManager.instance.InteractOn("Searching...");
                }
                if (Rebind.GetInputDown("Interact") && !LC.ChestFilled) 
                {
                    LC.StartCoroutine(LC.SearchChest());
                    ML.enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    movement.rb.constraints = RigidbodyConstraints.FreezeAll;
                } else if (Rebind.GetInputDown("Interact") && LC.ChestFilled) 
                {
                    LC.DisplayChest();
                    LC.ChestOpened = true;
                    HUD.openInventory();
                    HUD.OpenChestInventory();
                    ML.enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    movement.rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }

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
                    if (IW.amount > 1) 
                    {
                        HUD.InteractOn("Press <b><color=yellow>" + Rebind.GetEntry("Interact") + "</color></b> To Pick Up " + IW.item.itemName + " <b><color=yellow>x" + IW.amount + "</color></b>");
                    } else 
                    {
                        HUD.InteractOn("Press <b><color=yellow>" + Rebind.GetEntry("Interact") + "</color></b> To Pick Up " + IW.item.itemName + " <b><color=yellow>");
                    }
                    
                    if (Rebind.GetInputDown("Interact"))
                        {
                            IR_Inventory.instance.AddItem(IW.item, IW.WorldWeaponHealth, IW.amount);
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
                Tree tree = hit.transform.GetComponentInParent<Tree>();
                if (Rebind.GetInputDown("Attack")) 
                {
                    tree.DamageTree(10);
                    int randomNum = Random.Range(0, 25);
                    IR_Inventory.instance.AddItem(tree.ItemToGive, tree.ItemToGive.itemHealth, randomNum);
                }
                
            }

           
        } else
        {
            HUD.InteractOff();
        }
        
    }
    #endregion



}
