using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RoomPacket;

public class Room : MonoBehaviour {

    //Recorded Data
    Packet roomPacket = new Packet();
    [SerializeField] string RoomName = "noName";
    float timeToComplete = 0f;
    DataSaver dS;

    //Timer
    [SerializeField] float roomTime = 5f;

    //Generation
    [SerializeField] float additionalRoomChance = 0f;
    [SerializeField] List<Door> spawnableDoors = new List<Door>();

    public bool isNormalRoom = false;
    public bool spawnedNeighbours = false;
    public int endOfLine;//The number of allowed rooms left this chain will spawn
    public bool isTrunk = false; //Is this path meant to lead to the final room

    //Collision
    public Collider roomCollider;

    //Encounters
    [SerializeField] public bool isEncounter = false;
    public DungeonGenerator parentGenerator;
    public int remainingObjectives = 0;

    [SerializeField] List<Door> encounterDoors = new List<Door>();


    //==================================================================================initialization
    void Start()
    {
        dS = GameObject.Find("DataMaster").GetComponent<DataSaver>();
    }




    //===================================================================================Doors
    public Door GetAvailableDoor() {
        if (spawnableDoors.Count == 0)
            return null;
        int chosenDoorFromList = (int)Mathf.Floor(Random.value * spawnableDoors.Count);
        Door chosenDoor = spawnableDoors[chosenDoorFromList];
        spawnableDoors.RemoveAt(chosenDoorFromList);
        return chosenDoor;
    }

    void SetDoors(bool isOpen)
    {
        foreach (Door d in encounterDoors)
        {
            d.gameObject.SetActive(isOpen);
        }
    }

    public void AddDoorToEncounter(Door door)
    {
        encounterDoors.Add(door);
    }

    //===================================================================================Spawning
    void SpawnNeighbours()
    {
        spawnedNeighbours = true;
        if (isTrunk) {
            //Spawn The Trunk
            Debug.Log("BEEP");
            parentGenerator.SpawnFromList(this, !isNormalRoom, true, false);
        }
        else
        {
            if (Random.value < additionalRoomChance)
            {
                Debug.Log("B33P");
                parentGenerator.SpawnFromList(this, !isNormalRoom, false, false);
            }
        }
        //Have a chance to spawn an aditional branch
        if (Random.value < additionalRoomChance)
        {
            Debug.Log("BiiP");
            parentGenerator.SpawnFromList(this, !isNormalRoom, false, false);
        }
    }


    //===================================================================================Encounters
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {


            if (!spawnedNeighbours) {
                SpawnNeighbours();
            }

            if (isEncounter) {
                isEncounter = false;
                SetDoors(true);
                StartCoroutine(DungeonTimer(roomTime));
                StartCoroutine(RoomTimer());
            }
        }
    }

    public void ReduceObjectives()
    {
        remainingObjectives -= 1;
        if (remainingObjectives == 0)
        {
            SetDoors(false);

        }
    }


    //===================================================================================Timers
    IEnumerator RoomTimer()
    {
        while (remainingObjectives != 0)
        {
            yield return new WaitForFixedUpdate();
            timeToComplete += Time.fixedDeltaTime;
        }
        SendData();
        Debug.Log(timeToComplete);
    }



    IEnumerator DungeonTimer(float predictedTime)
    {
        yield return new WaitForSeconds(predictedTime);
        if (remainingObjectives == 0) {

        } else {
            if (remainingObjectives > 0)
                parentGenerator.reduction += 1;
        }
    }

    //==============================================================================Data

    public int GetDungeonSeed()
    {
        return parentGenerator.GetSeed();
    }

    public bool GetIsShrinking()
    {
        return parentGenerator.GetIsShrinking();
    }

	void SendData()
	{
		
		roomPacket.roomName = RoomName;
		roomPacket.expectedTime = roomTime;
		roomPacket.timeToComplete = timeToComplete;
		roomPacket.endOfLine = endOfLine;
		//roomPacket.
		dS.AddToDataPacket (roomPacket);
	}
}
