using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_the_boss_script : MonoBehaviour
{
     bool isInInferno = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Inferno") && !isInInferno)
        {
            // damage by  10
            gameObject.GetComponent<BossMech>().damageBoss(10);
            StartCoroutine(InfernoDamageOverTime(2, 1f, other.gameObject)); // Periodic damage
        }
        if (other.CompareTag("Axe"))
        {
            gameObject.GetComponent<BossMech>().damageBoss(5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inferno"))
        {
            isInInferno = false; // Stop periodic damage when exiting the inferno
        }
    }

    IEnumerator InfernoDamageOverTime(int damage, float interval, GameObject inferno)
    {
        isInInferno = true;
        while (isInInferno)
        {
            // Check if the Inferno GameObject is still valid
            if (inferno == null)
            {
                isInInferno = false; // Stop damage if the inferno is destroyed
                yield break;
            }

            yield return new WaitForSeconds(interval); // Wait for the specified interval
            // Apply periodic damage
            gameObject.GetComponent<BossMech>().damageBoss(damage);
        }
    }


}
