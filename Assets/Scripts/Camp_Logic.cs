using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Camp_Logic : MonoBehaviour
{
    float minX;
    float maxX;
    float minZ;
    float maxZ;
    float minionY = 0.176f;
    float demonY = 0.21f;

    [SerializeField] GameObject minion;
    [SerializeField] GameObject demon;
    [SerializeField] GameObject key;

    List<GameObject> minionsArray = new List<GameObject>();
    List<GameObject> demonsArray = new List<GameObject>();

    bool keyFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        minX = this.transform.position.x - 20;
        minZ = this.transform.position.z - 20;
        maxX = this.transform.position.x + 20;
        maxZ = this.transform.position.z + 20;

        //Instantiate(demon, new Vector3(minX, demonY, minZ), Quaternion.identity);
        //Instantiate(minion, new Vector3(minX, minionY, minZ), Quaternion.identity);

        int demonCount = Random.Range(1, 3);
        int minionCount = Random.Range(8, 11);
        GameObject tmp;
        for (int i = 0; i < demonCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(demonY);
            tmp = Instantiate(demon, randomPosition, Quaternion.identity);
            demonsArray.Add(tmp);
        }

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(minionY);
            tmp = Instantiate(minion, randomPosition, Quaternion.identity);
            minionsArray.Add(tmp);
        }

    }

    Vector3 GetRandomPosition(float y)
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, y, randomZ);
    }

    // Update is called once per frame
    void Update()
    {
        // THIS CODE IS NOT AN EFFICIENT WAY TO CHECK IF ALL MINIONS AND DEMONS ARE DEAD SINCE IT'LL CHECK EVERY FRAME
        foreach (var minion in minionsArray)
        {
            // if a minion is alive, put the key as false
        }

        foreach (var demon in demonsArray)
        {
            // if a demon is alive, put the key as false
        }

        if (keyFlag)
        {
            Instantiate(key, new Vector3((minX + maxX) / 2, 1.0f, (minZ + maxZ) / 2), Quaternion.identity);
            keyFlag = true;
        }
    }
}
