using UnityEngine;
using System.Collections.Generic;
public class Axe_Script : MonoBehaviour
{
    private Vector3 previousPosition;
    public static List<GameObject> affectedMinions = new List<GameObject>();
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
   {
        if (PlayerMechanics.barAttacking && other.CompareTag("Summoned_Minions"))
        {
            
            GameObject minion = other.gameObject;
            if (minion == PlayerMechanics.minion)
            {
                minion.GetComponent<Minion_Logic>().TakeDamage(5);
            }
            // Debug.Log("Minion Hit");
            PlayerMechanics.minion = null;    
        }
        if (PlayerMechanics.circleAttacking && other.CompareTag("Summoned_Minions"))
        {
           
                if(affectedMinions.Contains(other.gameObject))
               {
                return;
               }
                GameObject minion = other.gameObject;
                minion.GetComponent<Minion_Logic>().TakeDamage(5);
                affectedMinions.Add(minion);

            // Debug.Log("Minion Hit");

        }
        if (PlayerMechanics.barAttacking && other.CompareTag("Minion"))
        {

            GameObject minion = other.gameObject;
            if (minion == PlayerMechanics.minion)
            {
                minion.GetComponent<Minion_Logic>().TakeDamage(5);
            }
            // Debug.Log("Minion Hit");
            PlayerMechanics.minion = null;
        }
        if (PlayerMechanics.barAttacking && other.CompareTag("Demon"))
        {

            GameObject minion = other.gameObject;
            if (minion == PlayerMechanics.minion)
            {
                minion.GetComponent<DemonLogic>().damageDemon(5);
            }
            // Debug.Log("Minion Hit");
            PlayerMechanics.minion = null;
        }


        if (PlayerMechanics.circleAttacking && other.CompareTag("Minion"))
        {

            if (affectedMinions.Contains(other.gameObject))
            {
                return;
            }
            GameObject minion = other.gameObject;
            minion.GetComponent<Minion_Logic>().TakeDamage(5);
            affectedMinions.Add(minion);

            // Debug.Log("Minion Hit");

        }
        if (PlayerMechanics.circleAttacking && (other.CompareTag("Demon")))
        {

            if (affectedMinions.Contains(other.gameObject))
            {
                return;
            }
            GameObject minion = other.gameObject;
            minion.GetComponent<DemonLogic>().damageDemon(5);
            affectedMinions.Add(minion);

            // Debug.Log("Minion Hit");

        }

    }
}
