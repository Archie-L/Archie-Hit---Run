using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc : MonoBehaviour
{
    private Transform target;
    public GameObject self;
    public NavMeshAgent agent;
    public float walkPointRange;
	private Vector3 newPos;
    public bool PointSet;

    // Start is called before the first frame update
    void Start()
    {
        PointSet = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (!PointSet)
		{
            FindPoint();
        }

        if (PointSet)
        {
            agent.SetDestination(newPos);
        }

        Vector3 distanceToWalkPoint = transform.position - newPos;

        if (distanceToWalkPoint.magnitude < 1f)
		{
            PointSet = false;
		}
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "player fist")
		{
            Debug.Log("hit");
            Destroy(gameObject);
		}
	}

	void FindPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        newPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        PointSet = true;
    }
}

