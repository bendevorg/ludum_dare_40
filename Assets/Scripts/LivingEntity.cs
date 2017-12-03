using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int health = 1;

	public event System.Action OnDeath;

	public event Action<LivingEntity> OnEntityDeath;

	public void TakeDamage(int damage){
		health -= damage;

		if (health <= 0)
			Death();
	}

	void Death(){
		if (OnDeath != null){
				OnDeath();
		};
		GameObject.Destroy(gameObject);
	}

}
