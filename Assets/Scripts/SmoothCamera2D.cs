using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform targetObject;

	// Update is called once per frame
	//Fixed update used for smoothness
	void FixedUpdate () 
	{
		if (targetObject)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(targetObject.position);
			Vector3 delta = targetObject.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}
