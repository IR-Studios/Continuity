using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Continuity.Keybinds;

namespace Continuity.UI
{
public class MenuManager : MonoBehaviour
{
    public List<Menu> Menus = new List<Menu>();

    public void setMenuActive(string menuName) 
    {
        for (int i=0; i < Menus.Count; i++) 
        {
            if (Menus[i].MenuName == menuName) 
            {
                Menus[i].MenuObj.SetActive(true);
            } else 
            {
                Menus[i].MenuObj.SetActive(false);
            }
        }
    }
    public void Quit() 
    {
        Application.Quit();
    }

}
}

[System.Serializable]
public class Menu 
{
    public string MenuName;
    public GameObject MenuObj;
}
