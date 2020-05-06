using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	void Start()
    	{
	FindObjectOfType<AudioManager>().Play("MenuMusic");
	Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
	}
    public void PlayGame () 
 {
  FindObjectOfType<AudioManager>().Play("ButtonSound");
  SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
  FindObjectOfType<AudioManager>().StopPlaying("MenuMusic");
  FindObjectOfType<AudioManager>().Play("MainMusic");
 }
 
 public void QuitGame ()
 {
  FindObjectOfType<AudioManager>().Play("ButtonSound");
  Debug.Log ("QUIT!");
  Application.Quit();
 }
}
