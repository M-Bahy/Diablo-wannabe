using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Clone : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to inflict
    private List<GameObject> targets = new List<GameObject>(); // To store overlapping enemies
    AudioManagerScript audioManager;

    private void Awake()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DamageAndDestroyAfterDelay(5f)); // Start the process with a delay
    }

    private void OnTriggerEnter(Collider other)
    {
         if (!targets.Contains(other.gameObject))
            targets.Add(other.gameObject);
        
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove targets when they exit the trigger area
        if (targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }

    private IEnumerator DamageAndDestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Damage all valid targets in the area
        foreach (GameObject target in targets)
        {
            if (target != null) // Ensure target hasn't been destroyed
            {
                // Attempt to call TakeDamage on the target
                var targetLogic = target.GetComponent<MonoBehaviour>();
                if (targetLogic != null)
                {
                    // Check for TakeDamage method and invoke it
                    if (target.CompareTag("Minion") || target.CompareTag("Summoned_Minions"))
                    {
                        target.GetComponent<Minion_Logic>().TakeDamage(damageAmount);
                    }
                    else if (target.CompareTag("Demon"))
                    {
                        var takeDamageMethod = targetLogic.GetType().GetMethod("damageDemon");
                        if (takeDamageMethod != null)
                        {
                            takeDamageMethod.Invoke(targetLogic, new object[] { damageAmount });
                        }
                    }
                }
            }
        }
        audioManager.PlaySFX(audioManager.Explosive_Detonates);
        transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}