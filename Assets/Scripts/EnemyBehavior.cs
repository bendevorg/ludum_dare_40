using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehavior : MonoBehaviour {

	Controller2D controller;

	Rigidbody2D ball;
	public CircleCollider2D ballCollider;
	CircleCollider2D enemyCollider;

	public LayerMask ballCollisionMask;
	RaycastOrigins raycastOrigins;

	public BoxCollider2D map;
	public Spawner spawner;

	bool dangerZone = false;
	Path path;

	float timeToCatchPickup = 5f;
	float timeToCatchPickupRemaining = 0;

	//	Pathfinding
	Seeker seeker;
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = .3f;
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	PowerupController powerupController;

	void Start(){
		controller = GetComponent<Controller2D>();
		powerupController = GetComponent<PowerupController>();
		ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
		raycastOrigins = new RaycastOrigins();
		seeker = GetComponent<Seeker>();
	}

	public void NextMovement(ref Vector2 input){

		if(controller.playerInfo.inputEnabled){
			Vector2 direction = Vector2.zero;
			if (dangerZone){

				RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, ball.velocity, Mathf.Infinity, ballCollisionMask);

				if(hit){
					if (!controller.playerInfo.onDash) {
						for (int i = 0; i < powerupController.powerups.Length; i++){
							if (powerupController.powerups[i] == PowerupController.Powerups.Dash){
								powerupController.UsePowerup(i);
								break;
							}
						}
					}
					if (!controller.playerInfo.onZhonya) {
						for (int i = 0; i < powerupController.powerups.Length; i++){
							if (powerupController.powerups[i] == PowerupController.Powerups.Zhonya){
								powerupController.UsePowerup(i);
								break;
							}
						}
					}
				} else {
					dangerZone = false;
				}
			}
			if (!dangerZone && spawner.instantiatedPickup != null && timeToCatchPickupRemaining < timeToCatchPickup){
				timeToCatchPickupRemaining += Time.deltaTime;
				if (path != null){
					if (currentWaypoint < path.vectorPath.Count){
						// Direction to the next waypoint
						direction = (path.vectorPath[currentWaypoint]-transform.position).normalized;
						if (Vector2.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
								currentWaypoint++;
						}
					} else {
						path = null;
					}
				} else{
					seeker.StartPath(transform.position, spawner.instantiatedPickup.transform.position, OnPathComplete);
				}
				//	Calculate path to powerup
				//Vector2 powerupPosition = spawner.instantiatedPowerup.transform.position;

				//direction = new Vector2(powerupPosition.x - transform.position.x, powerupPosition.y - transform.position.y);
			} else {

				for (int i = 0; i < powerupController.powerups.Length; i++){
					if (powerupController.powerups[i] == PowerupController.Powerups.Freeze){
						if (Random.Range((int)1, (int)100000) < 10)
							powerupController.UsePowerup(i);
					}
				}

				//	Reset try to catch pickup
				if (spawner.instantiatedPickup == null){
					timeToCatchPickupRemaining = 0f;
				}
				direction = ball.velocity.Rotate(90f);

				if (Mathf.Sign(ball.velocity.x) == 1 && 
					transform.position.x <= ball.transform.position.x && 
					Mathf.Sign(direction.x) == 1){
					direction.x *= -1;
				}

				if (Mathf.Sign(ball.velocity.x) == -1 && 
					transform.position.x >= ball.transform.position.x && 
					Mathf.Sign(direction.x) == -1){
					direction.x *= -1;
				}

				if (Mathf.Sign(ball.velocity.y) == 1 && 
					transform.position.y <= ball.transform.position.y && 
					Mathf.Sign(direction.y) == 1){
					direction.y *= -1;
				}

				if (Mathf.Sign(ball.velocity.y) == -1 && 
					transform.position.y >= ball.transform.position.y && 
					Mathf.Sign(direction.y) == -1){
					direction.y *= -1;
				}
			}
			input = direction.normalized;
			controller.playerInfo.input = input;
		}
	}

	public void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
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
