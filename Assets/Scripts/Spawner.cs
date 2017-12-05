using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public PowerUp powerUp;
	[HideInInspector]
	public PowerUp instantiatedPowerup = null;

	public Collider2D map;
	Bounds mapSize;
	float mapOffsetX;
	float mapOffsetY;

	public LayerMask wallMask;

	public float minTimeToSpawn = 6f;
	public float maxTimeToSpawn = 15f;
	float nextTimeToSpawn;
	float timeToSpawnReamining = 0f;

	// Use this for initialization
	void Start () {
		mapSize = map.bounds;
		nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
		mapOffsetX = Mathf.Abs(mapSize.min.x - mapSize.max.x)*0.2f;
		mapOffsetY = Mathf.Abs(mapSize.min.y - mapSize.max.y)*0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (instantiatedPowerup == null)
			SpawnPowerup();
	}

	void SpawnPowerup(){
		if (timeToSpawnReamining > nextTimeToSpawn){
			// Spawn powerup
			nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
			float spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
			float spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);

			while(Physics.Raycast(new Vector3(spawnX, spawnY, -10f), Vector3.forward, 10f, wallMask)){
				spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
				spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);
			}
			Vector3 spawnLocation = new Vector3(spawnX, spawnY, -2f);
			instantiatedPowerup =  Instantiate(powerUp, spawnLocation, Quaternion.identity);
			instantiatedPowerup.OnPickup += ClearPowerup;
			timeToSpawnReamining = 0f;
		} else {
			timeToSpawnReamining += Time.deltaTime;
		}
	}

	void ClearPowerup(){
		instantiatedPowerup = null;
	}

}
