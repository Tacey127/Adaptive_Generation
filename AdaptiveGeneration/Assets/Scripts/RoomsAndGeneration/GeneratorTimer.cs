using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTimer : MonoBehaviour {

	public void StartTimer (float time) {
		StartCoroutine (DungeonTimer (time));
	}

	//Wait until the timer finishes before reducing room sizes
	//predicted time(dungeon size time)
	//room time(encounters completed successfully)
	//???
	IEnumerator DungeonTimer(float predictedTime)
	{
		while (true) {
			yield return new WaitForSeconds (predictedTime);
			Debug.Log ("TTT");
		}
	}
}
