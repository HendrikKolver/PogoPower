using UnityEngine;
using System.Collections;
using System;

public class PogoController : MonoBehaviour
{

    [HideInInspector]

    public int dropoutPoint = -10;
    private Transform groundDetector;
    private Vector2 spawnPoint;
    public Rigidbody2D pogoBlock;

    private bool jumpTrigger;
    private bool goingLeft = false;
    private bool goingRight = false;
    private bool rotating = false;
    private int totalRotation = 0;



    void Start()
    {
        pogoBlock = GetComponent<Rigidbody2D>();
        jumpTrigger = false;
    }

    void Awake()
    {
        // Setting up references.
        spawnPoint = transform.position;
    }

    void FixedUpdate()
    {
        if (goingLeft)
        {
            this.pogoBlock.AddForce(new Vector2(-5f, 0f));
        }

        if (goingRight)
        {
            this.pogoBlock.AddForce(new Vector2(5f, 0f));
        }

        if (totalRotation < 180)
        {
            Vector3 rotation;
            if (pogoBlock.velocity.x <= 0)
            {
                rotation = new Vector3(0, 0, 5);
            }
            else
            {
                rotation = new Vector3(0, 0, -5);
            }
            transform.Rotate(rotation);
            totalRotation += 10;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTrigger = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            goingLeft = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            goingLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            goingRight = true;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            goingRight = false;
        }

        if(IsObjectTooLow())
        {
            ResetPosition();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Equals("ground"))
        {
            if (jumpTrigger)
            {
                this.pogoBlock.AddForce(new Vector2(0f, 350f));
                totalRotation = 0;
                jumpTrigger = false;
            }
            else
            {
                this.pogoBlock.AddForce(new Vector2(0f, 250f));
            }
        }
    }

    public void ResetPosition()
    {
        pogoBlock.velocity = Vector2.zero;
        transform.position = spawnPoint;
    }

    private float GetRandomNumber(float minVal, float maxVal)
    {
        return (float)Math.Round(UnityEngine.Random.Range(minVal, maxVal));
    }

    private bool IsObjectTooLow()
    {
        return (pogoBlock.position.y < dropoutPoint);
    }

    private bool IsObjectVisible(Renderer renderer)
    {
        return renderer.isVisible;
    }


    /*
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
    */
}
