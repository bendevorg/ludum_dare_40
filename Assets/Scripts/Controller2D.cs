using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;
	public Vector2 playerInput;

	public float dashSpeedMultiplier = 40f;
	public float dashTime = .25f;
	float accelerationTime = .1f;
	float dashTimeRemaining;


	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}

	public void Move(Vector2 moveAmount, Vector2 input){
		rb.MovePosition(rb.position + moveAmount);
		playerInput = input;

	}	

	public void Dash(ref PlayerInfo playerInfo){
		if (!playerInfo.onDash){
			playerInfo.onDash = true;
			playerInfo.inputEnabled = false;
			playerInfo.attack = true;
			playerInfo.moveSpeed *= dashSpeedMultiplier;
			dashTimeRemaining = 0f;
			playerInfo.accelerationTime = 0f;
		} else if (dashTimeRemaining < dashTime){
			dashTimeRemaining += Time.deltaTime;
		} else {
			playerInfo.onDash = false;
			playerInfo.inputEnabled = true;
			playerInfo.attack = false;
			playerInfo.moveSpeed /= dashSpeedMultiplier;
			playerInfo.accelerationTime = accelerationTime;
			playerInfo.velocity.x = 0;
			playerInfo.velocity.y = 0;
		}
	}
}
