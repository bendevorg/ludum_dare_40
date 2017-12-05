using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int health = 1;

	public event System.Action OnDeath;

	public void TakeDamage(int damage){
		health -= damage;

		if (health <= 0)
			Death();
	}

	void Death(){
		if (OnDeath != null){
			OnDeath();
		};
		GetComponent<BoxCollider2D>().enabled = false;
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
		GetComponent<Controller2D>().playerInfo.dead = true;
	}
}
