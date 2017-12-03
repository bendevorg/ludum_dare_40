using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyBehavior))]
public class Enemy : LivingEntity {

	Controller2D controller;
	EnemyBehavior enemyBehavior;

	float velocityXSmoothing;
	float velocityYSmoothing;

	List<Transform> players;
	Vector2 input;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		enemyBehavior = GetComponent<EnemyBehavior>();
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
		float targetVelocityX = input.x * controller.playerInfo.moveSpeed;
		float targetVelocityY = input.y * controller.playerInfo.moveSpeed;
		controller.playerInfo.velocity.x = Mathf.SmoothDamp(controller.playerInfo.velocity.x, targetVelocityX, ref velocityXSmoothing, controller.playerInfo.accelerationTime);
		controller.playerInfo.velocity.y = Mathf.SmoothDamp(controller.playerInfo.velocity.y, targetVelocityY, ref velocityYSmoothing, controller.playerInfo.accelerationTime);
		controller.Move(controller.playerInfo.velocity * Time.deltaTime, input);
	}
}
