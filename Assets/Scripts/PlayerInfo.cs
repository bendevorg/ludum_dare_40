using UnityEngine;

public struct PlayerInfo {
	public bool onDash;
	public bool inputEnabled;
	public bool attack;
	public float moveSpeed;
	public float accelerationTime;
	public Vector3 velocity;

	public PlayerInfo(bool _onDash, bool _inputEnabled, bool _attack, float _moveSpeed, float _accelerationTime, Vector3 _velocity){
		onDash = _onDash;
		inputEnabled = _inputEnabled;
		attack = _attack;
		moveSpeed = _moveSpeed;
		accelerationTime = _accelerationTime;
		velocity = _velocity;
	}
}
