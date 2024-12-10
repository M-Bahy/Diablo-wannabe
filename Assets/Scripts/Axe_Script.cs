using UnityEngine;

public class Axe_Script : MonoBehaviour
{
    private Vector3 previousPosition;

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

        }
    }
}
