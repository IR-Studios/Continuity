using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arctic.Objectives;

public enum ObjectiveType { Area, Interaction, Steps }
public class Objective : MonoBehaviour
{
    public ObjectiveType objectiveType;

    public int ObjNum;

    public ObjectiveManager OBJM;

    private void Start()
    {
        OBJM = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && objectiveType == ObjectiveType.Area)
        {
            if (OBJM.CurrentObjective == ObjNum)
            {
                OBJM.Objectives[ObjNum].isComplete = true;
            }
          
        }
    }
}
