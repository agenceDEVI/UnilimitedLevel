using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	public void BackButton()
	{
	FindObjectOfType<AudioManager>().Play("ButtonSound");	
	
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }
}
