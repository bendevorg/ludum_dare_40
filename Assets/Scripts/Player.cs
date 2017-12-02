using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;
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
		playerInfo = new PlayerInfo(false, true, moveSpeed, accelerationTime, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInfo.inputEnabled){
			input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			input = input.normalized;
		} else if(playerInfo.onDash) {
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
		controller.Move(playerInfo.velocity * Time.deltaTime);
	}

	void Dash(){
		if (!playerInfo.onDash){
			playerInfo.onDash = true;
			playerInfo.inputEnabled = false;
			playerInfo.moveSpeed *= dashSpeedMultiplier;
			Debug.Log(playerInfo.moveSpeed);
			dashTimeRemaining = 0f;
			playerInfo.accelerationTime = 0f;
		} else if (dashTimeRemaining < dashTime){
			dashTimeRemaining += Time.deltaTime;
			Debug.Log("a");
		} else {
			Debug.Log("b");
			playerInfo.onDash = false;
			playerInfo.inputEnabled = true;
			playerInfo.moveSpeed /= dashSpeedMultiplier;
			playerInfo.accelerationTime = accelerationTime;
		}
	}

	struct PlayerInfo {
		public bool onDash;
		public bool inputEnabled;
		public float moveSpeed;
		public float accelerationTime;
		public Vector3 velocity;

		public PlayerInfo(bool _onDash, bool _inputEnabled, float _moveSpeed, float _accelerationTime, Vector3 _velocity){
			onDash = _onDash;
			inputEnabled = _inputEnabled;
			moveSpeed = _moveSpeed;
			accelerationTime = _accelerationTime;
			velocity = _velocity;
		}
	}
}
