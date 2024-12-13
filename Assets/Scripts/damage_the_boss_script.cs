using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class damage_the_boss_script : MonoBehaviour
{
     bool isInInferno = false;
    public static bool barAttackedBoss = false;
    public static bool specialAttackedBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        FbScript.boss = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log("Triggered");
        if (other.CompareTag("Inferno") && !isInInferno)
        {
            // damage by  10
            gameObject.GetComponent<BossMech>().damageBoss(10);
            StartCoroutine(InfernoDamageOverTime(2, 1f, other.gameObject)); // Periodic damage
        }
        if (other.CompareTag("Axe") && PlayerMechanics.barAttacking)
        {
            
            if(barAttackedBoss == false)
            {
                StartCoroutine(DelayedDamage(0.3f, 5));
                barAttackedBoss = true;
            }
        }
        if (other.CompareTag("Fireball"))
        {
            gameObject.GetComponent<BossMech>().damageBoss(5);
            Destroy(other.gameObject);
        }
        //if((other.CompareTag("Barbarian")) && other.GetComponent<Animator>().GetBool("isSprint"))
        //{
        //    gameObject.GetComponent<BossMech>().damageBoss(20);
        //}
        if(other.CompareTag("Axe") && PlayerMechanics.circleAttacking)
        {
           if(specialAttackedBoss == false)
            {
                StartCoroutine(DelayedDamage(0.3f, 10));
                specialAttackedBoss = true;
            }
        }
    }
    private IEnumerator DelayedDamage(float delay, int damage)
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        gameObject.GetComponent<BossMech>().damageBoss(damage);  // Apply damage after the delay
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
