using UnityEngine;
using System;
using System.Collections;

public class MovingGround : MonoBehaviour {

	float originalX;
	
	public float floatStrength = 1; // You can change this in the Unity Editor to 
	// change the range of y positions that are possible.
	
	void Start()
	{
		this.originalX = this.transform.position.x;
	}
	
	void Update()
	{
		transform.position = new Vector3(originalX + ((float)Math.Sin(Time.time/2f) * floatStrength),
                                      	 transform.position.y,
		                                 transform.position.z);
	}
}
