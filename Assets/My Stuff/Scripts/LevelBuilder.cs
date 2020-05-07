using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
	public Room startRoomPrefab, endRoomPrefab;
	public List<Room> roomPrefabs = new List<Room> ();
	public Vector2 iterationRange = new Vector2 (3, 10);
	public Player playerPrefab;
	public WanderIA wanderIA;
	public GameObject LoadingScreen;
	public GameObject LoadingCamera;
	public NavMeshSurface surface;

	List<Doorway> availableDoorways = new List<Doorway> ();

	StartRoom startRoom;
	EndRoom endRoom;
	List<Room> placedRooms = new List<Room> ();

	public LayerMask roomLayerMask;

	Player player;
	WanderIA ennemy;

	void Start ()
	{
		LoadingScreen.SetActive(true);
		LoadingCamera.SetActive(true);
		roomLayerMask = LayerMask.GetMask ("Room");
		StartCoroutine ("GenerateLevel");
	}

	
	IEnumerator GenerateLevel ()
	{
		WaitForSeconds startup = new WaitForSeconds (1);
		WaitForFixedUpdate interval = new WaitForFixedUpdate ();

		yield return startup;

		// Place start room
		PlaceStartRoom ();
		yield return interval;

		// Random iterations
		int iterations = Random.Range ((int)iterationRange.x, (int)iterationRange.y);

		for (int i = 0; i < iterations; i++) {
			// Place random room from list
			PlaceRoom ();
			yield return interval;
		}

		// Place end room
		PlaceEndRoom ();
		yield return interval;

		// Level generation finished
		Debug.Log ("Level generation finished");

		// Place player
		
		player = Instantiate (playerPrefab) as Player;
		player.controller.enabled = false;
		player.transform.position = startRoom.playerStart.position;
		player.transform.rotation = startRoom.playerStart.rotation;
		player.controller.enabled = true;
		Debug.Log ("Player Placed");

		//Place Ennemy 
		ennemy = Instantiate (wanderIA) as WanderIA;
		
		ennemy.transform.position = endRoom.iaSpawn.position;
		ennemy.transform.rotation = endRoom.iaSpawn.rotation;

		// Build NavMesh
		surface.BuildNavMesh(); 
		
		
		LoadingCamera.SetActive(false);
		LoadingScreen.SetActive(false);
//		yield return new WaitForSeconds (3);
//		ResetLevelGenerator ();
	}

	void PlaceStartRoom ()
	{
		// Instantiate room
		startRoom = Instantiate (startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList (startRoom, ref availableDoorways);

		// Position room
		startRoom.transform.position = Vector3.zero;
		startRoom.transform.rotation = Quaternion.identity;
		Bounds startbounds = startRoom.RoomBounds;
		
	}

	void AddDoorwaysToList (Room room, ref List<Doorway> list)
	{
		foreach (Doorway doorway in room.doorways) {
			int r = Random.Range (0, list.Count);
			list.Insert (r, doorway);
		}
	}

	void PlaceRoom ()
	{
		// Instantiate room
		Room currentRoom = Instantiate (roomPrefabs [Random.Range (0, roomPrefabs.Count)]) as Room;
		currentRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway> (availableDoorways);
		List<Doorway> currentRoomDoorways = new List<Doorway> ();
		AddDoorwaysToList (currentRoom, ref currentRoomDoorways);

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList (currentRoom, ref availableDoorways);

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways) 
		{
			// Try all available doorways in current room
			foreach (Doorway currentDoorway in currentRoomDoorways) 
				{
				// Position room
				PositionRoomAtDoorway (ref currentRoom, currentDoorway, availableDoorway);

				// Check room overlaps
				if (CheckRoomOverlap (currentRoom) == true) {
					continue;
				}

				roomPlaced = true;

				// Add room to list
				placedRooms.Add (currentRoom);

				// Remove occupied doorways
				currentDoorway.gameObject.SetActive (false);
				availableDoorways.Remove (currentDoorway);

				availableDoorway.gameObject.SetActive (false);
				availableDoorways.Remove (availableDoorway);

				// Exit loop if room has been placed
				break;

			}

			// Exit loop if room has been placed
			if (roomPlaced) {
				break;
			}
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced) {
			Destroy (currentRoom.gameObject);
			Debug.Log ("Reset");
			ResetLevelGenerator ();
		}
	}

	void PositionRoomAtDoorway (ref Room room, Doorway roomDoorway, Doorway targetDoorway)
	{
		
		// Reset room position and rotation
		room.transform.position = Vector3.zero;
		room.transform.rotation = Quaternion.identity;

		// Rotate room to match previous doorway orientation
		Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
		Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
		float deltaAngle = Mathf.DeltaAngle (roomDoorwayEuler.y, targetDoorwayEuler.y);
		Quaternion currentRoomTargetRotation = Quaternion.AngleAxis (deltaAngle, Vector3.up);
		room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler (0, 180f, 0);

		// Position room
		Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
		room.transform.position = targetDoorway.transform.position - roomPositionOffset;
	}

	bool CheckRoomOverlap (Room room)
	{
			
		Bounds bounds = room.RoomBounds;
			
		bounds.Expand (-0.1f);
		bounds.center = room.transform.position; 
			
			
		Collider[] colliders = Physics.OverlapBox (bounds.center, bounds.size / 2, room.transform.rotation, roomLayerMask);
		
			
	

	
		if (colliders.Length > 0) {
			// Ignore collisions with current room
			foreach (Collider c in colliders) {
				if (c.transform.parent.gameObject.Equals (room.gameObject)) {
					Debug.Log ("Overlap not detected");
					continue;
				} else {
					Debug.Log ("Overlap detected");
					return true;
				}
			}
		}

		return false;
	}

	void PlaceEndRoom ()
	{
		// Instantiate room
		endRoom = Instantiate (endRoomPrefab) as EndRoom;
		endRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway> (availableDoorways);
		Doorway doorway = endRoom.doorways [0];

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways) {
			// Position room
			Room room = (Room)endRoom;
			PositionRoomAtDoorway (ref room, doorway, availableDoorway);

			// Check room overlaps
			if (CheckRoomOverlap (endRoom)) {
				continue;
			}

			roomPlaced = true;

			// Remove occupied doorways
			doorway.gameObject.SetActive (false);
			availableDoorways.Remove (doorway);

			availableDoorway.gameObject.SetActive (false);
			availableDoorways.Remove (availableDoorway);

			// Exit loop if room has been placed
			break;
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced) {
			ResetLevelGenerator ();
		}
	}

	void ResetLevelGenerator ()
	{
		Debug.Log ("Reset level generator");

		StopCoroutine ("GenerateLevel");

		// Delete all rooms
		if (startRoom) {
			Destroy (startRoom.gameObject);
		}

		if (endRoom) {
			Destroy (endRoom.gameObject);
		}

		foreach (Room room in placedRooms) {
			Destroy (room.gameObject);
		}

		// Clear lists
		placedRooms.Clear ();
		availableDoorways.Clear ();

		// Reset coroutine
		StartCoroutine ("GenerateLevel");
	}
}
