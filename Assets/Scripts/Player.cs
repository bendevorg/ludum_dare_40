using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;

	Vector2 input;
	Vector3 velocity;
	float velocityXSmoothing;
	float velocityYSmoothing;
	float accelerationTime = .1f;
	public float moveSpeed = 2f;

	public float dashSpeedMultiplier = 4f;
	public float dashTime = .25f;
	bool onDash = false;
	float dashTimeRemaining;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (!onDash){
			input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			input = input.normalized;
		} else if(dashTimeRemaining >= dashTime) {
			onDash = false;
			moveSpeed /= dashSpeedMultiplier;
		} else {
			dashTimeRemaining += Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			if (!onDash){
				onDash = true;
				moveSpeed *= dashSpeedMultiplier;
				dashTimeRemaining = 0f;
			}
		}
	}

	void FixedUpdate(){
		float targetVelocityX = input.x * moveSpeed;
		float targetVelocityY = input.y * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
		velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);
		controller.Move(velocity * Time.deltaTime);
	}

}
