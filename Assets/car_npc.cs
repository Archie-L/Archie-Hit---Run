using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class car_npc : MonoBehaviour
{
    private Transform target;
    Rigidbody rb;
    public NavMeshAgent agent;
    public GameObject player;
	Vector3 m_EulerAngleVelocity;
    float time;
    public float StopSpeed = 0f, WalkSpeed = 14f;
    public Transform[] points;

    private int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_EulerAngleVelocity = new Vector3(500, 0, 0);

        Transform parent = GameObject.Find("road points").transform;

        points = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
		{
            points[i] = parent.GetChild(i).transform;
		}

        GotoNextPoint();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
        {
            agent.speed = StopSpeed;
		}

        if(other.gameObject.tag == "car bumper")
		{
            float thrust = 400f;
             
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            rb.AddForce(transform.up * thrust);
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            Debug.Log("hit");
        }
	}
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            agent.speed = WalkSpeed;
        }
    }
}

