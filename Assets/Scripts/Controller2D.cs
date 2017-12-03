using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;
	BoxCollider2D collider;
	Animator animator;
	[HideInInspector]
	public PlayerInfo playerInfo;

	SpriteRenderer spriteRenderer;

	public float moveSpeed = 20f;
	float accelerationTime = .1f;

	public Vector2 playerInput;


	public float dashSpeedMultiplier = 40f;
	public float dashTime = .25f;
	float dashTimeRemaining;

	public float zhonyaTime = 2f;
	float zhonyaTimeRemaining;
	public float zhonyaDrawbackTime = 2f;
	float zhonyaDrawbackTimeRemaining;

	void Awake(){
		playerInfo = new PlayerInfo(moveSpeed, accelerationTime, Vector3.zero);
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Move(Vector2 moveAmount, Vector2 input) {
		rb.MovePosition(rb.position + moveAmount);
		playerInput = input;

		//	Update animation
		animator.SetFloat("inputX", input.x);
		animator.SetFloat("inputY", input.y);
		if (input.x != 0 || input.y != 0) {
			animator.SetBool("isWalking", true);
			animator.SetFloat("lastInputX", input.x);
			animator.SetFloat("lastInputY", input.y);
		} else {
			animator.SetBool("isWalking", false);
		}
	}	

	public void Dash(ref PlayerInfo playerInfo){
		if (!playerInfo.onDash){
			playerInfo.onDash = true;
			playerInfo.inputEnabled = false;
			playerInfo.attack = true;
			playerInfo.moveSpeed *= dashSpeedMultiplier;
			dashTimeRemaining = 0f;
			playerInfo.accelerationTime = 0f;
		} else if (dashTimeRemaining < dashTime){
			dashTimeRemaining += Time.deltaTime;
		} else {
			playerInfo.onDash = false;
			playerInfo.inputEnabled = true;
			playerInfo.attack = false;
			playerInfo.moveSpeed /= dashSpeedMultiplier;
			playerInfo.accelerationTime = accelerationTime;
			playerInfo.velocity.x = 0;
			playerInfo.velocity.y = 0;
		}
	}

	public void Zhonya(ref PlayerInfo playerInfo) {
		if (!playerInfo.onZhonya){
			playerInfo.onZhonya = true;
			playerInfo.inputEnabled = false;
			playerInfo.moveSpeed = 0;
			playerInfo.accelerationTime = 0f;
			collider.isTrigger = true;
			zhonyaTimeRemaining = 0f;
			zhonyaDrawbackTimeRemaining = 0f;
			playerInfo.velocity.x = 0;
			playerInfo.velocity.y = 0;

			// Color
			Color temp = spriteRenderer.color;
			temp.a = 0.2f;
			spriteRenderer.color = temp;
		} else if (zhonyaTimeRemaining < zhonyaTime){
			zhonyaTimeRemaining += Time.deltaTime;
		} else if (zhonyaDrawbackTimeRemaining < zhonyaDrawbackTime){
				if (collider.isTrigger){
					collider.isTrigger = false;
					// Color
					Color temp = spriteRenderer.color;
					temp.a = 1f;
					spriteRenderer.color = temp;
				}
			zhonyaDrawbackTimeRemaining += Time.deltaTime;
		} else {
			playerInfo.onZhonya = false;
			playerInfo.inputEnabled = true;
			playerInfo.moveSpeed = moveSpeed;
			playerInfo.accelerationTime = accelerationTime;
		}
	}
}
