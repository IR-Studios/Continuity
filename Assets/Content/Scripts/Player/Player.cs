using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSettings settings;
    MouseLook ML;
    [HideInInspector]
    protected GameObject Cam { get; set; }

    [Header("Vitals")]
    public float _health; //Current Health
    public float _stamina; //Current Stamina

    [HideInInspector]
    public PlayerMovement movement;
    public HUDManager HUD;

    public void Start()
    {
        Cam = GameObject.Find("Camera");
        ML = Cam.GetComponent<MouseLook>();
        movement = GetComponent<PlayerMovement>();
        HUD = GetComponent<HUDManager>();
    }

    private void Update()
    {
        Interact();
    }

    private void FixedUpdate()
    {
       
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
                        HUD.InteractOn("Press E To Set Up Tent");
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Destroy(hit.transform.gameObject);
                            obj.OBJM.Objectives[obj.ObjNum].isComplete = true;
                        }
                    }
                } else
                {
                    HUD.InteractOn("Press E To Destroy Object");
                    if (Input.GetKeyDown(KeyCode.E))
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
