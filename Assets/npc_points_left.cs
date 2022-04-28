using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc_points_left : MonoBehaviour
{
    public Animator anim;

    private Transform target;
    public GameObject self;
    public GameObject bat;
    public BoxCollider fist, batCol;
    public Transform playerT;
    public GameObject player;
    public NavMeshAgent agent;
    Rigidbody rb;
    private State state;
    Vector3 m_EulerAngleVelocity;
    float time;
    public float health;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool KnockedOver, GetUp, Angry, BatAngry, Attacking, Parried;
    public Transform[] points;
    public GameObject Tpm;
    GameObject spawnNumb;
    public int destPoint;
    public GameObject pointRand;

    private enum State
    {
        Normal,
        Angry,
        Weapon,
        Parry,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        pointRand = GameObject.Find("points");

        fist.enabled = !fist.enabled;
        batCol.enabled = !batCol.enabled;

        destPoint = pointRand.GetComponent<points>().randomNumb;
        

        Transform parent = GameObject.Find("points").transform;

        playerT = GameObject.Find("Player").transform;
        player = GameObject.Find("Player");
        Tpm = GameObject.Find("Player");

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
		if (health <= 0f)
		{
            state = State.Dead;
		}

        switch (state)
        {
            default:
            case State.Normal:
                NpcMovement();
                break;
            case State.Angry:
                AngryController();
                break;
            case State.Weapon:
                BatController();
                break;
            case State.Parry:
                ParriedController();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    void ParriedController()
	{
        agent.speed = 0f;

        new WaitForSeconds(4.6f);
        Parried = false;

        if (Angry && !Parried)
        {
            state = State.Angry;
        }

        if (BatAngry && !Parried)
        {
            state = State.Weapon;
        }
    }

    void AngryController()
    {
        if (Parried)
        {
            state = State.Parry;
        }

        agent.speed = 6f;
        agent.Set​​​​​​​​​​​​​​Destination(playerT.position);

        if (agent.remainingDistance < 1.5f && !Attacking || agent.remainingDistance < 1.5f && !Parried)
        {
            fist.enabled = !fist.enabled;
            agent.speed = 0f;
            anim.SetTrigger("Attack");
            StartCoroutine(Attack());
            Attacking = true;
        }
    }

    IEnumerator Attack()
	{
        yield return new WaitForSeconds(1f);
        agent.speed = 0f;
        fist.enabled = !fist.enabled;
        Attacking = false;
    }

    void BatController()
    {
		if (Parried)
		{
            state = State.Parry;
		}

        bat.SetActive(true);

        agent.speed = 6f;
        agent.Set​​​​​​​​​​​​​​Destination(playerT.position);

        if (agent.remainingDistance < 1.5f && !Attacking || agent.remainingDistance < 1.5f && !Parried)
        {
            batCol.enabled = !batCol.enabled;
            agent.speed = 0f;
            anim.SetTrigger("Battack");
            StartCoroutine(Battack());
            StartCoroutine(ParryWait());
            Attacking = true;
        }
    }

    IEnumerator Battack()
    {
        yield return new WaitForSeconds(2.5f);
		if (!Parried)
        {
            agent.speed = 6f;
            batCol.enabled = !batCol.enabled;
            Attacking = false;
        }
    }

    IEnumerator ParryWait()
	{
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Parry Now");
        if (player.GetComponent<ThirdPersonMovement>().Parry == true)
        {
            if (Angry)
            {
                fist.enabled = !fist.enabled;
            }

            if (BatAngry)
			{
                batCol.enabled = !batCol.enabled;
            }
            anim.SetTrigger("Parried");
            Parried = true;
            Debug.Log("Parried");
        }
    }

    public void NpcMovement()
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
            /*var aggro = Random.Range(1, 3);

            if (aggro == 1)
            {
                var HasBat = Random.Range(1, 3);

                if (HasBat == 1)
				{
                    BatAngry = true;
                }
				else
				{
                    Angry = true;
                    Debug.Log("Angry");
                }
            }*/

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

        if (BatAngry)
        {
            agent.speed = 6f;
            anim.SetTrigger("running");
            state = State.Weapon;
        }
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        destPoint = (destPoint - 1) % -points.Length;
    }

    void Dead()
	{
        anim.SetTrigger("die");
    }

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "player fist" && !Knocke​dOver|| other.gameObject.tag == "car bumper" && !KnockedOver)
        {
            anim.SetTrigger("knocked");

            if(Angry || BatAngry)
			{
                health = health - 10f;
			}
			else
			{
                Tpm.GetComponent<ThirdPersonMovement>().MeterIncrease();
            }

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