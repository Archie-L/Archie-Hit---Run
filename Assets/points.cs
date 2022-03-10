using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    public GameObject parentOfChild;
    public GameObject prefab;
    public Transform pointOne, pointTwo, pointThree, pointFour;
    public float spawnTime = 5f;
    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > spawnTime)
		{
            time = 0;

            Vector3 pos;

            int randomChild = Random.Range(1, 5);
            if (randomChild == 1)
            {
                pos = pointOne.position;
            }
            if (randomChild == 2)
            {
                pos = pointTwo.position;
            }
            if (randomChild == 3)
            {
                pos = pointThree.position;
            }
            else
            {
                pos = pointFour.position;
            }

            npc_points npc = Instantiate(prefab, pos, Quaternion.identity).GetComponent<npc_points>();
            npc.spawnPos = randomChild;
        }
    }
}
