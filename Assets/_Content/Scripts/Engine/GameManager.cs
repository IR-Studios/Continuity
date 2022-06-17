using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject FIRST_SPAWNPOINT;

    //Player Prefab
    public GameObject PLAYER_PREFAB;

    void Awake() 
    {
        Instantiate(PLAYER_PREFAB, FIRST_SPAWNPOINT.transform.position, FIRST_SPAWNPOINT.transform.rotation);
    }

    void start() 
    {
        //FIRST_SPAWNPOINT = GameObject.FindGameObjectWithTag("FirstSpawn");

        

    }
}
