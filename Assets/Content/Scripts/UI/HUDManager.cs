using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI InteractText;

    [Header("Objectives")]
    public TextMeshProUGUI ObjTitle;
    public TextMeshProUGUI ObjText;
    public GameObject ObjNotif;

    private void Start()
    {
        InteractText = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
        InteractOff();
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
