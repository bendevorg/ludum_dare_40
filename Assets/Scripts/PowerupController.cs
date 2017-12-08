using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Zhonya))]
[RequireComponent(typeof(Freeze))]
[RequireComponent(typeof(Dash))]
public class PowerupController : MonoBehaviour {
	
	PlayerUI playerUI;

	public enum Powerups { None = -1, Dash = 0, Zhonya = 1, Freeze = 2 };

	string[] powerupNames = new string[] { "None", "Dash", "Zhonya", "Freeze" };
	public Powerups[] powerups = new Powerups[] { Powerups.None, Powerups.None };

	Zhonya zhonya;
	Freeze freeze;
	Dash dash;

	private AudioSource source;
	public AudioClip pickup;

	// Use this for initialization
	void Start () {
		playerUI = GetComponent<PlayerUI>();
		source = GetComponent<AudioSource>();
		zhonya = GetComponent<Zhonya>();
		freeze = GetComponent<Freeze>();
		dash = GetComponent<Dash>();
	}

	public void UsePowerup(int usedPowerup) {
		switch (powerups[usedPowerup]) {
			case Powerups.Dash:
				dash.Use();
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
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
