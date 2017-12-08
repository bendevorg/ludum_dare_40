using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUI))]
public class Powerup : MonoBehaviour {
	
	PlayerUI playerUI;

	public enum Powerups { None = -1, Dash = 0, Zhonya = 1, Freeze = 2 };

	string[] powerupNames = new string[] { "None", "Dash", "Zhonya", "Freeze" };
	public Powerups[] powerups = new Powerups[] { Powerups.None, Powerups.None };
	
	public float dashSpeedMultiplier = 2.5f;
	public float dashTime = .75f;
	float dashTimeRemaining;

	public float userFreezeTime = 3f;
	float userFreezeTimeRemaining;
	public float freezeTime = 3f;
	float freezeTimeRemaining;

	public AudioClip freeze;
	public AudioClip pickup;
  public AudioClip dash;

	// Use this for initialization
	void Start () {
		playerUI = GetComponent<PlayerUI>();
	}

	public void UsePowerup(int usedPowerup) {
		switch (powerups[usedPowerup]) {
			case Powerups.Dash:
				if (Mathf.Abs(playerInfo.input.x) + Mathf.Abs(playerInfo.input.y) > 0) {
					Dash();
					playerUI.SetDriveText(usedPowerup, powerupNames[0]);
					powerups[usedPowerup] = Powerups.None;
				}
				break;
			case Powerups.Zhonya:
				StartCoroutine("StartZhonya");
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
				break;
			case Powerups.Freeze:
				StartCoroutine("Freeze");
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
				break;
			default:
				break;
		}
	}
	
	public void Dash() {
		if (!playerInfo.onDash) {
      source.PlayOneShot(dash, 1);
      playerInfo.onDash = true;
			playerInfo.inputEnabled = false;
			playerInfo.attack = true;
			playerInfo.moveSpeed *= dashSpeedMultiplier;
			dashTimeRemaining = 0f;
			playerInfo.accelerationTime = 0f;
		} else if (dashTimeRemaining < dashTime) {
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


	IEnumerator Freeze() {
		playerInfo.onFreeze = true;
		userFreezeTimeRemaining = 0f;
		StopMovement();
		source.PlayOneShot(freeze, 1);
		Color color = new Color(0f, 0f, 255f, 0.7f);
		ChangeColor(color);

		while (userFreezeTimeRemaining < userFreezeTime) {
			userFreezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		playerInfo.onFreeze = false;
		RecoverMovement();
		source.PlayOneShot(freeze, 1);
		Color defaultColor = new Color(255f, 255f, 255f, 1f);
		ChangeColor(defaultColor);
		Color frozenColor = new Color(0f, 0f, 255f, 0.7f);
		freezeTimeRemaining = 0f;
		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.StopMovement();
				enemy.ChangeColor(frozenColor);
			}
		}

		while (freezeTimeRemaining < freezeTime) {
			freezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.RecoverMovement();
				enemy.ChangeColor(defaultColor);
			}
		}
	}

}
