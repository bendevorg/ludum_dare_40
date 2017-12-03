using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	Controller2D controller;

	Rigidbody2D ball;
	CircleCollider2D ballCollider;
	CircleCollider2D enemyCollider;

	public LayerMask playerMask;
	[Range(2, 10)]
	public int raycastCount;
	RaycastOrigins raycastOrigins;

	public BoxCollider2D map;

	bool dangerZone = false;

	void Start(){
		controller = GetComponent<Controller2D>();
		ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
		ballCollider = ball.GetComponent<CircleCollider2D>();
		raycastOrigins = new RaycastOrigins();
	}

	public void NextMovement(ref Vector2 input, ref PlayerInfo enemyInfo){

		if (dangerZone){
			Debug.Log("Top term");
			if (!enemyInfo.onDash){
				Debug.Log("Olha que bosta");
				controller.Dash(ref enemyInfo);
			}
		}

		Vector2 perpendicularDirection = ball.velocity.Rotate(90f);
		if ((Mathf.Sign(perpendicularDirection.x) > 0 && transform.position.x < ball.transform.position.x) ||
			(Mathf.Sign(perpendicularDirection.x) < 0 && transform.position.x > ball.transform.position.x)){
			perpendicularDirection.x *= -1;
		} else if ((Mathf.Sign(perpendicularDirection.y) > 0 && transform.position.y < ball.transform.position.y) ||
			(Mathf.Sign(perpendicularDirection.y) < 0 && transform.position.y > ball.transform.position.y)){
			perpendicularDirection.y *= -1;
		}
		input = perpendicularDirection.normalized;
	}

	public void UpdateRaycastOrigins(){
		Bounds bounds = ballCollider.bounds;
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	public struct RaycastOrigins{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Ball"){
			dangerZone = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Ball"){
			dangerZone = false;
		}
	}

}
