using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Healing_Script : MonoBehaviour
{
    public GameObject healingPotionPrefab;
    public TMP_Text bottleCountText;
    List<GameObject> availableHealingPotions = new List<GameObject>();

    public static List<GameObject> collectedHealingPotions = new List<GameObject>();
    Animator anim;
    int numberOfHealingPotions = 10;

    int collectedHealingPotionsLimit = 3; 

    public GameObject healParticle; 

    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
     spawnHealingPotions();   
     anim = gameObject.GetComponent<Animator>();
     bottleCountText.text = "X " + collectedHealingPotions.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(collectedHealingPotions.Count > 0)
            {
               // PlayerMechanics player = gameObject.GetComponent<PlayerMechanics>();
                if (PlayerMechanics.playerCurrenttHealth >= PlayerMechanics.playerMaxHealth)
                {
                    return;
                }
                int healingAmount = PlayerMechanics.playerMaxHealth /2;
                PlayerMechanics.playerCurrenttHealth += healingAmount;
                if (PlayerMechanics.playerCurrenttHealth > PlayerMechanics.playerMaxHealth)
                {
                    PlayerMechanics.playerCurrenttHealth = PlayerMechanics.playerMaxHealth;
                }
                collectedHealingPotions.RemoveAt(0);
                audioManager.PlaySFX(audioManager.Wanderer_Healing_Potion);
                bottleCountText.text = "X " + collectedHealingPotions.Count;
                GameObject healEffect = Instantiate(healParticle, gameObject.transform.position, Quaternion.identity);
                healEffect.transform.parent = gameObject.transform;
            }
        }
        
    }

    void spawnHealingPotions()
    { 
        //Debug.Log("is level 1: " + PlayerMechanics.isLevel1);
        if (PlayerMechanics.isLevel1){
            // region 1 :
            // X : 108.7  ,   330.79
            // Z :    12    ,   233.2

            // region 2 :
            // X : 108.7  ,   330.79
            // Z :    313.2    ,   485.2

            // spawn half of the potions in region 1 and the other half in region 2
            for (int i = 0; i < numberOfHealingPotions; i++)
            {
                // pick up or down
                bool up = Random.Range(0, 2) == 0;
                // pick a random x value
                float x =  up ? Random.Range(108.7f, 330.79f) : Random.Range(108.7f, 330.79f);
                // pick a random z value
                float z = up ? Random.Range(12, 233.2f) : Random.Range(313.2f, 485.2f);

                float y = gameObject.transform.position.y - 2.5f;
                GameObject healingPotion = Instantiate(healingPotionPrefab, new Vector3(x, y, z), Quaternion.identity);
                // scale the potion down
                healingPotion.transform.localScale = new Vector3(5, 5, 5);
                availableHealingPotions.Add(healingPotion);
            }

        }
        else{
        // Level 2

        // Box 1 :
        // bottom left : X : 23.25 , Z : -24.174
        // top right : X : 27.931 , Z : -8.509

         // Box 2 :
        // bottom left : X : -31.15 , Z : -21.3
        // top right : X : -15.27 , Z : -25.21

         // Box 3 :
        // bottom left : X : -26.189 , Z : 38.419
        // top right : X : -38.93 , Z : 27.5

         // Box 4 :
        // bottom left : X : 29.7 , Z : 29.7
        // top right : X : 16.6 , Z : 39.46

            for (int i = 0; i < numberOfHealingPotions; i++)
            {
            // // pick up or down
            //     bool up = Random.Range(0, 2) == 0;
            //     // pick a random x value
            //     float x =  up ? Random.Range(-52, 52) : Random.Range(-30, 30);
            //     // pick a random z value
            //     float z = up ? Random.Range(-80, -20) : Random.Range(30, 102);

            //     float y = gameObject.transform.position.y;
            //     availableHealingPotions.Add(Instantiate(healingPotionPrefab, new Vector3(x, y, z), Quaternion.identity));


            Vector3 position;
            do
            {
                // pick up or down
                bool up = Random.Range(0, 2) == 0;
                // pick a random x value
                float x = up ? Random.Range(-52, 52) : Random.Range(-30, 30);
                // pick a random z value
                float z = up ? Random.Range(-80, -20) : Random.Range(30, 102);

                float y = gameObject.transform.position.y;
                position = new Vector3(x, y, z);
            } while (IsInsideAnyBox(position));

            availableHealingPotions.Add(Instantiate(healingPotionPrefab, position, Quaternion.identity));


            }
        }
       
    }

    private bool IsInsideAnyBox(Vector3 position)
    {
        return IsInsideBox(position, new Vector3(23.25f, 0, -24.174f), new Vector3(27.931f, 0, -8.509f)) ||
               IsInsideBox(position, new Vector3(-31.15f, 0, -21.3f), new Vector3(-15.27f, 0, -25.21f)) ||
               IsInsideBox(position, new Vector3(-26.189f, 0, 38.419f), new Vector3(-38.93f, 0, 27.5f)) ||
               IsInsideBox(position, new Vector3(29.7f, 0, 29.7f), new Vector3(16.6f, 0, 39.46f));
    }

    private bool IsInsideBox(Vector3 position, Vector3 bottomLeft, Vector3 topRight)
    {
        return position.x >= bottomLeft.x && position.x <= topRight.x &&
               position.z >= bottomLeft.z && position.z <= topRight.z;
    }

    public void spawnHealingPotionsInCamps (float minX, float maxX, float minZ, float maxZ)
    {
        for (int i = 0; i < 2; i++)
        {
            // pick up or down
            bool up = Random.Range(0, 2) == 0;
            // pick a random x value
            float x =  up ? Random.Range(minX, maxX) : Random.Range(minX, maxX);
            // pick a random z value
            float z = up ? Random.Range(minZ, maxZ) : Random.Range(minZ, maxZ);

            float y = gameObject.transform.position.y - 2f;
            GameObject healingPotion = Instantiate(healingPotionPrefab, new Vector3(x, y, z), Quaternion.identity);
            bool isNull = healingPotion == null;
            //Debug.Log("healing potion is null: " + isNull);
            healingPotion.transform.localScale = new Vector3(3, 3, 3);
            availableHealingPotions.Add(healingPotion);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "potion")
        {
            if (collectedHealingPotions.Count < collectedHealingPotionsLimit)
            {
                collectedHealingPotions.Add(availableHealingPotions[0]);
                availableHealingPotions.RemoveAt(0);
                Destroy(other.gameObject);
                audioManager.PlaySFX(audioManager.Item_Picked_Up);
                int count = collectedHealingPotions.Count;
                bottleCountText.text = "X " + count;
            }
        }
    }
}
