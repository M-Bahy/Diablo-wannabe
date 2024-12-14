using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public static GameObject player;

    [SerializeField] static float xOffset;
    [SerializeField] static float yOffset;
    [SerializeField] static float zOffset;

    [SerializeField] static float xRotation;
    [SerializeField] static float yRotation;
    [SerializeField] static float zRotation;
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        if (!PlayerMechanics.isLevel1)
        {
            xOffset = -2.84f;
            yOffset = 35.3f;
            zOffset = 22.1f;

            xRotation = 50.0f;
            yRotation = 0.347f;
            zRotation = 0.003f;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
        else
        {
            chooseCameraPosition(MainMenu_Script.cameraChoice);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - xOffset, player.transform.position.y + yOffset, player.transform.position.z - zOffset);
    }

    public static void chooseCameraPosition(int choice)
    {
        switch (choice)
        {
            case 1:
                xOffset = 5.64f;
                yOffset = 10.68f;
                zOffset = 5.64f;
                xRotation = 30.8f;
                yRotation = 46.821f;
                zRotation = 0f;
                break;
            case 2:
                xOffset = 5.64f;
                yOffset = 10.68f;
                zOffset = 5.64f;
                xRotation = 45.0f;
                yRotation = 46.821f;
                zRotation = 0.0f;
                break;
            case 3:
                xOffset = 5.64f;
                yOffset = 7.56f;
                zOffset = 5.64f;
                xRotation = 19.2f;
                yRotation = 46.821f;
                zRotation = 0.0f;
                break;
        }
    }
}
