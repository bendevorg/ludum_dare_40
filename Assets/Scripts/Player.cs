using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;

	Vector2 input;
	public float speed = 2f;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
	}

	void FixedUpdate(){
		controller.Move(input * Time.deltaTime * speed);
	}
}
