using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEnterExit : MonoBehaviour
{
    public carController script;
    public Transform car, empty;
    public GameObject Player, carCam;
    public bool inCar;

    // Start is called before the first frame update
    void Start()
    {
        inCar = false;
        script.enabled = false;
        carCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inCar)
        {
            GetOutCar();
        }

        if (Input.GetKeyDown(KeyCode.F) && !inCar)
        {
            GetInCar();
        }
    }

	private void OnTriggerStay(Collider other)
	{
		/*if(other.gameObject.tag == "Player")
		{
			if (Input.GetKeyDown(KeyCode.F) && !inCar)
			{
                GetInCar();
			}
        }*/
	}

    void GetInCar()
	{
        Player.transform.parent = car.transform;
        Player.SetActive(false);
        carCam.SetActive(true);
        script.enabled = true;
        inCar = true;
    }

    void GetOutCar()
    {
        Player.transform.parent = empty.transform;
        Player.SetActive(true);
        carCam.SetActive(false);
        script.enabled = false;
        inCar = false;
    }
}
