using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Zhonya))]
[RequireComponent(typeof(Freeze))]
public class PowerupController : MonoBehaviour {
	
	PlayerUI playerUI;

	public enum Powerups { None = -1, Dash = 0, Zhonya = 1, Freeze = 2 };

	string[] powerupNames = new string[] { "None", "Dash", "Zhonya", "Freeze" };
	public Powerups[] powerups = new Powerups[] { Powerups.None, Powerups.None };

	Zhonya zhonya;
	Freeze freeze;
	
	public float dashSpeedMultiplier = 2.5f;
	public float dashTime = .75f;
	float dashTimeRemaining;

	private AudioSource source;
	public AudioClip pickup;
  public AudioClip dash;

	// Use this for initialization
	void Start () {
		playerUI = GetComponent<PlayerUI>();
		source = GetComponent<AudioSource>();
		zhonya = GetComponent<Zhonya>();
		freeze = GetComponent<Freeze>();
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
				zhonya.Use();
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
				break;
			case Powerups.Freeze:
				freeze.Use();
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

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Pickup") {
			Pickup pickedPowerup = collider.GetComponent<Pickup>();
			int newPowerup = pickedPowerup.GetPowerup();
      source.PlayOneShot(pickup, 1);
      //	TODO: Redo this to be more flexible
      if (powerups[0] == Powerups.None) {
				powerups[0] = (Powerups) newPowerup;
				playerUI.SetDriveText(0, powerupNames[newPowerup + 1]);
			} else if (powerups[1] == Powerups.None) {
				powerups[1] = (Powerups) newPowerup;
				playerUI.SetDriveText(1, powerupNames[newPowerup + 1]);
			} else {
				powerups[0] = (Powerups) newPowerup;
				playerUI.SetDriveText(0, powerupNames[newPowerup + 1]);
			}
			pickedPowerup.Destroy();
		}
	}

}
