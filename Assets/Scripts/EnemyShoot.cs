using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyShoot : MonoBehaviour {

	public float timeBetweenShoots = 4f;
	float timeRemainingToEnableShoot = 0;

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	}

	public void Shoot(Vector3 targetPosition){
		if (timeRemainingToEnableShoot < Time.time){
			float AngleRad = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x);
			float AngleDeg = (180 / Mathf.PI) * AngleRad;
			GameObject spawnedBullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, AngleDeg));
			timeRemainingToEnableShoot = Time.time + timeBetweenShoots;
		}
	}
}
