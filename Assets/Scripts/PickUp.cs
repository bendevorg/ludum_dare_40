using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	public event System.Action OnPickup;

	public int amountOfPowerups = 3;
	int powerUp;

	void Start(){
		powerUp = Random.Range(0, amountOfPowerups);
	}

	public int GetPowerup(){
		return powerUp;
	}

	public void Destroy(){
		if (OnPickup != null){
			OnPickup();
		}
		Destroy(this.gameObject);
	}
}
