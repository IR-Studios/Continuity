using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public TextMeshProUGUI InteractText;

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

    [HideInInspector]
    public bool InvOpen = false;
    public bool ChestOpen = false;

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
        InvOpen = true;
        InventoryObj.SetActive(true);
    }
    public void closeInventory() 
    {
        InvOpen = false;
        InventoryObj.SetActive(false);
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
}
