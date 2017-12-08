using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PowerupController))]
public class Player : LivingEntity {

	Controller2D controller;
	PowerupController powerupController;

	Vector2 input;
	float velocityXSmoothing;
	float velocityYSmoothing;

	[Range(1, 2)]
	public int player;
	string horizontal = "Horizontal_Player";
	string vertical = "Vertical_Player";
	string driveOne = "Drive1_Player";
	string driveTwo = "Drive2_Player";

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		powerupController = GetComponent<PowerupController>();
		horizontal += player.ToString();
		vertical += player.ToString();
		driveOne += player.ToString();
		driveTwo += player.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		if (!controller.playerInfo.dead){
			
			if (controller.playerInfo.inputEnabled){
				input = new Vector2 (Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
				input = input.normalized;
				controller.playerInfo.input = input;
				if (Input.GetButtonDown(driveOne)){
					powerupController.UsePowerup(0);
				} else if (Input.GetButtonDown(driveTwo)){
					powerupController.UsePowerup(1);
				}
			}
		}
	}

	void FixedUpdate(){
		if (!controller.playerInfo.dead){
			float targetVelocityX = input.x * controller.playerInfo.moveSpeed;
			float targetVelocityY = input.y * controller.playerInfo.moveSpeed;
			controller.playerInfo.velocity.x = Mathf.SmoothDamp(controller.playerInfo.velocity.x, targetVelocityX, ref velocityXSmoothing, controller.playerInfo.accelerationTime);
			controller.playerInfo.velocity.y = Mathf.SmoothDamp(controller.playerInfo.velocity.y, targetVelocityY, ref velocityYSmoothing, controller.playerInfo.accelerationTime);
			controller.Move(controller.playerInfo.velocity * Time.deltaTime, input);
		}
	}
}
