using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerUI))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;
	BoxCollider2D collider;
	Animator animator;
	[HideInInspector]
	public PlayerInfo playerInfo;
	PlayerUI playerUI;
	public AudioClip zonyas;
	public AudioClip freeze;
	public AudioClip stun;
	public AudioClip pickup;
  public AudioClip dash;
	public AudioClip death;
  private AudioSource source;

	SpriteRenderer spriteRenderer;

	public float moveSpeed = 20f;
	float accelerationTime = .1f;

	public Vector2 playerInput;
	List<Controller2D> otherPlayers;

	public enum Powerups { None = -1, Dash = 0, Zhonya = 1, Freeze = 2 };

	string[] powerupNames = new string[] { "None", "Dash", "Zhonya", "Freeze" };
	public Powerups[] powerups = new Powerups[] { Powerups.None, Powerups.None };

	public float dashSpeedMultiplier = 2.5f;
	public float dashTime = .75f;
	float dashTimeRemaining;

	public float zhonyaTime = 2f;
	float zhonyaTimeRemaining;
	public float zhonyaDrawbackTime = 2f;
	float zhonyaDrawbackTimeRemaining;

	public float userFreezeTime = 3f;
	float userFreezeTimeRemaining;
	public float freezeTime = 3f;
	float freezeTimeRemaining;

	void Awake() {
		playerInfo = new PlayerInfo(moveSpeed, accelerationTime, Vector3.zero);
		source = GetComponent<AudioSource>();
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerUI = GetComponent<PlayerUI>();

		otherPlayers = new List<Controller2D>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			if (player.transform != this.transform) {
				otherPlayers.Add(player.GetComponent<Controller2D>());
			}
		}
		LivingEntity livingEntity = GetComponent<LivingEntity>();
		livingEntity.OnDeath += TriggerDeathAnimation;
		livingEntity.OnDeath += PlayDeathSFX;
	}

	public void Move(Vector2 moveAmount, Vector2 input) {
		rb.MovePosition(rb.position + moveAmount);
		playerInput = input;

		//	Update animation
		animator.SetFloat("inputX", input.x);
		animator.SetFloat("inputY", input.y);
		if (input.x != 0 || input.y != 0) {
			animator.SetBool("isWalking", true);
			animator.SetFloat("lastInputX", input.x);
			animator.SetFloat("lastInputY", input.y);
		} else {
			animator.SetBool("isWalking", false);
		}
	}

	public void UsePowerup(int usedPowerup) {
		switch (powerups[usedPowerup]) {
			case Powerups.Dash:
				if (Mathf.Abs(playerInfo.input.x) + Mathf.Abs(playerInfo.input.y) > 0) {
					Dash();
					playerUI.SetDriveText(usedPowerup, powerupNames[0]);
					powerups[usedPowerup] = Powerups.None;
				}
				break;
			case Powerups.Zhonya:
				Zhonya();
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
				break;
			case Powerups.Freeze:
				StartCoroutine("Freeze");
				playerUI.SetDriveText(usedPowerup, powerupNames[0]);
				powerups[usedPowerup] = Powerups.None;
				break;
			default:
				break;
		}
	}

	public void Dash() {
		if (!playerInfo.onDash) {
      source.PlayOneShot(dash, 1);
      playerInfo.onDash = true;
			playerInfo.inputEnabled = false;
			playerInfo.attack = true;
			playerInfo.moveSpeed *= dashSpeedMultiplier;
			dashTimeRemaining = 0f;
			playerInfo.accelerationTime = 0f;
		} else if (dashTimeRemaining < dashTime) {
			dashTimeRemaining += Time.deltaTime;
		} else {
			playerInfo.onDash = false;
			playerInfo.inputEnabled = true;
			playerInfo.attack = false;
			playerInfo.moveSpeed /= dashSpeedMultiplier;
			playerInfo.accelerationTime = accelerationTime;
			playerInfo.velocity.x = 0;
			playerInfo.velocity.y = 0;
		}
	}

	public void Zhonya() {
		if (!playerInfo.onZhonya) {
			playerInfo.onZhonya = true;
			StopMovement();
			source.PlayOneShot(zonyas, 1);
			collider.isTrigger = true;
			zhonyaTimeRemaining = 0f;
			zhonyaDrawbackTimeRemaining = 0f;

			// Color
			Color temp = spriteRenderer.color;
			temp.a = 0.2f;
			spriteRenderer.color = temp;
		} else if (zhonyaTimeRemaining < zhonyaTime) {
			zhonyaTimeRemaining += Time.deltaTime;
		} else if (zhonyaDrawbackTimeRemaining < zhonyaDrawbackTime) {
			if (collider.isTrigger) {
				source.Stop();
				source.PlayOneShot(stun, 1);
				animator.SetBool("isStunned", true);
				collider.isTrigger = false;
				// Color
				Color temp = spriteRenderer.color;
				temp.a = 1f;
				spriteRenderer.color = temp;
			}
			zhonyaDrawbackTimeRemaining += Time.deltaTime;
		} else {
			playerInfo.onZhonya = false;
			animator.SetBool("isStunned", false);
			RecoverMovement();
		}
	}

	IEnumerator Freeze() {
		playerInfo.onFreeze = true;
		userFreezeTimeRemaining = 0f;
		StopMovement();
		source.PlayOneShot(freeze, 1);
		Color color = new Color(0f, 0f, 255f, 0.7f);
		ChangeColor(color);

		while (userFreezeTimeRemaining < userFreezeTime) {
			userFreezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		playerInfo.onFreeze = false;
		RecoverMovement();
		source.PlayOneShot(freeze, 1);
		Color defaultColor = new Color(255f, 255f, 255f, 1f);
		ChangeColor(defaultColor);
		Color frozenColor = new Color(0f, 0f, 255f, 0.7f);
		freezeTimeRemaining = 0f;
		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.StopMovement();
				enemy.ChangeColor(frozenColor);
			}
		}

		while (freezeTimeRemaining < freezeTime) {
			freezeTimeRemaining += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		foreach (Controller2D enemy in otherPlayers) {
			if (enemy != null) {
				enemy.RecoverMovement();
				enemy.ChangeColor(defaultColor);
			}
		}
	}

	public void StopMovement() {
		playerInfo.inputEnabled = false;
		playerInfo.moveSpeed = 0;
		playerInfo.accelerationTime = 0;
		playerInfo.velocity.x = 0;
		playerInfo.velocity.y = 0;
	}

	public void ChangeColor(Color color) {
		spriteRenderer.color = color;
	}

	public void RecoverMovement() {
		playerInfo.inputEnabled = true;
		playerInfo.moveSpeed = moveSpeed;
		playerInfo.accelerationTime = accelerationTime;
	}

	void TriggerDeathAnimation() {
		animator.SetBool("isDead", true);
	}

	void PlayDeathSFX(){
		source.PlayOneShot(death, 1.5f);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Powerup") {
			PowerUp pickedPowerup = GetComponent<PowerUp>();
			int newPowerup = pickedPowerup.GetPowerup();
      source.PlayOneShot(pickup, 1);
      //	TODO: Redo this to be more flexible
      if (powerups[0] == Powerups.None) {
				powerups[0] = (Powerups) newPowerup;
				playerUI.SetDriveText(0, powerupNames[newPowerup + 1]);
			} else if (powerups[1] == Powerups.None) {
				powerups[1] = (Powerups) newPowerup;
				playerUI.SetDriveText(1, powerupNames[newPowerup + 1]);
			} else {
				powerups[0] = (Powerups) newPowerup;
				playerUI.SetDriveText(0, powerupNames[newPowerup + 1]);
			}
			pickedPowerup.Destroy();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Enemy" && playerInfo.attack) {
			other.collider.GetComponent<LivingEntity>().TakeDamage(5);
		} else if (other.collider.tag == "Wall" && playerInfo.onDash) {
			transform.GetComponent<LivingEntity>().TakeDamage(99999);
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.tag == "Enemy" && playerInfo.attack) {
			other.collider.GetComponent<LivingEntity>().TakeDamage(5);
		} //else if (other.collider.tag == "Wall" && playerInfo.onDash){
		//transform.GetComponent<LivingEntity>().TakeDamage(99999);
		//}
	}

}
