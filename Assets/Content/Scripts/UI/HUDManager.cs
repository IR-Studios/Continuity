using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
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

    private void Awake()
    {
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
    }

    public void UpdateStamina(float maxStamina, float stamina) 
    {
        _Stamina.maxValue = maxStamina;
        _Stamina.value = stamina;
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
}
