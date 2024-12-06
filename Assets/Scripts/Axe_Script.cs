using UnityEngine;

public class Axe_Script : MonoBehaviour
{
    private Vector3 previousPosition;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
   {
        if (PlayerMechanics.isAttacking && other.CompareTag("Summoned_Minions"))
        {
            
                GameObject minion = other.gameObject;
                //minion.GetComponent<Minion_Logic>().TakeDamage(5);
                Debug.Log("Minion Hit");

        }
    }
}
