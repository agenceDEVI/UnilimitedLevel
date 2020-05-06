using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;

	void Start () {
		
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		GameIsPaused = false;
		
        pauseMenuUI.SetActive(false);
 	}
	
    // Update is called once per frame
    void Update()
    {	
		
        if (Input.GetKeyDown(KeyCode.Escape)) 
	{	
		if (GameIsPaused)
		{
			Resume();
		}
		else
		{
		Pause();
		}
	}
    }

	public void Resume()
	{
		FindObjectOfType<AudioManager>().Play("ButtonSound");
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		GameIsPaused = false;
	}
	
	void Pause() 
	{
	
	pauseMenuUI.SetActive(true);
	Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
	Time.timeScale = 0f;
	GameIsPaused = true;
	
	}

	public void LoadMenu()
	{
		FindObjectOfType<AudioManager>().Play("ButtonSound");
		FindObjectOfType<AudioManager>().StopPlaying("MainMusic");
		FindObjectOfType<AudioManager>().Play("MenuMusic");
		Time.timeScale = 1f;
		
		SceneManager.LoadScene("Main Menu");
	}
	
	public void QuitGame()
	{
		FindObjectOfType<AudioManager>().Play("ButtonSound");
		
		Debug.Log("Quitting game...");
		Application.Quit();
	}
	
	public bool GamePaused ()
	{
		return !GameIsPaused;
	}
}
