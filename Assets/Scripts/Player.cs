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

	public float dashSpeedMultiplier = 40f;
	public float dashTime = .25f;
	float dashTimeRemaining;

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
			Dash();
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			if (!playerInfo.onDash){
				Dash();
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

	void Dash(){
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

	struct PlayerInfo {
		public bool onDash;
		public bool inputEnabled;
		public bool attack;
		public float moveSpeed;
		public float accelerationTime;
		public Vector3 velocity;

		public PlayerInfo(bool _onDash, bool _inputEnabled, bool _attack, float _moveSpeed, float _accelerationTime, Vector3 _velocity){
			onDash = _onDash;
			inputEnabled = _inputEnabled;
			attack = _attack;
			moveSpeed = _moveSpeed;
			accelerationTime = _accelerationTime;
			velocity = _velocity;
		}
	}
}
