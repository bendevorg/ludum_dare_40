using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	void Awake(){
		if(gameController != null){
			Destroy(gameObject);
		} else {
			gameController = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Update(){
		if (Input.GetButton("Submit")){
			PauseGame();
		}
	}

	void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
	}

	public void LoadScene(int scene){
		 SceneManager.LoadScene(scene);
	}
}
