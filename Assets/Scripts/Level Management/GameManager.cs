using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// private static GameManager m_instance = null;
	// public static GameManager Instance { get { return m_instance; } }
	public GameObject gameOverScreen;
	public bool isPaused = false;
	public bool isGameOver = false;
	public bool isGameStarted = false;
	public bool isLevelComplete = false;
	private AudioManager audioManager;
	// public bool isLevelStarted = false;
	// public float gameSpeed;

	void Start(){
		audioManager = AudioManager.Instance;
	}
	
	// void Awake(){
	// 	if (m_instance != null && m_instance != this) {
	// 		Destroy(this.gameObject);
	// 		return;
	// 	}
	// 	else {
	// 		m_instance = this;
	// 		m_instance.name = "GameManager";

	// 		if (!GameBegin){
	// 			DontDestroyOnLoad(gameObject);
	// 			GameBegin = true;
	// 		}
	// 	}
	// 	if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
	// 		gameOverScreen = GameObject.Find("HUD/Game Over");
	// 	}
	// }

	// public void GameOver(){
	// 	Time.timeScale = 0;
	// 	gameOverScreen.SetActive(true);
	// }
	void Update(){
		if (isPaused){
			Time.timeScale = 0;
			audioManager.PauseSFX();
		}
		else{
			Time.timeScale = 1;
			audioManager.UnpauseSFX();
		}
	}
}