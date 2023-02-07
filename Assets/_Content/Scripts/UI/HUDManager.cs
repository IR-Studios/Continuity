using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Continuity.Keybinds;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public TextMeshProUGUI InteractText;

    [Header("Canvas")]
    public GameObject PauseCanvas;

    [Header("Player Stats")]
    public Slider _Health;
    public Slider _Stamina;
    public Slider _Hunger;
    public Slider _Thirst;

    [Header("Objectives")]
    public TextMeshProUGUI ObjTitle;
    public TextMeshProUGUI ObjText;
    public GameObject ObjNotif;

    [Header("Inventory/Chest Inventory")]
    public GameObject InventoryObj;
    public GameObject ChestInventoryObj;

    public GameObject itemInfoObj;
    public TextMeshProUGUI itemInfoName;
    public Image itemInfoIcon;
    public TextMeshProUGUI itemInfoDescription;

    [Header("Weapons")]
    public TextMeshProUGUI AmmoCount;
    public TextMeshProUGUI AmmoLoaded;
    public GameObject AmmoObj;

    [Header("Game Telemetry")]
    public TextMeshProUGUI FpsText;

    [HideInInspector]
    public bool InvOpen = false;
    public bool ChestOpen = false;
    public bool Paused = false;

    private float deltaTime;

    private void Awake()
    {
        instance = this;

        //Finding all the HUD objects
        //Player Stats
        _Health = GameObject.Find("Health").GetComponent<Slider>();
        _Stamina = GameObject.Find("Stamina").GetComponent<Slider>();
        _Hunger = GameObject.Find("Hunger").GetComponent<Slider>();
        _Thirst = GameObject.Find("Thirst").GetComponent<Slider>();
    }

    private void Start()
    {
        InteractText = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
        InteractOff();
        closeInventory();
        closeChestInventory();
        ItemInfo(false);
    }

    public void Update() 
    {
        DisplayFPS();

        if (Rebind.GetInputDown("Pause") && !Paused) 
        {
            PauseGame(true);
        } else if (Rebind.GetInputDown("Pause") && Paused) 
        {
            PauseGame(false);
        }
    }

    public void UpdateStamina(float maxStamina, float stamina) 
    {
        _Stamina.maxValue = maxStamina;
        _Stamina.value = stamina;
    }

    public void UpdateVitals(float hunger, float thirst)
    {
        PlayerSettings PS = gameObject.GetComponent<Player>().settings;
        _Hunger.maxValue = PS.maxHunger;
        _Thirst.maxValue = PS.maxThirst;


        _Hunger.value = hunger;
        _Thirst.value = thirst;
    }

    //Interact Text
    public void InteractOn(string text)
    {
        InteractText.enabled = true;
        InteractText.text = text;
    }
    public void InteractOff()
    {
        InteractText.enabled = false;
        InteractText.text = "";
    }

    public void openInventory() 
    {
        if (!Paused)
        {
            InvOpen = true;
            InventoryObj.SetActive(true);
        }
        
    }
    public void closeInventory() 
    {
        if (!Paused) 
        {
            InvOpen = false;
            InventoryObj.SetActive(false);
        }
 
    }

    public void OpenChestInventory() 
    {
        ChestOpen = true;
        ChestInventoryObj.SetActive(true);
    }
    public void closeChestInventory() 
    {
        ChestOpen = false;
        ChestInventoryObj.SetActive(false);
    }

    public void ItemInfo(bool active) 
    {
        if (active) 
        {
            itemInfoObj.SetActive(true);
        } else if (!active) 
        {
            itemInfoObj.SetActive(false);
        }
        
    }
    
    public void DisplayFPS() 
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
         float fps = 1.0f / deltaTime;
         FpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }

    public void PauseGame(bool pause) 
    {
        MouseLook ML = GameObject.Find("CamHolder").GetComponent<MouseLook>();
        Player P = transform.GetComponent<Player>();

        if (pause) 
        {
            Paused = true;
            PauseCanvas.gameObject.SetActive(true);
            ML.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            P.movement.rb.constraints = RigidbodyConstraints.FreezeAll;
        } else if (!pause) 
        {
            Paused = false;
            PauseCanvas.gameObject.SetActive(false);
            ML.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            P.movement.rb.constraints = RigidbodyConstraints.None;
            P.movement.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    
    }
}
