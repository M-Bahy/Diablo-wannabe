using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_damage_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "jump radius"){
            Debug.Log("hit");
            // get the player mechanics script
            // call the take damage function
            PlayerMechanics player = gameObject.GetComponent<PlayerMechanics>();
            player.playerCurrenttHealth -= 10;


        }
    }

    private void OnCollisionEnter(Collision other) {
         Debug.Log(other.gameObject.tag);
         // jump radius
        if(other.gameObject.tag == "jump radius"){
            Debug.Log("hit");
            // get the player mechanics script
            // call the take damage function
            PlayerMechanics player = gameObject.GetComponent<PlayerMechanics>();
            player.playerCurrenttHealth -= 10;


        }
    }
}
