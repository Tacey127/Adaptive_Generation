using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTester : MonoBehaviour {
	[SerializeField] GameObject RoomTest;
	List <Room> RoomsToTest = new List<Room>();

	void Start()
	{
		//The room that already exists
		GameObject roomObject = Instantiate (RoomTest, transform);
		//Add room to list
		Room A = roomObject.GetComponent<Room> ();
		Debug.Log (A.roomCollider.bounds);

		Room B = RoomTest. GetComponent<Room> ();
		Debug.Log (B.roomCollider.bounds);

		//if (B.RoomBounds.Intersects(B.RoomBounds))
			Debug.Log ("AAAAAAAAAA");
	}

	void NonStart()
	{
		GameObject roomObject = Instantiate (RoomTest, transform);
		//Add room to list
		Room A = roomObject.GetComponent<Room> ();
		RoomsToTest.Add (A);

		SpawnFromList (A, false, true); 

	}

	bool SpawnFromList(Room room, bool spawnNormalRoom, bool isTrunk)
	{

		//picks an available Door from the Room
		Door LocalDoor = room.GetAvailableDoor ();
		//Gets the position and rotation of the this rooms chosen door
		Vector3 roomPos = LocalDoor.transform.position;
		roomPos.y -= 1.75f;
		Quaternion startRotation = LocalDoor.transform.rotation;
		LocalDoor.gameObject.SetActive (false);

		//Choose a room from the spawnable list and creates it
		GameObject newRoom = RoomTest;
		//clone room
		newRoom = Instantiate (newRoom, roomPos, Quaternion.identity, transform);
		//Pick a entry from the hallway to connect to
		Door AChosenDoor = newRoom.GetComponent<Room> ().GetAvailableDoor ();
		//rotate the newRoom to face the room 
		Quaternion altRotation = AChosenDoor.transform.rotation;
		newRoom.transform.Rotate (0, (startRotation.eulerAngles.y + 180) - altRotation.eulerAngles.y, 0);
		//move the newRoom into position
		roomPos = AChosenDoor.transform.localPosition;
		roomPos.y -= 1.75f;
		newRoom.transform.Translate (-roomPos);

		Room r = newRoom.GetComponent<Room> ();
		r.isTrunk = isTrunk;
		r.endOfLine = room.endOfLine;

		if (!spawnNormalRoom) { //if the current room spawned is a corridor, spawn a room next to it
			SpawnFromList (r, true, isTrunk);
			r.spawnedNeighbours = true;
		}

		AChosenDoor.gameObject.SetActive (false);
		return true;
	}

}
