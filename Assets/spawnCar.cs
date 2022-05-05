using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCar : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform car;
    public GameObject txt;

    // Start is called before the first frame update
    void Start()
    {
        txt.SetActive(false);
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
            txt.SetActive(true);

			if (Input.GetKeyDown(KeyCode.F))
			{
                Spawn();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            txt.SetActive(false);
        }
    }

    void Spawn()
	{
        car.position = spawnPoint.position;
        car.rotation = spawnPoint.rotation;
    }
}
