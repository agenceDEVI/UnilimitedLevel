using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
	void Start()
    	{
	
	//Cursor.lockState = CursorLockMode.Confined;
	Cursor.lockState = CursorLockMode.None;
	Cursor.visible = true;
	}
    public void Continue () 
 {
  FindObjectOfType<AudioManager>().Play("ButtonSound");	
  SceneManager.LoadScene("Main Menu");
 }
}