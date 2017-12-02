using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	Rigidbody2D rb;
	public float ballInitialSpeed = 100f;
	public float ballSpeedIncrementOnBounce = 100f;
	public float ballMinSpeed = 3f;
	public float ballMaxSpeed = 25f;

	float directionX;
	float directionY;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Vector2 initialForce = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
		initialForce = initialForce.normalized;
		directionX = Mathf.Sign(initialForce.x);
		directionY = Mathf.Sign(initialForce.y);
		rb.AddForce(initialForce * ballSpeed);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Wall"){
			Vector2 newVelocity = rb.velocity * ballSpeedIncrementOnBounce;
			directionX = Mathf.Sign(newVelocity.x);
			directionY = Mathf.Sign(newVelocity.y);
			newVelocity = new Vector2(Mathf.Clamp(Mathf.Abs(newVelocity.x), ballMinSpeed, ballMaxSpeed) * directionX, 
				Mathf.Clamp(Mathf.Abs(newVelocity.y), ballMinSpeed, ballMaxSpeed) * directionY);
			rb.velocity = newVelocity;
		} else if (other.collider.tag == "Player" || other.collider.tag == "Enemy"){
			other.collider.GetComponent<LivingEntity>().TakeDamage(999);
		}
	}
}
