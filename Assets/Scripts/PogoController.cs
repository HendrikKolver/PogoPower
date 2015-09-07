using UnityEngine;
using System.Collections;
using System;

public class PogoController : MonoBehaviour {

	[HideInInspector]
	private bool jump = false;	
	private bool jumpPressed = false;
	private bool bounce = false;
	
	public float moveForce = 365f;			
	public float maxSpeed = 5f;			
	public float jumpForce = 1000f;
	public int randomMovementTimer = 100;
	public int dropoutPoint = -10;

			
	private bool grounded = false;

	private int randomMovementCounter = 0;
	private Transform groundDetector;
	private Vector2 spawnPoint;
	private bool canJump;

	void Awake()
	{
		// Setting up references.
		groundDetector = transform.Find("GroundDetector");
		spawnPoint = transform.position;
	}
	
	void FixedUpdate ()
	{
		grounded = isGrounded ();
		if (grounded) {
			bounce = false;
		}
		
		if (jumpPressed && grounded && !bounce) {
			jump = true;
			jumpPressed = false;
			
			//Reset the velocity to ensure that jump height is always equal
			GetComponent<Rigidbody2D>().velocity= new Vector2(GetComponent<Rigidbody2D>().velocity.x,0f);
		}

		addRandomMovement ();

		//Check if player in bounds
		if(isObjectTooLow(GetComponent<Transform>())){
			returnToSpawnPoint();
		}

		var h = getTouchInput ();
		applyMovement (h);		 		
		jumpIfNeeded ();
	}

	void jumpIfNeeded ()
	{
		if (jump) {
			jumpObject (GetComponent<Rigidbody2D> (), jumpForce);
			moveObject (GetComponent<Rigidbody2D> (), (Vector2.right * getRandomNumber (-100.0f, 100.0f)));
			jump = false;
		}
	}

	void applyMovement (float h)
	{
		if (h * GetComponent<Rigidbody2D> ().velocity.x < maxSpeed) {
			moveObject (GetComponent<Rigidbody2D> (), (Vector2.right * h * moveForce));
		}
		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > maxSpeed) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	float getTouchInput ()
	{
		float h = 0f;
		if (Input.touchCount > 0) {
			Touch[] touches = Input.touches;
			foreach (Touch touch in touches) {
				if (touch.position.x < Screen.width / 4) {
					h = -1f;
				}
				else
					if (touch.position.x > Screen.width - Screen.width / 4) {
						h = 1f;
					}
					else {
						jumpPressed = true;
					}
			}
		}
		return h;
	}

	void addRandomMovement ()
	{
		if (grounded) {
			randomMovementCounter++;
		}
		if (randomMovementCounter >= randomMovementTimer && grounded && !jump && !jumpPressed) {
			bounce = true;
			moveObject (GetComponent<Rigidbody2D> (), (Vector2.right * getRandomNumber (-100.0f, 100.0f)));
			if (GetComponent<Rigidbody2D> ().velocity.y <= 0f)
				jumpObject (GetComponent<Rigidbody2D> (), (jumpForce / 1.5f));
			randomMovementCounter = 0;
		}
	}

	bool isGrounded ()
	{
		float yOfset = 0.47f;
		float xOfset = 0.5f;
		Vector2 startLineCast = new Vector2 (transform.position.x - xOfset, transform.position.y - yOfset);
		Vector2 endLineCast = new Vector2 (transform.position.x + xOfset, transform.position.y - yOfset);
		return Physics2D.Linecast (startLineCast, endLineCast, 1 << LayerMask.NameToLayer ("Ground"));
	}

	private void moveObject(Rigidbody2D component, Vector2 force){
		component.AddForce(force);
	}

	private void jumpObject(Rigidbody2D component, float jumpForce){
		component.AddForce(new Vector2(0f, jumpForce));
	}

	private void setObjectPosition(Rigidbody2D component, Vector2 position){
		component.velocity = Vector2.zero;
		transform.position = position;
	}

	private float getRandomNumber(float minVal, float maxVal){
		return (float)Math.Round(UnityEngine.Random.Range(minVal,maxVal));
	}

	private bool isObjectTooLow(Transform component){
		return (component.position.y < dropoutPoint);
	}

	private bool isObjectVisible(Renderer renderer){
		return renderer.isVisible;
	}

	public void returnToSpawnPoint(){
		setObjectPosition(GetComponent<Rigidbody2D>(), spawnPoint);
	}
}
