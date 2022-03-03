using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator anim;

    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float time;
    public float WaitTime = 1.05f;
    float turnSmoothVelocity;

	void Start()
	{

    }

	// Update is called once per frame
	void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
		{
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
		}

        time += Time.deltaTime;

        if (time > WaitTime)
        {
            time = 0;
            speed = 6f;
        }

        if (Input.GetButtonDown("Fire1"))
		{
            anim.SetTrigger("punch");

            time = 0;
            speed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
		{
            Walk();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{
                Walk();
            }
			else
			{
                StopWalk();
			}
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{
                Walk();
			}
            else
            {
                StopWalk();
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
            else
            {
                StopWalk();
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
            {
                Walk();
            }
            else
            {
                StopWalk();
            }
        }
    }

    void Walk()
	{
        anim.SetBool("trigger", true);
	}

    void StopWalk()
	{
        anim.SetBool("trigger", false);
    }
}