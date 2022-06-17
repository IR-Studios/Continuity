using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLook : MonoBehaviour
{
    GameObject PlayerObj;

    private void Start()
    {
        PlayerObj = GameObject.Find("Player");

    }

    private void Update()
    {
        Vector3 StyleLook = new Vector3(0, 1, 3);

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * StyleLook);
        this.transform.Rotate(0, 180, 0);
    }
}
