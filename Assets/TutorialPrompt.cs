using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPrompt : MonoBehaviour
{
    public GameObject prompt;
    public bool closed, open;

    // Start is called before the first frame update
    void Start()
    {
        prompt.SetActive(false);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && open)
		{
			closed = true;
			Time.timeScale = 1;
				;
			prompt.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && !closed)
		{
            prompt.SetActive(true);
			Time.timeScale = 0;
			open = true;
		}
	}
}
