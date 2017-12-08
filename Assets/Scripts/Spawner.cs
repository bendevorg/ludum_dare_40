using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Pickup pickup;
	[HideInInspector]
	public Pickup instantiatedPickup = null;

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
		if (instantiatedPickup == null)
			SpawnPowerup();
	}

	void SpawnPowerup(){
		if (timeToSpawnReamining > nextTimeToSpawn){
			// Spawn powerup
			nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
			float spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
			float spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);
			while(Physics2D.Raycast(new Vector2(spawnX, spawnY), Vector2.right, .5f, wallMask)){
				spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
				spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);
			}
			Vector3 spawnLocation = new Vector3(spawnX, spawnY, -2f);
			instantiatedPickup =  Instantiate(pickup, spawnLocation, Quaternion.identity);
			instantiatedPickup.OnPickup += ClearPowerup;
			timeToSpawnReamining = 0f;
		} else {
			timeToSpawnReamining += Time.deltaTime;
		}
	}

	void ClearPowerup(){
		instantiatedPickup = null;
	}

}
