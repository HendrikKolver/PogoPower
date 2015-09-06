using UnityEngine;
using System;
using System.Collections;

public class endingOrb : MonoBehaviour {

	float originalY;
	private int currentLevel;
	
	public float floatStrength = 1; // You can change this in the Unity Editor to 
	// change the range of y positions that are possible.
	
	void Start()
	{
		this.originalY = this.transform.position.y;
	}

	void Awake(){
		currentLevel = Application.loadedLevel;
	}
	
	void Update()
	{
		transform.position = new Vector3(transform.position.x,
		                                 originalY + ((float)Math.Sin(Time.time) * floatStrength),
		                                 transform.position.z);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			GlobalRunner.setCurrentLevel (currentLevel);
		}
	}
}
