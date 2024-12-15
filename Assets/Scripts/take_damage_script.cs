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
        PlayerMechanics player = gameObject.GetComponent<PlayerMechanics>();
         if(player.inCover){
                return;
            }
        if(other.gameObject.tag == "jump radius"){
            // 20
            Debug.Log("Dive Bomb");
            if (gameObject.tag == "Barbarian"){
                player.takeDamage(10);
            }
            else{
                player.takeDamage(20);
            }
        }
        if(other.gameObject.tag == "spikes"){
            // 30
            Debug.Log("Spikes");
            player.takeDamage(30);
        }
    }

   
}
