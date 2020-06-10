using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

	[SerializeField] string mX, mY;
	[SerializeField] float sensitivity;

	[SerializeField] Transform playerBody;

	float xClamp;


	private void Awake()
	{
		xClamp = 0;
		LockCursor ();
	}

	void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update () 
	{
		CameraRotation ();
	}

	void CameraRotation()
	{
		float mouseX = Input.GetAxis (mX) * sensitivity * Time.deltaTime;
		float mouseY= Input.GetAxis (mY) * sensitivity * Time.deltaTime;

		xClamp += mouseY;

		if (xClamp > 90) {
			xClamp = 90;
			mouseX = 0;
			ClampXToValue (270.0f);
		}
		else if (xClamp < -90) {
			xClamp = -90;
			mouseY = 0;
			ClampXToValue (90.0f);
		}

		transform.Rotate (Vector3.left * mouseY);
		playerBody.Rotate (Vector3.up * mouseX);
	}

	void ClampXToValue(float value)
	{
		Vector3 eRotation = transform.eulerAngles;
		eRotation.x = value;
		transform.eulerAngles = eRotation;
	}
}
