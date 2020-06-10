using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomPacket {

	//Rooms encountered:
	//RoomName
	//branchNumber = 0;
	//branchFrom = 0;
	//timeToComplete = 0f;
	//timesHitByObstacle = 0;
	//obstacleHitTimes
	//endofline
	//explored
	public class Packet
	{
		public string roomName;
		public int branchNum;
		public int branchFrom;
		public float expectedTime;
		public float timeToComplete;
		public int timesHitByObstacle;
		public float obstacleHitTimes;
		public int endOfLine;
		public bool explored;

		//For Filtered result
		public int numberOfRooms = 1;
	}
}
