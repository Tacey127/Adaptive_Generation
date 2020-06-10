using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 direction;
	// Update is called once per frame
	void Update () {
		transform.localPosition += direction * Time.deltaTime;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Debug.Log ("SLOW");
			col.gameObject.GetComponent<PlayerMove> ().SlowPlayer ();
			Destroy (gameObject);
		}

		if (col.gameObject.tag == "Wall") 
		{
			Destroy (gameObject);
		}
	}

}
