using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	public Player player;
	public GameObject levelSelectScreen;
	public GameObject controlsScreen;
	public GameObject mainMenuScreen;
	public GameObject pauseMenuScreen;
	public GameObject optionsMenuScreen;
	public GameObject creditsScreen;
	public GameObject shopScreen;
	public GameObject musicMuted;
	public GameObject soundMuted;
	public GameObject pauseButton;
	public GameObject page1;
	public GameObject page2;
	public AudioSource music;
	public AudioSource sound;
	public GameManager gameManager;
	public GameObject gameMan;
	public LevelManager levelManager;
	public GameObject HUD;
	public float currentLevel;
	public string currentLevelName;
	public string nextLevelName;
	public UnityAds unityAds;
	//public AudioManager audioManager = AudioManager.Instance;

	void Start(){
		Time.timeScale = 1;
		HUD = GameObject.Find("HUD");
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("OptionsScreen/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("OptionsScreen/MuteSoundButton/SoundMuted").gameObject;
		}
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("Pause Menu/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("Pause Menu/MuteSoundButton/SoundMuted").gameObject;
			pauseButton = HUD.transform.Find("PauseButton").gameObject;
			pauseMenuScreen = HUD.transform.Find("Pause Menu").gameObject;
			gameMan = GameObject.Find("GameManager");
			gameManager = gameMan.GetComponent<GameManager>();
			//sound = player.audio;
			currentLevel = float.Parse(SceneManager.GetActiveScene().name);
			currentLevelName = "" + currentLevel;
			nextLevelName = "" + (currentLevel + 1);
		}
		GameObject audioManager = GameObject.Find("AudioManager");
		music = audioManager.GetComponent<AudioSource>();
		levelManager = LevelManager.Instance;
	}

	void Update(){
		//TODO: Make sure sound targets Player sound and mutes it.
		//TODO: attach audio source to all game objects in scene.

		// If you are in the Main scene and the Options menu is open... show if music/sound is muted.
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main") && optionsMenuScreen.activeSelf){
			if (music.mute){
				musicMuted.SetActive(true);
			}
			else{
				musicMuted.SetActive(false);
			}
			// if (sound.mute == false){
			// 	soundMuted.SetActive(true);
			// }
		}
		// If you are in any level scene and the Pause menu is open... show if music/sound is muted.
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main") && pauseMenuScreen.activeSelf){
			//pauseButton = GameObject.Find("HUD/PauseButton");
			if (music.mute){
				musicMuted.SetActive(true);
			}
			else{
				musicMuted.SetActive(false);
			}
			// if (sound.mute == false){
			// 	soundMuted.SetActive(true);
			// }
		}
	}

	public void LoadMainMenu(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene("Main");
	}

	public void LoadNewLevel(string levelName){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(levelName);
	}

	public void LoadNextLevel(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
		//levelManager.(levelName).SetActive(false);
		//gameManager.UnlockLevel(levelName);
		// levelManager.UnlockLevel("" + levelName);
	}

	public void ReplayLevel(){
		//unityAds.ShowAd();
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(currentLevelName);
	}

	public void ShowLevelSelect(){
		levelSelectScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void ShowControls(){
		controlsScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void BackButton(){
		levelSelectScreen.SetActive(false);
		controlsScreen.SetActive(false);
		optionsMenuScreen.SetActive(false);
		creditsScreen.SetActive(false);
		//shopScreen.SetActive(false);
		mainMenuScreen.SetActive(true);
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void PauseGame(){
		Time.timeScale = 0;
		gameManager.isPaused = true;
		pauseMenuScreen.SetActive(true);
		pauseButton.SetActive(false);
	}

	public void ResumeGame(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		pauseMenuScreen.SetActive(false);
		pauseButton.SetActive(true);
	}

	public void MuteMusic(){
		if (!music.mute){
			music.mute = true;
			musicMuted.SetActive(true);
		}
		else {
			music.mute = false;
			musicMuted.SetActive(false);
		}
	}

	public void MuteSound(){
		//TODO: Make this button mute ALL sound.
		if (!sound.mute){
			sound.mute = true;
			soundMuted.SetActive(true);
		}
		else {
			sound.mute = false;
			soundMuted.SetActive(false);
		}
	}

	public void ShowOptions(){
		optionsMenuScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void ShowStore(){
		//TODO: Make a shop screen.
		//shopScreen.SetActive(true);
	}

	public void ShowCredits(){
		optionsMenuScreen.SetActive(false);
		creditsScreen.SetActive(true);
	}

	public string GetSceneName(){
		string levelName = "" + SceneManager.GetActiveScene();
		print ("level name is " + SceneManager.GetActiveScene());
		return levelName;
	}

	public void ShowPageOne(){
		page1.SetActive(true);
		page2.SetActive(false);
	}

	public void ShowPageTwo(){
		page1.SetActive(false);
		page2.SetActive(true);
	}
}