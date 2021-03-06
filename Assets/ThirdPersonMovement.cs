using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public Transform cam;
    public Slider mSlider, pSlider;
    public Transform self;
    public bool Blocking, Parry;
    public float speed = 6f;
    public float health = 10f;
    public float turnSmoothTime = 0.1f;
    float time;
    public float crimeMeter;
    public float maxMeter;
    public float WaitTime = 1.05f, ClobberingTime = 0.525f;
    float turnSmoothVelocity;
    bool Clobbering;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    Vector3 moveVector;
    private State state;
    private Vector3 moveDirection = Vector3.zero;
    public GameObject police;
    public Vector3 spawnPoint;
    public float spawnPointRange;
    float seconds = 10f;
    bool spawned, wanted;
    public bool rolling;
    public int storyProgress;
    public int effigyNumb;
    public int loadGame;

    public Transform[] effigies;

    private enum State
    {
        Normal,
        Wanted,
        Dead
    }

    void Start()
	{
        mSlider.maxValue = 50f;
        pSlider.maxValue = maxMeter;

        state = State.Normal;

        Cursor.lockState = CursorLockMode.Locked;

        Clobbering = false;

        loadGame = PlayerPrefs.GetInt("loadGame");
        if(loadGame == 1)
        {
            Respawn();
        }
    }

	// Update is called once per frame
	void Update()
    {
        health = gameObject.GetComponent<health>().NPCHealth;

        if (rolling || Blocking)
        {
            this.gameObject.tag = "car bumper";
        }
        else
        {
            this.gameObject.tag = "Player";
        }

        mSlider.value = health;
        pSlider.value = crimeMeter;

        if (health <= 0f)
        {
            state = State.Dead;
        }

        if (crimeMeter >= maxMeter)
		{
            state = State.Wanted;
            wanted = true;
            crimeMeter = 100f;
		}

        switch (state)
        {
            default:
            case State.Normal:
                CharacterMovement();
                break;
            case State.Wanted:
                CharacterMovement();
                WantedController();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    void Respawn()
    {
        effigyNumb = PlayerPrefs.GetInt("effigyNumb");
        storyProgress = PlayerPrefs.GetInt("storyProgress");

        transform.position = effigies[effigyNumb - 1].position;
    }

    void CharacterMovement()
	{
        time += Time.deltaTime;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        moveVector = Vector3.zero;

        if (controller.isGrounded == false)
        {
            moveVector += Physics.gravity;
        }

		if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("roll");
		}

        controller.Move(moveVector * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (time > WaitTime && Clobbering)
        {
            time = 0;
            speed = 6f;
            Clobbering = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Clobbering = true;
            time = 0;
            speed = 0f;

            anim.SetTrigger("punch");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
		{
            Blocking = true;

            StopWalk();

            time = 0;
            speed = 0f;
            anim.SetBool("blocking", true);

            new WaitForSeconds(0.2f);
            Debug.Log("Parry");
            Parry = true;
            StartCoroutine(ParryTime());
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Blocking = false;

            speed = 6f;
            anim.SetBool("blocking", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Clobbering == false && !Blocking)
        {
            anim.SetBool("running", true);
            speed = 12f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && Clobbering == false && !Blocking)
        {
            anim.SetBool("running", false);
            speed = 6f;
        }

        if (Input.GetKeyDown(KeyCode.W) && !Blocking || Input.GetKeyDown(KeyCode.A) && !Blocking || Input.GetKeyDown(KeyCode.S) && !Blocking || Input.GetKeyDown(KeyCode.D) && !Blocking)
        {
            Walk();
        }

        if (Input.GetKeyUp(KeyCode.W) && !Blocking)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
            else
            {
                StopWalk();
                anim.SetBool("running", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.A) && !Blocking)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
            else
            {
                StopWalk();
                anim.SetBool("running", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.S) && !Blocking)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
            else
            {
                StopWalk();
                anim.SetBool("running", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.D) && !Blocking)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
            {
                Walk();
            }
            else
            {
                StopWalk();
                anim.SetBool("running", false);
            }
        }
    }

    IEnumerator ParryTime()
	{
        new WaitForSeconds(0.2f);
        Parry = false;
        yield break;

    }

    void WantedController()
	{
        float randomZ = Random.Range(-spawnPointRange, spawnPointRange);
        float randomX = Random.Range(-spawnPointRange, spawnPointRange);

        spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

		if (!spawned)
        {
            spawned = true;
            StartCoroutine(PoliceSpawnTime());
        }
    }

    IEnumerator PoliceSpawnTime()
    {
        yield return new WaitForSeconds(seconds);
        spawned = false;
        Instantiate(police, spawnPoint, Quaternion.identity);
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "npc fist")
		{
            if (!Blocking || !rolling)
            {
                health--;
            }
        }

        if (other.gameObject.tag == "bat")
        {
			if (!Blocking || !rolling)
            {
                health = health - 2f;
            }
        }
    }

    void Dead()
    {
        anim.SetTrigger("die");
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("test scene");
    }

	void Walk()
	{
        anim.SetBool("trigger", true);
	}

    void StopWalk()
	{
        anim.SetBool("trigger", false);
    }

    public void MeterIncrease()
	{
        crimeMeter = crimeMeter + 10f;
	}
}