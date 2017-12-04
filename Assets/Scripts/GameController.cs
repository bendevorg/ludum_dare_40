using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public GameObject menuUI;
	public GameObject gameOverUI;
	public GameObject pauseUI;

	List<LivingEntity> players;

	void Awake(){
		if(gameController != null){
			Destroy(gameObject);
		} else {
			gameController = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start(){
		SceneManager.activeSceneChanged += GetPlayers;
		menuUI.SetActive(true);
		gameOverUI.SetActive(false);
		pauseUI.SetActive(false);
	}

	void Update(){
		if (Input.GetButtonDown("Submit")){
			PauseGame();
		}
	}

	void GetPlayers(Scene fromScene, Scene toScene){
		players = new List<LivingEntity>();
		GameObject[] playersGameObjects = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in playersGameObjects){
			LivingEntity playerLivingEntity = player.GetComponent<LivingEntity>();
			playerLivingEntity.OnDeath += PlayerDeath;
			players.Add(playerLivingEntity);
		}
	}

	void PlayerDeath(){
		players.RemoveAt(0);
		if (players.Count <= 1){
			Time.timeScale = 0;
			GameOver();
		}
	}

	void GameOver(){
		gameOverUI.SetActive(true);
		Time.timeScale = 0;
	}

	void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
		pauseUI.SetActive(!pauseUI.activeSelf);
	}

	public void LoadScene(int scene){
		ResetUI();
		if (scene == 0){
			menuUI.SetActive(true);
		}
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	public void RestartScene(){
		LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void ResetUI(){
		menuUI.SetActive(false);
		gameOverUI.SetActive(false);
		pauseUI.SetActive(false);
	}
}
