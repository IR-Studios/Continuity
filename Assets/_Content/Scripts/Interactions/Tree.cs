using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float t_StartingHealth = 100.0f;
    float t_Health;

    public GameObject t_MainBody;
    public GameObject t_RigidbodyPrefab;

    Vector3 t_Position;
    Vector3 t_SpawnPosition;
    Quaternion t_Rotation;


    void Start() 
    {
        t_Health = t_StartingHealth;
        t_Position = t_MainBody.transform.position;
        t_Rotation = t_MainBody.transform.rotation;

        t_SpawnPosition = new Vector3(t_Position.x, t_Position.y+1, t_Position.z);
    }

    void Update() 
    {
        if (t_Health <= 0) 
        {
            treeDeath();
        }
    }

    void treeDeath() 
    {
        t_MainBody.SetActive(false); //Disables the original tree model.
        GameObject t_Spawned = Instantiate(t_RigidbodyPrefab, t_SpawnPosition, t_Rotation);
        t_Spawned.transform.parent = this.transform;
        Destroy(t_MainBody);
    }

    public float SetTreeHealth(float t) 
    {
        t_Health = t;
        return t_Health;
    }
}
