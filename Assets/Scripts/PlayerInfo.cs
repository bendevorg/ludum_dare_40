using UnityEngine;

public struct PlayerInfo {
	public bool onDash;
	public bool onZhonya;
	public bool onFreeze;
	public bool inputEnabled;
	public bool attack;
	public float moveSpeed;
	public float accelerationTime;
	public Vector3 velocity;
	public Vector2 input;

	public PlayerInfo(float _moveSpeed, float _accelerationTime, Vector3 _velocity){
		onDash = false;
		onZhonya = false;
		onFreeze = false;
		inputEnabled = true;
		attack = false;
		moveSpeed = _moveSpeed;
		accelerationTime = _accelerationTime;
		velocity = _velocity;
		input = Vector2.zero;
	}
}
