using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(AudioSource))]
public class Freeze : MonoBehaviour {

	Controller2D controller;

	public float userFreezeTime = 3f;
	public float othersFreezeTime = 3f;

	Color frozenColor = new Color(0f, 0f, 255f, 0.7f);
	Color defaultColor = new Color(255f, 255f, 255f, 1f);

	private AudioSource source;
	public AudioClip freeze;

	List<Controller2D> otherPlayers;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		source = GetComponent<AudioSource>();

		otherPlayers = new List<Controller2D>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			if (player.transform != this.transform) {
				otherPlayers.Add(player.GetComponent<Controller2D>());
			}
		}
	}
		
	void Use(){
		StartCoroutine("StartFreeze");
	}

	void ActivateFreezeOnItself(){
		controller.playerInfo.onFreeze = true;
		controller.StopMovement();
		controller.ChangeColor(frozenColor);
		source.PlayOneShot(freeze, 1);
	}

	void DeactivateFreezeOnItself(){
		controller.playerInfo.onFreeze = false;
		controller.RecoverMovement();
		source.PlayOneShot(freeze, 1);
		controller.ChangeColor(defaultColor);
	}

	void ActivateFreezeOnOthers(){
		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.StopMovement();
				enemy.ChangeColor(frozenColor);
			}
		}
	}

	void DeactivateFreezeOnOthers(){
		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.RecoverMovement();
				enemy.ChangeColor(defaultColor);
			}
		}
	}
	
	IEnumerator StartFreeze() {

		ActivateFreezeOnItself();
		float userFreezeTimeRemaining = 0f;

		while (userFreezeTimeRemaining < userFreezeTime) {
			userFreezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		DeactivateFreezeOnItself();
		float othersFreezeTimeRemaining = 0f;

		while (othersFreezeTimeRemaining < othersFreezeTime) {
			othersFreezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		DeactivateFreezeOnOthers();
	}
}
