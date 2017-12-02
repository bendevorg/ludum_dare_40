using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

	public float speed;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.MovePosition(rb.position + ((Vector2)transform.right * speed));
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag == "Wall"){
			Destroy(gameObject);
		} else if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy"){
			collision.collider.GetComponent<LivingEntity>().TakeDamage(1);
		}
	}
}
