using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	Transform ballTransform;

	void Start(){
		ballTransform = GameObject.FindGameObjectWithTag("ball").transform;
	}

	public void NextMovement(ref Vector2 input){

	}
}
