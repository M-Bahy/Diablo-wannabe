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
        if(other.gameObject.tag == "jump radius"){
            PlayerMechanics player = gameObject.GetComponent<PlayerMechanics>();
            player.playerCurrenttHealth -= 10;
        }
    }

   
}
