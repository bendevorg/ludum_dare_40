using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public GameObject gameOverUI;
	public GameObject pauseUI;

	void Awake(){
		if(gameController != null){
			Destroy(gameObject);
		} else {
			gameController = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start(){
		gameOverUI.SetActive(false);
		pauseUI.SetActive(false);
	}

	void Update(){
		if (Input.GetButtonDown("Submit")){
			PauseGame();
		}
	}

	void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
		pauseUI.SetActive(!pauseUI.activeSelf);
	}

	public void LoadScene(int scene){
		SceneManager.LoadScene(scene);
	}
}
