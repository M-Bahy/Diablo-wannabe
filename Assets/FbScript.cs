using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FbScript : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Move the Fireball towards the target
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Destroy the Fireball when it reaches the target
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Adjust radius as needed
                GameObject minion = new GameObject();
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Minion"))
                    {
                        minion = collider.gameObject;
                        break;
                    }
                }
                if (minion.CompareTag("Minion"))
                {
                    minion.GetComponent<Minion_Logic>().TakeDamage(5);
                }
                Destroy(gameObject);
            }
        }
    }
    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Minion"))
        {
            collision.gameObject.GetComponent<Minion_Logic>().TakeDamage(5);
        }
        Destroy(gameObject);
    }
}
