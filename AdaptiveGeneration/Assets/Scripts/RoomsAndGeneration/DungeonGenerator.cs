using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DungeonGenerator : MonoBehaviour {

    //Variables that can change
    //How big is the dungon from Root to stem

    [SerializeField] int seed = 0;
	[SerializeField] int dungeonSize = 3;
	[SerializeField] int minDungeonSize = 2;
	[SerializeField] int leniancy = 1;//how much tolerance the dungeon can take for players taking too long to finish a room before shrinking

	int roomsSpawned = 1;

	int roomRemovable = 0;
	public int reduction = 0;

	[SerializeField] bool endRoomSpawned = false;
	[SerializeField] bool spawningEnd = false;
	//Different rooms
	[SerializeField] GameObject[] startRooms;
	[SerializeField] GameObject[] rooms;
	[SerializeField] GameObject[] hallways;

	[SerializeField] GameObject[] minimumRooms;
	[SerializeField] GameObject[] minimumHallways;

	[SerializeField] GameObject[] endRooms;

	//collision
    [SerializeField] List<Bounds> generatedBounds = new List<Bounds>();

    [SerializeField] bool appliesShrinkage = false;

	//
	void Start()
	{
        //Random.InitState (seed);
        seed = (int)(Random.value * 10000);
		StartGeneration ();
	}

	void StartGeneration()
	{
		if (startRooms.Length != 0) 
		{
			roomRemovable = dungeonSize - minDungeonSize;
			SpawnStartRoom ();

		}
	}


	//==================================================================Generation

	GameObject ChooseListObject(GameObject[] rl)
	{
        int chosenRoom = (int)Mathf.Floor(Random.value * rl.Length);

		return rl [chosenRoom];
	}


	void SpawnStartRoom()
	{
		//Room Spawns
		GameObject roomObject = Instantiate (ChooseListObject (startRooms), transform);
		//Add room to list
		Room r = roomObject.GetComponent<Room> ();
		r.parentGenerator = this;
		r.isTrunk = true;
		r.isEncounter = false;
		r.endOfLine = dungeonSize;
		generatedBounds.Add(r.roomCollider.bounds);
		r.roomCollider.isTrigger = true;
	}

	//Spawns either the base room or a hallway next to the calling Room
	public bool SpawnFromList(Room room, bool spawnNormalRoom, bool isTrunk, bool useMinimumRooms)
	{
		//Check if what is being made is a hallway or a room
		GameObject[] spawnList;


		if (isTrunk) {
			Debug.Log (isTrunk);
		}
	
		if (room.endOfLine <= 0 && isTrunk && !endRoomSpawned) {
			spawnList = endRooms;
			Debug.Log ("finish", room);
			spawningEnd = true;
            room.endOfLine = 0;
		} 
		else 
		{
			if (spawnNormalRoom) {
				spawnList = rooms;
				if (useMinimumRooms) {
					spawnList = minimumRooms;
				}
			} else {
				spawnList = hallways;
				if (useMinimumRooms) {
					spawnList = minimumHallways;
				}

				if ((reduction > 0) && (leniancy > 0)) {
					int r = reduction;
					reduction = reduction - leniancy;
					leniancy -= r;
				}

                if (appliesShrinkage)
                {
                    room.endOfLine = room.endOfLine - 1 - reduction;
                }
                else
                {
                    room.endOfLine -= 1;
                }

				if (room.endOfLine <= 0 && isTrunk && !endRoomSpawned) 
				{
					spawningEnd = true;
				}
				reduction = 0;
			}
		}

		if (room.endOfLine >= 0 || spawningEnd) 
		{
			Debug.Log ("Spawning");
			//picks an available Door from the current Room
			Door AChosenDoor = room.GetAvailableDoor ();
            if (AChosenDoor == null)
            {
				Debug.Log ("No Doors Left");
                if (isTrunk)
                {
					
                    GameObject.Find("DataMaster").GetComponent<DataSaver>().SaveData(seed, appliesShrinkage);

					PlayerPrefs.SetInt ("lvl", PlayerPrefs.GetInt("lvl") +1 );
					if (PlayerPrefs.GetInt ("lvl") == 4)
					{
						Application.Quit ();
					}
					else 
					{
						SceneManager.LoadScene (1);
					}
                }
                return false;
            }
			//Gets the position and rotation of the this rooms chosen door
			Vector3 roomPos = AChosenDoor.transform.position;
			roomPos.y -= 1.75f;
			Quaternion startRotation = AChosenDoor.transform.rotation;
            

			//Choose a room from the spawnable list and create it with the correct position
			GameObject newRoom = ChooseListObject (spawnList);
			newRoom = Instantiate (newRoom, roomPos, Quaternion.identity, transform);
			//Pick a entry from the hallway to connect to
			Door BChosenDoor = newRoom.GetComponent<Room> ().GetAvailableDoor ();
			BChosenDoor.gameObject.SetActive (false);
			//rotate the newRoom to face the room 
			Quaternion altRotation = BChosenDoor.transform.rotation;
			newRoom.transform.Rotate (0, (startRotation.eulerAngles.y + 180) - altRotation.eulerAngles.y, 0);
			//move the newRoom into position
			roomPos = BChosenDoor.transform.localPosition;
			roomPos.y -= 1.75f;
			newRoom.transform.Translate (-roomPos);

			//Sort the rooms properties
			Room r = newRoom.GetComponent<Room> ();
			r.parentGenerator = this;
			r.isTrunk = isTrunk;
			r.endOfLine = room.endOfLine;
            


			//=======================================Collision
            generatedBounds.Add(r.roomCollider.bounds);

			//Test the collisions
			bool noCollision = true;

			//check for surrounding rooms
            int i = 0;
            foreach (Bounds bound in generatedBounds)
            {
                if(r.roomCollider.bounds.Intersects(bound))

                {
                    ++i;
                    if (i > 2)
                    {
                        noCollision = false;
                    }
                }
            }
			//

            r.roomCollider.enabled = false;
			//if this is the corridor, check if a normal room can spawn
			if (spawningEnd && noCollision && spawnNormalRoom) {
				endRoomSpawned = true;
			}

			if (!spawnNormalRoom && noCollision) {
				noCollision = SpawnFromList (r, true, isTrunk, false);
				r.spawnedNeighbours = true;
			}
				
			if (noCollision) {
				Debug.Log ("Success");
				AChosenDoor.gameObject.SetActive (false);
                r.AddDoorToEncounter(BChosenDoor);
                room.AddDoorToEncounter(AChosenDoor);
				roomsSpawned += 1;
				return true;
			} else {
				Debug.Log ("Fail");
				Destroy (newRoom);
				generatedBounds.Remove (r.roomCollider.bounds);
			}

			//if it fails, try again
            if (!noCollision && isTrunk)
            {
				Debug.Log ("minimum");
				return SpawnFromList(room, spawnNormalRoom, isTrunk, true);
			}
		}
		Debug.Log ("Failed to spawn room");
		return false;

		//======================================================================Data
	}

	public int GetSeed()
	{
		return seed;
	}

    public bool GetIsShrinking()
    {
        return appliesShrinkage;
    }

    //Spawn room
    //pick number of branches
    //choose trunk
    //spawn the branched rooms



    //Spawn Room (bnum){
    //Room Spawns
    //Number of branches decided (0 - 3)
    //attempt to spawn branches
    //Spawn Rooms on end of branch
    //}


    //Spawn Starting room

    //repeat

    //how many doors available
    //choose door to spawn from
    //pick a room
    //pick a room door
    //calculate the rotation offset


    //repeat


}
