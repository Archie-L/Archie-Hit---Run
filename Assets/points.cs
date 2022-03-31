using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    public GameObject parentOfChild;
    public GameObject prefab;
    public Transform pointOne, pointTwo, pointThree, pointFour;
    public float spawnTime;
	float time;

    // Update is called once per frame
    public void Update()
    {
        time += Time.deltaTime;

        if(time > spawnTime)
		{
            time = 0;

            int randomChild = Random.Range(1, 5);
            if (randomChild == 1)
            {
                Instantiate(prefab, pointOne.position, Quaternion.identity);
            }
            if (randomChild == 2)
            {
                Instantiate(prefab, pointTwo.position, Quaternion.identity);
            }
            if (randomChild == 3)
            {
                Instantiate(prefab, pointThree.position, Quaternion.identity);
            }
            if (randomChild == 4)
            {
                Instantiate(prefab, pointFour.position, Quaternion.identity);
            }

        }
    }
}
