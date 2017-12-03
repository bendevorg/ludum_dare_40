using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;
	Animator animator;
	public Vector2 playerInput;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	public void Move(Vector2 moveAmount, Vector2 input){
		rb.MovePosition(rb.position + moveAmount);
		playerInput = input;

		//	Update animation
		animator.SetFloat("velocityX", input.x);
		animator.SetFloat("velocityY", input.y);
	}
	
}
