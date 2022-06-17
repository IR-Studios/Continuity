using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;
using Continuity.UI;
using Continuity.Movement;

public class Player : MonoBehaviour
{
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
        Cam = GameObject.Find("Camera");
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

    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 2.0f))
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
                } else
                {
                    HUD.InteractOn("Press " + Rebind.GetEntry("Interact") + " To Set Up Tent");
                    if (Rebind.GetInputDown("Interact"))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
             
            } 

           
        } else
        {
            HUD.InteractOff();
        }
        
    }


}
