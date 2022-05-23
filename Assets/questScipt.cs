using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questScipt : MonoBehaviour
{
    public int numbProgress;
    public int playerProgress;
    public GameObject player, compass;
    public Transform newTarget;

    // Update is called once per frame
    void Update()
    {
        playerProgress = player.gameObject.GetComponent<ThirdPersonMovement>().storyProgress;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (numbProgress == playerProgress)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                compass.GetComponent<compass>().target = newTarget;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
