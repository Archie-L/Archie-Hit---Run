using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GodjamesBoss : MonoBehaviour
{
	public NavMeshAgent agent;
	public Transform player;
	public Animator anim;

    // Update is called once per frame
    void Start()
    {
		agent.speed = 0f;
		StartCoroutine(Intro());
	}

	IEnumerator Intro()
	{
		new WaitForSecondsRealtime(2.333f);
		agent.speed = 1f;
		anim.SetBool("Walk", true);
		yield break;
	}

	private void Update()
	{
		agent.SetDestination(player.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			anim.SetBool("Walk", false);
			agent.speed = 0f;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			anim.SetBool("Walk", true);
			agent.speed = 1f;
		}
	}
}
