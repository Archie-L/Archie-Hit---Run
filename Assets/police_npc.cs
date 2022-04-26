using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class police_npc : MonoBehaviour
{
    public Transform player;
    public Transform self;
    public Animator anim;
    public float length;
    public float speed;
    public float speed2;
    private State state;
    public ParticleSystem test;
    public bool Wait;

    private enum State
	{
        Checking,
        Shooting,
        Walking,
        Dead,
        Arresting
	}

    // Start is called before the first frame update
    void Start()
    {
        test.Stop();

        state = State.Walking;

        length = Random.Range(1, 15);
    }

    // Update is called once per frame
    void Update()
    {
		switch (state)
		{
            default:
            case State.Checking:
                DistCheck();
                break;
            case State.Shooting:
                Shooting();
                break;
            case State.Walking:
                Walking();
                break;
            case State.Dead:
                break;
            case State.Arresting:
                Arresting();
                break;
        }
    }

    void Walking()
    {
        anim.SetBool("walking", true);
        Debug.Log(length);

        var q = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);

        float step = speed2 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);

        if (length > 0 && Wait == true)
		{
            StartCoroutine(StepWait());
        }
        else if(length <= 0 && Wait == false)
		{
            anim.SetBool("walking", false);

            state = State.Shooting;

            Wait = true;
        }


        float dist = Vector3.Distance(player.position, self.position);
        if (dist < 1.5f)
        {
            state = State.Arresting;
        }
    }

    IEnumerator StepWait()
    {
        new WaitForSecondsRealtime(length);
        Wait = false;
        length = 0f;
        yield break;
	}

    void Shooting()
	{
        test.Play();
        Debug.Log("Shooting");
        StartCoroutine(WaitTest());
    }

    IEnumerator WaitTest()
	{
        new WaitForSecondsRealtime(10);
        length = Random.Range(1, 15);
        state = State.Checking;
        yield break;
    }

    void Arresting()
	{
        Destroy(gameObject);
	}

    void DistCheck()
    {
        test.Stop();
        Debug.Log("Stopped Shooting");

        float dist = Vector3.Distance(player.position, self.position);

        if (dist < 1.5f)
		{
            state = State.Arresting;
		}
		else
		{
            state = State.Walking;
		}
    }
}