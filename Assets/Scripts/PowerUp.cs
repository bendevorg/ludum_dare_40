using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public int amountOfPowerups = 3;
	int powerUp;

	void Start(){
		powerUp = Random.Range(1, amountOfPowerups + 1);
	}

	public int GetPowerup(){
		return powerUp;
	}
}
