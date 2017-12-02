using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	Rigidbody2D ball;

	void Start(){
		ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
	}

	public void NextMovement(ref Vector2 input){

		// Defensive Behavior first
		//Raycast hit = Physics2D.Raycast(ball.position, )

	}
}
