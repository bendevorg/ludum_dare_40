using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	Rigidbody2D ball;
	CircleCollider2D ballCollider;

	public LayerMask playerMask;
	[Range(2, 10)]
	public int raycastCount;
	RaycastOrigins raycastOrigins;

	public BoxCollider2D map;

	void Start(){
		ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
		ballCollider = ball.GetComponent<CircleCollider2D>();
		raycastOrigins = new RaycastOrigins();
	}

	public void NextMovement(ref Vector2 input){
		UpdateRaycastOrigins();
		// Defensive Behavior first
		for (int i = 1; i <= raycastCount; i++){

			Vector2 raycastOrigin = new Vector2(raycastOrigins.bottomLeft.x + (raycastOrigins.topRight.x - raycastOrigins.bottomLeft.x)/i,
				raycastOrigins.bottomLeft.y + (raycastOrigins.topRight.y - raycastOrigins.bottomLeft.y)/i);

			Debug.Log(ballCollider.bounds.min.y + (ballCollider.bounds.max.y + ballCollider.bounds.min.y)/2);
			RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, ball.velocity, Mathf.Infinity, playerMask);
			Debug.DrawRay(raycastOrigin, ball.velocity, Color.red);

			if (hit && hit.collider.transform == this.transform){
				input = new Vector2(hit.collider.transform.position.x + transform.position.x, hit.collider.transform.position.y + transform.position.y);
				input = input.normalized;
				return;
			}
		}

		// No action avaiable go to center for now
		input = new Vector2(map.bounds.center.x - transform.position.x, map.bounds.center.y - transform.position.y);
		input = input.normalized;
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
}
