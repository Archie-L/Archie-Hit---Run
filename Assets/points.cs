using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    public GameObject prefab;
    public GameObject joePrefab;
    public float spawnTime;
	float time;
    public float maxSpawned, currentSpawned;

    public int randomNumb;

    private void Start()
    {
        Transform[] childs = gameObject.GetComponentsInChildren<Transform>();
        randomNumb = Random.Range(0, childs.Length);
        Transform randomObject = childs[randomNumb];

        Instantiate(joePrefab, randomObject.position, Quaternion.identity);
    }

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

            Instantiate(prefab, randomObject.position, Quaternion.identity);
        }
    }
}
