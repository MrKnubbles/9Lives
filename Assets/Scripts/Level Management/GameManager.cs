using UnityEngine;
using UnityEngine.SceneManagement;
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
	public LoadLevel loadLevel;
	// public bool isLevelStarted = false;
	// public float gameSpeed;

	void Start(){
		audioManager = AudioManager.Instance;
		loadLevel = GameObject.Find("HUD").GetComponent<LoadLevel>();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
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

	// If the game is minimized while in a level, the game will bring up the PauseMenu.
	void OnApplicationPause(){
		if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
			loadLevel.PauseGame();
		}
	}
	public void PauseGame(){
		isPaused = true;
		Time.timeScale = 0;
		audioManager.PauseSFX();
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}

	public void UnpauseGame(){
		isPaused = false;
		Time.timeScale = 1;
		audioManager.UnpauseSFX();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
}