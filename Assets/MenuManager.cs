using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string newGameLevel;
    public Animator anim;
    public GameObject Disable;

    public void NewGameButton()
	{
		Disable.SetActive(false);
		anim.SetTrigger("play");
		StartCoroutine(WaitForAnim());

		PlayerPrefs.SetInt("loadGame", 0);
	}

	IEnumerator WaitForAnim()
	{
		yield return new WaitForSecondsRealtime(3.8f);

		SceneManager.LoadScene(newGameLevel);
	}

	public void LoadGameButton()
	{
		if (PlayerPrefs.HasKey("LevelSaved"))
		{
			Disable.SetActive(false);
			anim.SetTrigger("play");
			StartCoroutine(WaitForAnim2());

			PlayerPrefs.SetInt("loadGame", 1);
		}

		IEnumerator WaitForAnim2()
		{
			yield return new WaitForSecondsRealtime(3.8f);

			string levelToLoad = PlayerPrefs.GetString("LevelSaved");
			SceneManager.LoadScene(levelToLoad);
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
