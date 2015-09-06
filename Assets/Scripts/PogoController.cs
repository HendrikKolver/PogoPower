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

	private static PogoController thisObject;
	private static Collider thisGameCollider;

	
	void Awake()
	{
		// Setting up references.
		groundDetector = transform.Find("GroundDetector");
		spawnPoint = transform.position;
		thisObject = this;
		thisGameCollider = transform.GetComponent<Collider>();
	}
	
	void Update()
	{
		float yOfset = 0.47f;
		float xOfset = 0.5f;
		Vector2 startLineCast = new Vector2 (transform.position.x-xOfset,transform.position.y-yOfset);
		Vector2 endLineCast = new Vector2 (transform.position.x+xOfset,transform.position.y-yOfset);
		grounded = Physics2D.Linecast(startLineCast, endLineCast, 1 << LayerMask.NameToLayer("Ground"));

		if (grounded) {
			bounce = false;
		}

		if (jumpPressed && grounded && !bounce) {
			jump = true;
			jumpPressed = false;

			//Reset the velocity to ensure that jump height is always equal
			GetComponent<Rigidbody2D>().velocity= new Vector2(GetComponent<Rigidbody2D>().velocity.x,0f);
		}


		if(Input.GetButtonDown("Jump"))
			jumpPressed = true;

		if (grounded) {
			randomMovementCounter++;
		}

		if (randomMovementCounter >= randomMovementTimer && grounded && !jump && !jumpPressed) {
			bounce = true;
			moveObject(GetComponent<Rigidbody2D>(),(Vector2.right * getRandomNumber(-100.0f,100.0f)));
			if(GetComponent<Rigidbody2D>().velocity.y<=0f) 
				jumpObject(GetComponent<Rigidbody2D>(),(jumpForce/1.5f));
			randomMovementCounter = 0;
		}

		if(isObjectTooLow(GetComponent<Transform>())){
			returnToSpawnPoint();
		}
	}
	
	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
		if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) {
			moveObject(GetComponent<Rigidbody2D>(),(Vector2.right * h * moveForce)); 
		}		 		
		if (Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x) > maxSpeed) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}

		if(jump)
		{	

			jumpObject(GetComponent<Rigidbody2D>(),jumpForce);
			moveObject(GetComponent<Rigidbody2D>(),(Vector2.right * getRandomNumber(-100.0f,100.0f)));
			jump = false;
		}
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
