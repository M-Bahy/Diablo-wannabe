using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_phase1_script : MonoBehaviour
{
    public static bool fightStarted = false;
    public GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        // look at player
        transform.LookAt(player.transform);
       // anim.Play("Dive bommb");
    }
}
