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
    public BoxCollider fist;
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
    public float coolDown = 4f;
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

        fist.enabled = false;

        playerT = GameObject.Find("Player").transform;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = StopSpeed;

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (dist < 15f)
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

        if (agent.remainingDistance < 2f)
        {
            agent.speed = 0f;

            if (!Attacking && !Cooldown)
            {
                if (time > coolDown)
                {
                    time = 0f;
                    fist.enabled = true;
                    agent.speed = 0f;
                    anim.SetTrigger("Attack");
                    Attacking = true;
                    StartCoroutine(Attack());
                    StartCoroutine(CooldownWait());
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
        fist.enabled = false;
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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered trigger");

        if (other.gameObject.tag == "player fist" || other.gameObject.tag == "car bumper")
        {
            Debug.Log("damage");
            health--;
        }
    }
}