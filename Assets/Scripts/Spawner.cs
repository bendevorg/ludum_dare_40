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
		mapOffsetX = Mathf.Abs(mapSize.min.x - mapSize.max.x)*0.2f;
		mapOffsetY = Mathf.Abs(mapSize.min.y - mapSize.max.y)*0.2f;
		Debug.Log(mapSize.min.x);
		Debug.Log(mapSize.max.x);
		Debug.Log(mapSize.min.y);
		Debug.Log(mapSize.max.y);
		Debug.Log(mapOffsetX);
		Debug.Log(mapOffsetY);
	}
	
	// Update is called once per frame
	void Update () {
		SpawnPowerup();
	}

	void SpawnPowerup(){
		if (nextTimeToSpawn <= Time.time){
			// Spawn powerup
			nextTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn) + Time.time;
			float spawnX = Random.Range(mapSize.min.x + mapOffsetX, mapSize.max.x - mapOffsetX);
			float spawnY = Random.Range(mapSize.min.y + mapOffsetY, mapSize.max.y - mapOffsetY);
			Vector3 spawnLocation = new Vector3(spawnX, spawnY, -2f);
			Instantiate(powerUp, spawnLocation, Quaternion.identity);
		}
	}

}
