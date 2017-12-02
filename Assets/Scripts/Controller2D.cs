using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;
	public Vector2 playerInput;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}

	public void Move(Vector2 moveAmount, Vector2 input){
		rb.MovePosition(rb.position + moveAmount);
		playerInput = input;
	}
	
}
