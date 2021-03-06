using System.Collections;
using UnityEngine;

public class police_npc : MonoBehaviour
{
    public Transform player;
    public Transform self;
    public Transform barrel;
    public Animator anim;
    public float length;
    public float speed;
    public float speed2;
    public GameObject bullet;
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
        player = GameObject.Find("Player").transform;

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

        var q = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);

        float step = speed2 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);

        if (Wait)
		{
            StartCoroutine(StepWait());
        }
        else if(!Wait)
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
        yield return new WaitForSeconds(length);
        Wait = false;
        length = 0f;
	}

    void Shooting()
	{
        //test.Play();
        Debug.Log("Shooting");
        StartCoroutine(WaitTest());
    }

    IEnumerator WaitTest()
	{
        yield return new WaitForSeconds(1);

        GameObject projectile = Instantiate(bullet, barrel.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);

        length = Random.Range(1, 15);

        state = State.Checking;
    }

    void Arresting()
	{
        Destroy(gameObject);
	}

    void DistCheck()
    {
        //test.Stop();
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