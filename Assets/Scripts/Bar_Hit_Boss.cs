using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_Hit_Boss : MonoBehaviour
{
    public GameObject boss;
    public static bool barAttackedBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ontrigger
    private void OnTriggerEnter(Collider other)
    {
        if (!barAttackedBoss)
        {
            if (other.CompareTag("Axe"))
            {
                boss.GetComponent<BossMech>().damageBoss(5);
            }
            barAttackedBoss = true;
        }
    }
}
