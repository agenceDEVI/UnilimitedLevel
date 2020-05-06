using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundEventRandom : MonoBehaviour
{
	public GameObject player;
	
	private bool playerIsOverlapping = false;
	int i = 0;
	
	void Start()
	{
		player = GameObject.Find("Player(Clone)");
		
	}
	
	void Update()
	{
		player = GameObject.Find("Player(Clone)");
		
	}
		
		void	OnTriggerEnter (Collider other)
	{	

		i = Random.Range(1,20);
		
		if(i == 1)
		{
			FindObjectOfType<AudioManager>().Play("DistantLoudStep");	
		}
		if(i == 2)
		{
			FindObjectOfType<AudioManager>().Play("DistantFootstep");	
		}	
		if(i == 3 || i == 4)
		{
			FindObjectOfType<AudioManager>().Play("BigKnock");	
		}	
		if(i == 5)
		{
			FindObjectOfType<AudioManager>().Play("DistantDoor");	
		}
		if(i == 6)
		{
			FindObjectOfType<AudioManager>().Play("Whispers");	
		}
		
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}
	

	void OnTriggerExit (Collider other)
	{
		if(other.tag == "Player")
		{
		playerIsOverlapping = false;
		}
	}
	
}
