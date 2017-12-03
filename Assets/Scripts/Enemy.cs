using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyBehavior))]
public class Enemy : LivingEntity {

	Controller2D controller;
	EnemyBehavior enemyBehavior;
	EnemyInfo enemyInfo;

	public float moveSpeed = 20f;
	float accelerationTime = .1f;
	float velocityXSmoothing;
	float velocityYSmoothing;

	List<Transform> players;
	Vector2 input;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		enemyBehavior = GetComponent<EnemyBehavior>();
		enemyInfo = new EnemyInfo(moveSpeed, accelerationTime, Vector3.zero);
		players = new List<Transform>();
		GameObject[] playersGameObject = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in playersGameObject){
			players.Add(player.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		enemyBehavior.NextMovement(ref input);
	}
	
	void FixedUpdate(){
		float targetVelocityX = input.x * enemyInfo.moveSpeed;
		float targetVelocityY = input.y * enemyInfo.moveSpeed;
		enemyInfo.velocity.x = Mathf.SmoothDamp(enemyInfo.velocity.x, targetVelocityX, ref velocityXSmoothing, enemyInfo.accelerationTime);
		enemyInfo.velocity.y = Mathf.SmoothDamp(enemyInfo.velocity.y, targetVelocityY, ref velocityYSmoothing, enemyInfo.accelerationTime);
		controller.Move(enemyInfo.velocity * Time.deltaTime, input);
	}

	struct EnemyInfo {
		public float moveSpeed;
		public float accelerationTime;
		public Vector3 velocity;

		public EnemyInfo(float _moveSpeed, float _accelerationTime, Vector3 _velocity){
			moveSpeed = _moveSpeed;
			accelerationTime = _accelerationTime;
			velocity = _velocity;
		}
	}
}
