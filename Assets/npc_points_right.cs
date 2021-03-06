using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc_points_right : MonoBehaviour
{
    public Animator anim;

    private Transform target;
    public GameObject self;
    public Transform selfT;
    public GameObject bat;
    public BoxCollider fist, batCol;
    Transform playerT;
    public GameObject player;
    public NavMeshAgent agent;
    Rigidbody rb;
    private State state;
    Vector3 m_EulerAngleVelocity;
    float time, time2;
    public float health;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool KnockedOver, GetUp, Angry, BatAngry, Attacking, Parried;
    public Transform[] points;
    public GameObject Tpm;
    GameObject spawnNumb;
    public int destPoint;
    public GameObject pointRand;
    public AudioSource fart, hurt, hit1, hit2, promo, npc1, npc2, karen, liquor;
    float prevhealth;

    private enum State
    {
        Normal,
        Angry,
        Weapon,
        Parry,
        Dead,
        Range
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        prevhealth = health;

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
        health = gameObject.GetComponent<health>().NPCHealth;

        if (health != prevhealth)
		{
            Debug.Log(prevhealth + "joe swanson" + health);
            hurt.Play();

            anim.SetTrigger("knocked");

            if (Angry || BatAngry)
            {
                health = health - 1f;
            }
            else
            {
                Tpm.GetComponent<ThirdPersonMovement>().MeterIncrease();

                KnockedOver = true;
                GetUp = false;

                agent.speed = StopSpeed;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;

                var thrust = Random.Range(200, 750);
                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);

                rb.AddForce(transform.up * thrust);
            }

            time = 0;
        }

        if (health <= 0f)
		{
            state = State.Dead;
		}

        switch (state)
        {
            default:
            case State.Normal:
                OutOfRange();
                break;
            case State.Range:
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

        float dist = Vector3.Distance(playerT.position, selfT.position);
        if (dist < 200f)
        {
            state = State.Range;
        }

        prevhealth = health;
    }

    void OutOfRange()
	{
        anim.enabled = false;
        agent.speed = StopSpeed;

        float dist = Vector3.Distance(playerT.position, selfT.position);
        if (dist > 200f)
        {
            state = State.Normal;
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

        var randVoice = Random.Range(1, 3);
        if (randVoice == 1)
        {
            hit1.Play();
        }
        if (randVoice == 2)
        {
            hit2.Play();
        }
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

        var randVoice = Random.Range(1, 3);
        if (randVoice == 1)
        {
            hit1.Play();
        }
        if (randVoice == 2)
        {
            hit2.Play();
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
        time2 += Time.deltaTime;

        anim.enabled = true;
        agent.speed = WalkSpeed;

        if (time > WaitTime && KnockedOver)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;

            anim.SetTrigger("getting up");
            GetUp = true;
            KnockedOver = false;

            time = 0f;
        }

        if (time2 > Random.Range(10, 30))
		{
            time2 = 0f;

            var randVoice = Random.Range(1, 7);
            if (randVoice == 1)
            {
                npc1.Play();
            }
            if (randVoice == 2)
            {
                npc2.Play();
            }
            if (randVoice == 3)
            {
                fart.Play();
            }
            if (randVoice == 4)
            {
                promo.Play();
            }
            if (randVoice == 5)
            {
                liquor.Play();
            }
            if (randVoice == 6)
            {
                karen.Play();
            }
        }

        if (time > UpTime && GetUp)
        {
            var aggro = Random.Range(1, 3);

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

        var forBack = Random.Range(1, 3);
        if (forBack == 1)
        {
            destPoint = (destPoint + 1) % points.Length;
        }
        if (forBack == 2)
        {
            destPoint = (destPoint - 1) % points.Length;
        }
    }

    void Dead()
	{
        anim.SetTrigger("die");
    }
}