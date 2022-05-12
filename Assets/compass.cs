using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{
    public Transform target;
    public GameObject arrow;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var q = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Tab))
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
