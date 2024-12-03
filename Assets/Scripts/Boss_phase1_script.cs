using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_phase1_script : MonoBehaviour
{
    public static bool fightStarted = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // look at player
        transform.LookAt(player.transform);
    }
}
