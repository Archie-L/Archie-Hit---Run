using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    public GameObject prefabRight, prefabLeft;
    public float spawnTime;
	float time;
    public float maxSpawned, currentSpawned;

    public int randomNumb;

    // Update is called once per frame
    public void Update()
    {
        time += Time.deltaTime;

        if(time > spawnTime && currentSpawned <= maxSpawned)
		{
            currentSpawned++;

            Transform[] childs = gameObject.GetComponentsInChildren<Transform>();
            randomNumb = Random.Range(0, childs.Length);
            Transform randomObject = childs[randomNumb];

            time = 0;

            int randPrefab = Random.Range(0, 2);
            if(randPrefab == 0)
			{
                Instantiate(prefabRight, randomObject.position, Quaternion.identity);
            }
            if (randPrefab == 1)
            {
                Instantiate(prefabLeft, randomObject.position, Quaternion.identity);
            }
        }
    }
}
