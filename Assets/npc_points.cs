using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc_points : MonoBehaviour
{
    public Animator anim;

    private Transform target;
    public GameObject self;
    public GameObject player;
    public NavMeshAgent agent;
    Rigidbody rb;
	Vector3 m_EulerAngleVelocity;
    float time;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool KnockedOver, GetUp;
    public Transform[] points;

    private int destPoint;

    [HideInInspector] public int spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        Transform parent = GameObject.Find("points").transform;

        points = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
		{
            points[i] = parent.GetChild(i).transform;
		}

        destPoint = spawnPos + 1;

        GotoNextPoint();

        KnockedOver = false;
        GetUp = false;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        m_EulerAngleVelocity = new Vector3(500, 0, 0);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > WaitTime && KnockedOver)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;

            anim.SetTrigger("getting up");
            GetUp = true;
            KnockedOver = false;

            time = 0f;
        }

        if (time > UpTime && GetUp)
        {
            agent.speed = WalkSpeed;
            GetUp = false;

            anim.SetTrigger("walk");
        }

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
		if(other.gameObject.tag == "player fist" && !KnockedOver || other.gameObject.tag == "car bumper" && !KnockedOver)
        {
            float thrust = Random.Range(200, 750);

            anim.SetTrigger("knocked");

            time = 0;

            KnockedOver = true;
            GetUp = false;

            agent.speed = StopSpeed;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            rb.AddForce(transform.up * thrust);
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            Debug.Log("hit");
		}
	}
}

