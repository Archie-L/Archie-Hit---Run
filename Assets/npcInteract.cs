using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcInteract : MonoBehaviour
{
	public GameObject interact, dialouge;
	public bool storyRelated;
	public GameObject player;
	public float popUpTime;

	// Start is called before the first frame update
	void Start()
    {
		interact.SetActive(false);
		dialouge.SetActive(false);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			interact.SetActive(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				interact.SetActive(false);
				dialouge.SetActive(true);

				StartCoroutine(WaitTime());
			}
		}
	}
	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(popUpTime);
		dialouge.SetActive(false);

		if (storyRelated)
		{
			player.gameObject.GetComponent<ThirdPersonMovement>().storyProgress++;
		}
	}
}
