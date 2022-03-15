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
    float time;
    public float StopSpeed = 0f, WalkSpeed = 14f;
    public Transform[] points;

    private int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        rb = GetComponent<Rigidbody>();
        Transform parent = GameObject.Find("road points").transform;

        points = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
		{
            points[i] = parent.GetChild(i).transform;
		}

        GotoNextPoint();
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

    void OnTriggerEnter(Collider c)
	{
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "car bumper")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            var thrust = 100;
            var force = transform.position - c.transform.position;

            rb.AddForce(force * thrust);

            Debug.Log("hit");
        }
	}
}

