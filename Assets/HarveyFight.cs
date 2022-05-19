using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HarveyFight : MonoBehaviour
{
    public Animator anim;
    public GameObject self;
    public Transform selfT;
    public Transform playerT;
    public GameObject player;
    public NavMeshAgent agent;
    public Slider bossBarSlider;
    public GameObject bossBar;
    public AudioSource audioSource, death;
    private State state;
    float time; //this is a float variable that is called time. it does fuck all until it doesnt.
    public float health;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public float coolDown = 4f;
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
        bossBar.SetActive(false);

        state = State.Waiting;

        playerT = GameObject.Find("Player").transform;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = StopSpeed;

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<health>().NPCHealth;

        bossBarSlider.value = health; //this sets the bossbarslider value to the same as the health value, which in this case is a variable. thank you for reading, i've been alex solar pannell, and you've been watching archie legg gaming irl in front of a live studio audience partnered with bbc live.

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
                //StartCoroutine(DeathSound());
                break;
        }
    }

    void Waiting()
	{
        agent.speed = StopSpeed;

        float dist = Vector3.Distance(playerT.position, selfT.position);

        if (dist < 15f)
		{
            audioSource.Play();
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
        time += Time.deltaTime; //this is when time does not do fuck all, but instead does an archie moment

        bossBar.SetActive(true);
        death.Play();

        agent.SetDestination(playerT.position);

        if (Parried)
        {
            state = State.Parry;
        }

        agent.speed = WalkSpeed;

        if(agent.remainingDistance < 2f)
		{
            agent.speed = 0f;

            if (!Attacking && !Parried && !Cooldown)
            {
                if(time > coolDown)
				{
                    time = 0f;
                    agent.speed = 0f;
                    anim.SetTrigger("Attack");
                    Attacking = true;
                    StartCoroutine(Attack());
                    StartCoroutine(CooldownWait());
                    //StartCoroutine(ParryWait());
				}

            }
		}
        if (agent.remainingDistance > 2f)
		{
            agent.speed = WalkSpeed;
        }
    }

    IEnumerator Attack()
	{
        new WaitForSeconds(1f);
        agent.speed = 0f;
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
        agent.speed = StopSpeed;
        audioSource.Stop();
        anim.SetTrigger("die");
        StartCoroutine(DeathWait());
    }

    IEnumerator DeathWait()
	{
        yield return new WaitForSeconds(10);
        Destroy(self.gameObject);
	}
}