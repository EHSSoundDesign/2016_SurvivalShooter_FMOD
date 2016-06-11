using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//player's speed
	public float speed=6f;
	//player's movement
	Vector3 movement;
	// player's animation
	Animator anim;
	//player's rigid body
	Rigidbody playerRigidBody;
	// floor quad
	int floorMask;

	float camRayLength = 100f;

	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		//calling functions createed below

		Move (h, v);
		Turning ();
		Animating (h, v);

	}

	//Movement
	#region Movement
	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidBody.MovePosition (transform.position + movement);
	}
	#endregion

	//Turning
	#region Turning
	void Turning ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidBody.MoveRotation (newRotation);
		}
	}
	#endregion

	//Animation
	#region Animation
	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
	}
	#endregion

}
