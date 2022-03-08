using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc : MonoBehaviour
{
    private Transform target;
    public GameObject self;
    public GameObject player;
    public NavMeshAgent agent;
    public float walkPointRange;
	private Vector3 newPos;
    public bool PointSet;
    Rigidbody rb;
    public float thrust = 2000f;
	Vector3 m_EulerAngleVelocity;
    float time;
    public float WaitTime = 5f;
    public bool KnockedOver;

    // Start is called before the first frame update
    void Start()
    {
        PointSet = false;
        KnockedOver = false;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        m_EulerAngleVelocity = new Vector3(500, 0, 0);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

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

        if (time > WaitTime && KnockedOver)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            KnockedOver = false;
        }

    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "player fist" && !KnockedOver)
        {
            time = 0;

            KnockedOver = true;

            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            rb.AddForce(transform.up * thrust);
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            Debug.Log("hit");
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

