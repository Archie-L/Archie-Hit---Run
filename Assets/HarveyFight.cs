using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HarveyFight : MonoBehaviour
{
    public Animator anim;

    private Transform target;
    public GameObject self;
    public Transform selfT;
    public BoxCollider fist;
    public Transform playerT;
    public GameObject player;
    public NavMeshAgent agent;
    private State state;
    float time;
    public float health;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool Angry, Attacking, Parried, Cooldown;

    private enum State
    {
        Waiting,
        Angry,
        Parry,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;

        fist.enabled = !fist.enabled;

        Transform parent = GameObject.Find("points").transform;

        playerT = GameObject.Find("Player").transform;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = StopSpeed;

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
            case State.Waiting:
                Waiting();
                break;
            case State.Angry:
                AngryController();
                break;
            case State.Parry:
                ParriedController();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    void Waiting()
	{
        agent.speed = 0f;

        if (agent.remainingDistance < 10f)
		{
            state = State.Angry;
            anim.SetTrigger("running");
		}

    }

    void ParriedController()
	{
        agent.speed = StopSpeed;

        new WaitForSeconds(4.6f);
        Parried = false;

        if (!Parried)
        {
            state = State.Angry;
        }
    }

    void AngryController()
    {
        if (Parried)
        {
            state = State.Parry;
        }

        agent.speed = WalkSpeed;
        agent.Set​​​​​​​​​​​​​​Destination(playerT.position);

        if(agent.remainingDistance < 2.5f)
		{
            if (!Attacking && !Parried && !Cooldown)
            {
                fist.enabled = fist.enabled;
                agent.speed = 0f;
                anim.SetTrigger("Attack");
                Attacking = true;
                StartCoroutine(Attack());
                StartCoroutine(CooldownWait());
                //StartCoroutine(ParryWait());
            }
		}
    }

    IEnumerator Attack()
	{
        new WaitForSeconds(1f);
        agent.speed = 0f;
        fist.enabled = !fist.enabled;
        Attacking = false;
        Cooldown = true;
        yield break;
    }

    IEnumerator CooldownWait()
	{
        new WaitForSecondsRealtime(4f);
        Cooldown = false;
        yield break;
    }

    IEnumerator ParryWait()
    {

        yield return new WaitForSeconds(0.75f);
        Debug.Log("Parry Now");
        if (player.GetComponent<ThirdPersonMovement>().Parry == true && agent.remainingDistance < 2.5f)
        {
            anim.SetTrigger("Parried");
            Parried = true;
            Debug.Log("Parried");
        }
    }

    void Dead()
	{
        anim.SetTrigger("die");
    }

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "player fist")
        {
            health = health - 1f;
        }
	}
}