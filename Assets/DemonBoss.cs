using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DemonBoss : MonoBehaviour
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
    float time;
    public float health;
    public float StopSpeed = 0f, WalkSpeed = 1.5f;
    public float coolDown = 3f;
    public bool Angry, Attacking, Cooldown;

    private enum State
    {
        Waiting,
        Angry,
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

        bossBarSlider.value = health;

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

        if (dist < 30f)
        {
            audioSource.Play();
            state = State.Angry;
            anim.SetTrigger("running");
        }

    }

    void AngryController()
    {
        time += Time.deltaTime;

        bossBar.SetActive(true);
        death.Play();

        agent.SetDestination(playerT.position);

        agent.speed = WalkSpeed;

        if (agent.remainingDistance < 15f)
        {
            agent.speed = StopSpeed;

            if (!Attacking && !Cooldown)
            {
                if (time > coolDown)
                {
                    int randNumb = Random.Range(1, 3);
                    if(randNumb == 1)
                    {
                        anim.SetTrigger("attack 1");
                    }
                    if (randNumb == 2)
                    {
                        anim.SetTrigger("attack 2");
                    }

                    time = 0f;
                    agent.speed = StopSpeed;
                    Attacking = true;
                    StartCoroutine(Attack());
                    StartCoroutine(CooldownWait());
                }

            }
        }
        if (agent.remainingDistance > 15f)
        {
            agent.speed = WalkSpeed;
        }
    }

    IEnumerator Attack()
    {
        new WaitForSeconds(2.13333333f);
        agent.speed = 0f;
        Attacking = false;
        Cooldown = true;
        yield break;
    }

    IEnumerator CooldownWait()
    {
        new WaitForSecondsRealtime(coolDown);
        Cooldown = false;
        yield break;
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