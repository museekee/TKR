using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetCamera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 offset;

    void Update()
    {
        Rigidbody rb = Player.GetComponent<Rigidbody>();
        // Debug.Log(rb.rotation);
        Vector3 FixedPos = 
            new Vector3(
                Player.transform.position.x + offset.x,
                Player.transform.position.y + offset.y,
                Player.transform.position.z + offset.z
            );
        transform.position = FixedPos;
    }
}
