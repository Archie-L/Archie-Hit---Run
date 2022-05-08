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
    Rigidbody rb;
    private State state;
    float time;
    public float health;
    public float WaitTime = 5f, UpTime = 8.5f;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public bool Angry, Attacking, Parried;
    public GameObject Tpm;

    private enum State
    {
        Angry,
        Weapon,
        Parry,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Angry;

        fist.enabled = !fist.enabled;
        

        Transform parent = GameObject.Find("points").transform;

        playerT = GameObject.Find("Player").transform;
        player = GameObject.Find("Player");
        Tpm = GameObject.Find("Player");

        Angry = false;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

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

    void ParriedController()
	{
        agent.speed = 0f;

        new WaitForSeconds(4.6f);
        Parried = false;

        if (Angry && !Parried)
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
            health = health - 10f;
        }
	}
}