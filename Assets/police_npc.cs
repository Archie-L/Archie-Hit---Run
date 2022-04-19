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
    private State state;
    public ParticleSystem test;

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

        if (length > 0)
		{
            StartCoroutine(StepWait());
        }
        else if(length <= 0)
		{
            anim.SetBool("walking", false);

            state = State.Shooting;
		}

    }

    IEnumerator StepWait()
	{
        new WaitForSeconds(1);
        length--;
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
        new WaitForSeconds(10);
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