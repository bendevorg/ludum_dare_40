using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : LivingEntity {

	Controller2D controller;
	BoxCollider2D collider;
	PlayerInfo playerInfo;

	Vector2 input;
	float velocityXSmoothing;
	float velocityYSmoothing;
	float accelerationTime = .1f;
	public float moveSpeed = 2f;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		collider = GetComponent<BoxCollider2D>();
		playerInfo = new PlayerInfo(false, true, false, moveSpeed, accelerationTime, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInfo.inputEnabled){
			input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			input = input.normalized;
		} else if(playerInfo.onDash && (Mathf.Abs(input.x) + Mathf.Abs(input.y) != 0)) {
			controller.Dash(ref playerInfo);
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			if (!playerInfo.onDash){
				controller.Dash(ref playerInfo);
			}
		}
	}

	void FixedUpdate(){
		float targetVelocityX = input.x * playerInfo.moveSpeed;
		float targetVelocityY = input.y * playerInfo.moveSpeed;
		playerInfo.velocity.x = Mathf.SmoothDamp(playerInfo.velocity.x, targetVelocityX, ref velocityXSmoothing, playerInfo.accelerationTime);
		playerInfo.velocity.y = Mathf.SmoothDamp(playerInfo.velocity.y, targetVelocityY, ref velocityYSmoothing, playerInfo.accelerationTime);
		controller.Move(playerInfo.velocity * Time.deltaTime, input);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Enemy" && playerInfo.attack){
			other.collider.GetComponent<LivingEntity>().TakeDamage(5);
		} else if (other.collider.tag == "Wall" && playerInfo.onDash){
			transform.GetComponent<LivingEntity>().TakeDamage(99999);
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.tag == "Enemy" && playerInfo.attack){
			other.collider.GetComponent<LivingEntity>().TakeDamage(5);
		} //else if (other.collider.tag == "Wall" && playerInfo.onDash){
			//transform.GetComponent<LivingEntity>().TakeDamage(99999);
		//}
	}
}
