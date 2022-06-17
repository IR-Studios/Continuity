using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Continuity.Keybinds 
{


public class RuntimeRebind : MonoBehaviour
{
   private KeyEditInfo keyEditInfo;
   private string actionName = "";
   private bool create = false;

   void OnEnable()
    {
        Rebind.SetupSerializers();
        Rebind.defaultKeys.Dictionary = Rebind.defaultBindsSerializer.Read();
        Rebind.keys.Dictionary = Rebind.keyBindsSerializer.Read();
    }

    public bool pollForInput()
    {
        InputCode poll = checkInput();
        if (poll != InputCode.None)
        {
            Rebind.BindKeyToAction(keyEditInfo.actionName, poll, keyEditInfo.listIndex);
            Rebind.defaultBindsSerializer.Save(Rebind.defaultKeys.Dictionary);
            Rebind.keyBindsSerializer.Save(Rebind.defaultKeys.Dictionary);
            return false;
        }
        return true;
    }

    private void createDictionaryEntry()
    {
        actionName = actionName.Replace(" ", string.Empty);
        Rebind.CreateEntry(actionName);
        actionName = "";
        create = false;
        Rebind.defaultBindsSerializer.Save(Rebind.defaultKeys.Dictionary);
        Rebind.keyBindsSerializer.Save(Rebind.defaultKeys.Dictionary);
    }

     private InputCode checkInput()
    {
        if (Event.current.shift)
        {
            return InputCode.LeftShift;
        }

        else if (Event.current.isKey)
        {
            return (InputCode)((int)Event.current.keyCode);
        }
        else if (Event.current.type == EventType.ScrollWheel)
        {
            if (Event.current.delta.y > 0)
            {
                return InputCode.MouseScrollDown;
            }
            else
            {
                return InputCode.MouseScrollUp;
            }
        }

        else if (Event.current.isMouse)
        {
            return (InputCode)323 + Event.current.button;
        }

        return InputCode.None;
    }
}
public struct KeyEditInfo
{
    public bool editing;
    public string actionName;
    public int listIndex;

    public KeyEditInfo(string ActionName, int ListIndex)
    {
        editing = false;
        actionName = ActionName;
        listIndex = ListIndex;
    }
}
}
