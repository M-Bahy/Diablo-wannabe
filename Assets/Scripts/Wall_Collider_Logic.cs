using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Collider_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Destroy(collision.gameObject);
        }
    }
}
