using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField] GameObject player;

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
}
