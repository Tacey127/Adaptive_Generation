using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	[SerializeField] string horizontalInputName;
	[SerializeField] string verticalInputName;

	[SerializeField] float moveSpeed;

	private CharacterController charController;

	[SerializeField] AnimationCurve jumpFallOff;
	[SerializeField] float jumpMultiplier;
	[SerializeField] KeyCode jumpKey;
	bool isJumping;

    bool slow = false;

	void Awake()
	{
		charController = GetComponent<CharacterController> ();
	}

	void Update()
	{
		PlayerMovement();
	}

	void PlayerMovement()
	{
		float vertInput = Input.GetAxis (verticalInputName);
		float horizInput = Input.GetAxis (horizontalInputName);

		Vector3 forwardMovement = transform.forward * vertInput;
		Vector3 rightmMovement = transform.right * horizInput;

		if (slow) {
			charController.SimpleMove (Vector3.ClampMagnitude (forwardMovement + rightmMovement, 1.0f) * (moveSpeed * 0.5f));
		} else {
			charController.SimpleMove (Vector3.ClampMagnitude (forwardMovement + rightmMovement, 1.0f) * moveSpeed);
		}

		JumpInput ();
	}

	private void JumpInput(){
		if (Input.GetKeyDown (jumpKey) && !isJumping) {
			isJumping = true;
			StartCoroutine (JumpEvent());
		}
	}

	IEnumerator JumpEvent()
	{
		charController.slopeLimit = 90.0f;
		float timeInAir = 0.0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		}while(!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

		charController.slopeLimit = 45.0f;
		isJumping = false;
	}

	public void SlowPlayer()
	{
		StartCoroutine (Slow());
	}
		
    IEnumerator Slow()
    {
		Debug.Log ("SLOW");
        slow = true;
        yield return new WaitForSeconds(2);
        slow = false;
    }

}