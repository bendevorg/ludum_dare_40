using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : LivingEntity {

	Controller2D controller;
	BoxCollider2D collider;

	Vector2 input;
	float velocityXSmoothing;
	float velocityYSmoothing;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

		if (controller.playerInfo.onDash){
			controller.Dash();	
		} else if(controller.playerInfo.onZhonya){
			controller.Zhonya();
		}
		
		if (controller.playerInfo.inputEnabled){
			input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			input = input.normalized;
			controller.playerInfo.input = input;
			if (Input.GetButtonDown("Drive1_Player1")){
				controller.UsePowerup(0);
			} else if (Input.GetButtonDown("Drive2_Player1")){
				controller.UsePowerup(1);
			}
		}
	}

	void FixedUpdate(){
		float targetVelocityX = input.x * controller.playerInfo.moveSpeed;
		float targetVelocityY = input.y * controller.playerInfo.moveSpeed;
		controller.playerInfo.velocity.x = Mathf.SmoothDamp(controller.playerInfo.velocity.x, targetVelocityX, ref velocityXSmoothing, controller.playerInfo.accelerationTime);
		controller.playerInfo.velocity.y = Mathf.SmoothDamp(controller.playerInfo.velocity.y, targetVelocityY, ref velocityYSmoothing, controller.playerInfo.accelerationTime);
		controller.Move(controller.playerInfo.velocity * Time.deltaTime, input);
	}
}
