using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
	public GameObject txt;
	public Animator anim;
	public ThirdPersonMovement tpm;
	public int effigyNumb;
	public int storyProgress;

	private void Start()
	{
		txt.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			txt.SetActive(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				txt.SetActive(false);
				tpm.speed = 0f;

				anim.SetTrigger("pray");

				storyProgress = GameObject.Find("Player").gameObject.GetComponent<ThirdPersonMovement>().storyProgress;

				string activeScene = SceneManager.GetActiveScene().name;
				PlayerPrefs.SetString("LevelSaved", activeScene);
				PlayerPrefs.SetInt("effigyNumb", effigyNumb);
				PlayerPrefs.SetInt("storyProgress", storyProgress);
				PlayerPrefs.Save();

				Debug.Log(activeScene);
				StartCoroutine(WaitTime());
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			txt.SetActive(false);
		}
	}

	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(5.233f);
		tpm.speed = 6f;
	}
}
