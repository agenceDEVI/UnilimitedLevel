using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
		public GameObject player;
    	public Transform teleporterToGo;
		public CharacterController controller;

	private bool playerIsOverlapping = false;
	
	void Start()
	{
		player = GameObject.Find("Player(Clone)");
		
	}

	void Update()
	{
		player = GameObject.Find("Player(Clone)");
		controller = player.GetComponent(typeof (CharacterController)) as CharacterController;
		
		if(playerIsOverlapping)
		{	

			Vector3 portalToPlayer = player.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
			
			if(dotProduct < 0f)
			{
				controller.enabled = false;
				float rotationDiff = Quaternion.Angle(transform.rotation, teleporterToGo.rotation);
				rotationDiff += 0;
				player.transform.Rotate(Vector3.up, rotationDiff);
				
				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.transform.position = teleporterToGo.position + positionOffset;
				
				controller.enabled = true;
				playerIsOverlapping = false;
				Debug.Log("Teleportation Complete");
			}
			
		}
		
	
	}
	
	void	OnTriggerEnter (Collider other)
	{	
		player = GameObject.Find("Player(Clone)");
		
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
