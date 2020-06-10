using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

using RoomPacket;

public class DataSaver : MonoBehaviour {

	FileStream file; 
	//RecievedPacket
	List <Packet> rp = new List<Packet>();

	List <Packet> sortedPacket = new List<Packet> ();

	int a  = 1;
    // Use this for initialization

    public void SaveData(int seed, bool isShrinking) {

		SaveResults (seed, isShrinking);
		SaveFilteredResults(seed);
	}

	void SaveResults(int seed, bool isShrinking)
	{
        int shin = -1;
        if (isShrinking)
        {
			shin = 1;
        }
        else {
			shin = 0;
        }
		string path = Application.dataPath + "results.txt";

		StreamWriter writer = new StreamWriter (path, true);
		writer.WriteLine("NewFloor");

		writer.WriteLine("IsShrinking, ", + shin);
		writer.WriteLine ("Seed, " + seed);
		writer.WriteLine ("Total Session Time, " + Time.timeSinceLevelLoad);


		float predictedRoomTime = 0;
		float timeSpentInEncounters = 0;
		int roomsBeyondExpectedTime = 0;
		foreach (Packet packet in rp) {

			predictedRoomTime += packet.expectedTime;
			timeSpentInEncounters += packet.timeToComplete;

			if (packet.expectedTime <= packet.timeToComplete) {
				roomsBeyondExpectedTime += 1;
			}
		}

		writer.WriteLine("Rooms Generated", rp.Count);
		writer.WriteLine ("Expected Time In All Rooms, " + predictedRoomTime);
		writer.WriteLine ("Total Time Spent In All Rooms, " + timeSpentInEncounters);
		writer.WriteLine ("Number Of Rooms Past Expected Time, " + roomsBeyondExpectedTime);




		foreach (Packet packet in rp) {
			writer.WriteLine ("name, " + packet.roomName);
			writer.WriteLine ("branchDepth, " + packet.endOfLine);
			writer.WriteLine ("expectedTime, " + packet.expectedTime);
			writer.WriteLine ("completedTime, " + packet.timeToComplete);
		}

		writer.Close();


		//seed-
		//overall session time-

		//rooms generated-
		//overall predicted time for every room-
		//overall time in every room-
		//failed rooms-
		//obstacleHits
		//branches

		//Rooms encountered:
		//RoomName-
		//endofline-
		//timeToComplete-
		//expectedTime-
		//branchNumber
		//branchFrom
		//timesHitByObstacle
		//obstacleHitTimes
		//explored
	}

	void SaveFilteredResults(int seed)
	{

		bool newFilteredResult = false;
		//numberofrooms
		foreach (Packet packet in rp) 
		{
			newFilteredResult = true;
			foreach (Packet newPacket in sortedPacket)
			{
				Debug.Log ("T");
				if (packet.roomName == newPacket.roomName) 
				{
					newFilteredResult = false;
					newPacket.numberOfRooms++;
					newPacket.timeToComplete += packet.timeToComplete;
				}
			}
			if (newFilteredResult) 
			{
				sortedPacket.Add (packet);
			}
		
		}

		string path = Application.dataPath + "filtered_results.txt";

		StreamWriter writer = new StreamWriter (path, true);
		writer.WriteLine("New Set");

		foreach (Packet packet in sortedPacket) {
			writer.WriteLine ("name, " + packet.roomName);
			writer.WriteLine ("roomsEncountered, " + packet.numberOfRooms);
			writer.WriteLine ("totalTimeInRoomSet, " + packet.timeToComplete);
			writer.WriteLine ("averageCompletionTime, " + packet.timeToComplete / (float)packet.numberOfRooms);
			writer.WriteLine ("expectedTime, " + packet.expectedTime);

		}

		writer.Close();

	}

	public void AddToDataPacket(Packet newRoomPacket)
	{
		Debug.Log ("Data stored");
		rp.Add (newRoomPacket);
	}
}
