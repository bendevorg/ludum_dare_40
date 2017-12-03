using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public PowerUp powerUp;

	public BoxCollider2D map;
	Bounds mapSize;
	float mapOffsetX;
	float mapOffsetY;

	public float minTimeToSpawn = 6f;
	public float maxTimeToSpawn = 15f;
	float nextTimeToSpawn;

	// Use this for initialization
	void Start () {
		mapSize = map.bounds;
		nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn) + Time.time;
		mapOffsetX = (mapSize.min.x - mapSize.max.x)/10;
		mapOffsetY = (mapSize.min.y - mapSize.max.y)/10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnPowerup(){
		if (nextTimeToSpawn <= Time.time){
			// Spawn powerup
			nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn) + Time.time;
			float spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
			float spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);
			Vector3 spawnLocation = new Vector3(spawnX, spawnY, -1f);
			Instantiate(powerUp, spawnLocation, Quaternion.identity);
		}
	}

}
