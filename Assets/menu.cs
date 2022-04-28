using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public Animator anim;
    public GameObject Disable;

    public void PressPlay()
	{
        Disable.SetActive(false);
        anim.SetTrigger("play");
        StartCoroutine(WaitForAnim());
	}

    IEnumerator WaitForAnim()
	{
        yield return new WaitForSecondsRealtime(3.8f);
        SceneManager.LoadScene("test scene");
	}
}
