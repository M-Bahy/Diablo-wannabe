using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing_Script : MonoBehaviour
{
    public GameObject healingPotionPrefab;

    List<GameObject> availableHealingPotions = new List<GameObject>();

    List<GameObject> collectedHealingPotions = new List<GameObject>();

    int numberOfHealingPotions = 10;

    int collectedHealingPotionsLimit = 3; 
    // Start is called before the first frame update
    void Start()
    {
     spawnHealingPotions();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnHealingPotions()
    {

         // up (Z) :
        // -80 , -20
      // -52 , 52      --------> X
        // down (z) :
        // 30 , 102
          // -30 , 30      --------> X
      

        for (int i = 0; i < numberOfHealingPotions; i++)
        {
           // pick up or down
            bool up = Random.Range(0, 2) == 0;
            // pick a random x value
            float x =  up ? Random.Range(-52, 52) : Random.Range(-30, 30);
            // pick a random z value
            float z = up ? Random.Range(-80, -20) : Random.Range(30, 102);

            float y = gameObject.transform.position.y;
            availableHealingPotions.Add(Instantiate(healingPotionPrefab, new Vector3(x, y, z), Quaternion.identity));
        }
    }
}
