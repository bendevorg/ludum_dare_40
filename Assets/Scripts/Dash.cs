using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(AudioSource))]
public class Dash : MonoBehaviour {

	Controller2D controller;

	public float dashSpeedMultiplier = 2.5f;
	public float dashTime = .75f;

	private AudioSource source;
	public AudioClip dash;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		source = GetComponent<AudioSource>();
	}

	public void Use(){
		StartCoroutine("StartDash");
	}

	IEnumerator StartDash() {
		if (controller.playerInput.x != 0 || controller.playerInput.y != 0){
			ActivateDash();
			float dashTimeRemaining = 0f;

			while (dashTimeRemaining < dashTime){
				dashTimeRemaining += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			DeactivateDash();
		}
	}

	void ActivateDash(){
		controller.playerInfo.onDash = true;
		controller.playerInfo.inputEnabled = false;
		controller.BoostMovement(controller.playerInfo.moveSpeed * dashSpeedMultiplier, 0f);
		source.PlayOneShot(dash, 1);
	}

	void DeactivateDash(){
		controller.playerInfo.onDash = false;
		controller.playerInfo.inputEnabled = true;
		controller.RecoverMovement();
		controller.playerInfo.velocity.x = 0;
		controller.playerInfo.velocity.y = 0;
	}
	
}
