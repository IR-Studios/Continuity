using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagedBuild : MonoBehaviour
{
    public List<Build> Builds = new List<Build>();
}

[System.Serializable]
public class Build 
{
    int buildOrder;
    GameObject buildObj;
}
