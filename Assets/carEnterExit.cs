using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEnterExit : MonoBehaviour
{
    public carController script;
    public Transform car, empty;
    public GameObject Player, carCam, txt;
    public bool inCar;

    // Start is called before the first frame update
    void Start()
    {
        inCar = false;
        script.enabled = false;
        carCam.SetActive(false);
        txt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inCar)
        {
            GetOutCar();
        }
    }

	private void OnTriggerStay(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            txt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !inCar)
            {
                GetInCar();
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

    void GetInCar()
	{
        Player.transform.parent = car.transform;
        Player.SetActive(false);
        carCam.SetActive(true);
        script.enabled = true;
        txt.SetActive(false);
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
