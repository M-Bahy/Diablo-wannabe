using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public static GameObject player;


    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;

    [SerializeField] float xRotation;
    [SerializeField] float yRotation;
    [SerializeField] float zRotation;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - xOffset, player.transform.position.y + yOffset, player.transform.position.z - zOffset);
    }

    public void chooseCameraPosition(int choice)
    {
        switch (choice)
        {
            case 1:
                break;
            case 2:
                xRotation = 45.0f;
                break;
            case 3:
                yOffset = 7.56f;
                xRotation = 19.2f;
                break;
        }
    }
}
