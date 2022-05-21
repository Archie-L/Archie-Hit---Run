using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leScript : MonoBehaviour
{
    public bool leRoll;
    public GameObject lePlayer;

    // Update is called once per frame
    void Update()
    {
        lePlayer.gameObject.GetComponent<ThirdPersonMovement>().rolling = leRoll;
    }
}
