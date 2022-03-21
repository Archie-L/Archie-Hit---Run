using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc_points : MonoBehaviour
{
    public Animator anim;

    private Transform target;
    public GameObject self;
    public Transform player;
    public NavMeshAgent agent;
    Rigidbody rb;
    private State state;
    Vector3 m_EulerAngleVelocity;
    float time;
    public float health;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool KnockedOver, GetUp, Angry;
    public Transform[] points;

    private int destPoint;

    private enum State
    {
        Normal,
        Angry
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        player = GameObject.Find("Player").transform;

        Transform parent = GameObject.Find("points").transform;

        points = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
		{
            points[i] = parent.GetChild(i).transform;
		}

        GotoNextPoint();

        KnockedOver = false;
        GetUp = false;
        Angry = false;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        m_EulerAngleVelocity = new Vector3(500, 0, 0);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                NpcMovement();
                break;
            case State.Angry:
                AngryController();
                break;
        }
    }

    void AngryController()
    {
        agent.Set​​​​​​​​​​​​​​Destination(player.position);

        if (agent.remainingDistance < 1.5f)
		{
            anim.SetTrigger("Attack");
		}
	}

    void NpcMovement()
	{
        time += Time.deltaTime;

        if (time > WaitTime && KnockedOver)
        {
            gameObject.GetComponent<NavMeshAgent > ().enabled = true;

            anim.SetTrigger("getting up");
            GetUp = true;
            KnockedOver = false;

            time = 0f;
        }

        if (time > UpTime && GetUp)
        {
            var aggro = Random.Range(1, 3);

            if (aggro == 1)
            {
                Angry = true;
                Debug.Log("Angry");
            }

            agent.speed = WalkSpeed;
            GetUp = false;

            anim.SetTrigger("walk");
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !Angry)
            GotoNextPoint();

        if (Angry)
        {
            agent.speed = 6f;
			anim.SetTrigger("running");
            state = State.Angry;
        }
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
		if(other.gameObject.tag == "player fist" && !Knocke​dOver && !Angry || other.gameObject.tag == "car bumper" && !KnockedOver && !Angry)
        {
            anim.SetTrigger("knocked");

            time = 0;

            KnockedOver = true;
            GetUp = false;

            agent.speed = StopSpeed;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            var thrust = Random.Range(200, 750);
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            rb.AddForce(transform.up * thrust);

            Debug.Log("hit");
		}
	}
}