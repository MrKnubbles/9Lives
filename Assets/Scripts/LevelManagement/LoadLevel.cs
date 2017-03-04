using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
	public Player player;
	public GameObject worldSelectScreen;
	public GameObject levelSelectScreen;
	public GameObject[] levelScreens;
	public GameObject controlsScreen;
	public GameObject mainMenuScreen;
	public GameObject pauseMenuScreen;
	public GameObject optionsMenuScreen;
	public GameObject creditsScreen;
	public GameObject devOptionsScreen;
	public GameObject shopScreen;
	public GameObject musicMuted;
	public GameObject soundMuted;
	public GameObject pauseButton;
	public GameManager gameManager;
	public LevelManager levelManager;
	public GameObject HUD;
	public float currentLevel;
	public string currentLevelName;
	public string nextLevelName;
	public UnityAds unityAds;
	private AudioManager audioManager;
	public ScoreTracker scoreTracker;

	void Start(){
		Time.timeScale = 1;
		HUD = GameObject.Find("HUD");
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("OptionsScreen/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("OptionsScreen/MuteSoundButton/SoundMuted").gameObject;
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("Pause Menu/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("Pause Menu/MuteSoundButton/SoundMuted").gameObject;
			pauseButton = HUD.transform.Find("ControllerBar/PauseButton").gameObject;
			pauseMenuScreen = HUD.transform.Find("Pause Menu").gameObject;
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			scoreTracker = HUD.GetComponent<ScoreTracker>();
			//gameManager = gameMan.GetComponent<GameManager>();
			currentLevel = float.Parse(SceneManager.GetActiveScene().name);
			currentLevelName = "" + currentLevel;
			nextLevelName = "" + (currentLevel + 1);
		}
		audioManager = AudioManager.Instance;
		levelManager = LevelManager.Instance;
	}

	void Update(){
		// If you are in the Main scene and the Options menu is open... show if music/sound is muted.
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main") && optionsMenuScreen.activeSelf){
			if (PlayerPrefs.GetInt("MusicMuted") == -1){
				musicMuted.SetActive(true);
			}
			else{
				musicMuted.SetActive(false);
			}
			if (PlayerPrefs.GetInt("SFXMuted") == -1){
				soundMuted.SetActive(true);
			}
			else{
				soundMuted.SetActive(false);
			}
		}
		// If you are in any level scene and the Pause menu is open... show if music/sound is muted.
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main") && pauseMenuScreen.activeSelf){
			if (PlayerPrefs.GetInt("MusicMuted") == -1){
				musicMuted.SetActive(true);
			}
			else{
				musicMuted.SetActive(false);
			}
			if (PlayerPrefs.GetInt("SFXMuted") == -1){
				soundMuted.SetActive(true);
			}
			else{
				soundMuted.SetActive(false);
			}
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
		if (PlayerPrefs.GetInt("RemoveAds") != 1){
			PlayerPrefs.SetInt("LevelAdCounter", PlayerPrefs.GetInt("LevelAdCounter") + 1);
			if (PlayerPrefs.GetInt("LevelAdCounter") >= 5){
				unityAds.ShowAd("next");
				PlayerPrefs.SetInt("LevelAdCounter", 0);
			}
		}
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
	}

	public void LoadLevelAfterAd(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
	}

	public void ReplayLevel(){
		if (PlayerPrefs.GetInt("RemoveAds") != 1){
			levelManager.replayCounter++;
			if (levelManager.replayCounter >= 3){
				unityAds.ShowAd("restart");
				levelManager.replayCounter = 0;
			}
		}
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(currentLevelName);
	}

	// Only use this within UnityAds to resume gameplay after ad finishes.
	public void RestartLevelAfterAd(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(currentLevelName);
	}

	public void ShowLevelSelect(int worldNumber){
		HideWorlds();
		levelSelectScreen.SetActive(true);
		levelScreens[worldNumber - 1].SetActive(true);
		mainMenuScreen.SetActive(false);
		HidePages();
		ShowPage(1);
	}

	public void ShowDevOptions(){
		DisableAllScreens();
		devOptionsScreen.SetActive(true);
	}

	public void ShowWorldSelect(){
		DisableAllScreens();
		worldSelectScreen.SetActive(true);
	}

	public void ShowControls(){
		DisableAllScreens();
		controlsScreen.SetActive(true);
	}

	public void BackButton(){
		DisableAllScreens();
		mainMenuScreen.SetActive(true);
	}

	private void DisableAllScreens(){
		levelSelectScreen.SetActive(false);
		worldSelectScreen.SetActive(false);
		controlsScreen.SetActive(false);
		optionsMenuScreen.SetActive(false);
		creditsScreen.SetActive(false);
		shopScreen.SetActive(false);
		devOptionsScreen.SetActive(false);
		mainMenuScreen.SetActive(false);
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void PauseGame(){
		gameManager.PauseGame();
		pauseMenuScreen.SetActive(true);
		pauseButton.SetActive(false);
	}

	public void ResumeGame(){
		gameManager.UnpauseGame();
		pauseMenuScreen.SetActive(false);
		pauseButton.SetActive(true);
	}

	public void MuteMusic(){
		audioManager.MuteMusic();
	}

	public void MuteSound(){
		audioManager.MuteSFX();
	}

	public void ShowOptions(){
		DisableAllScreens();
		optionsMenuScreen.SetActive(true);
	}

	public void ShowStore(){
		DisableAllScreens();
		shopScreen.SetActive(true);
		shopScreen.GetComponent<Shop>().SetDefaultShopState();
	}

	public void ShowSkipLevel(){
		scoreTracker.skipLevelConfirmation.SetActive(true);
		scoreTracker.levelOverScreen.SetActive(false);
		scoreTracker.skipLevelText.GetComponent<Text>().text = "Are you sure you want to use (1) Skip to skip this level?\n\nYou will have (" + (PlayerPrefs.GetInt("Skip") - 1) + ") Skips remaining.";
	}

	public void ConfirmSkipLevel(){
		SkipLevel();
	}

	public void CloseSkipLevel(){
		scoreTracker.levelOverScreen.SetActive(true);
		scoreTracker.skipLevelConfirmation.SetActive(false);
	}

	private void SkipLevel(){
		if (Int32.Parse(SceneManager.GetActiveScene().name) <= 44){
			PlayerPrefs.SetInt("World1PlayerLevel", PlayerPrefs.GetInt("World1PlayerLevel") + 1);
		}
		else if (Int32.Parse(SceneManager.GetActiveScene().name) >= 46 && Int32.Parse(SceneManager.GetActiveScene().name) <= 89){
			PlayerPrefs.SetInt("World2PlayerLevel", PlayerPrefs.GetInt("World2PlayerLevel") + 1);
		}
		else if (Int32.Parse(SceneManager.GetActiveScene().name) >= 91 && Int32.Parse(SceneManager.GetActiveScene().name) <= 134){
			PlayerPrefs.SetInt("World3PlayerLevel", PlayerPrefs.GetInt("World3PlayerLevel") + 1);
		}
		PlayerPrefs.SetInt("Skip", PlayerPrefs.GetInt("Skip") - 1);
		LoadNextLevel();
	}

	public void ShowCredits(){
		DisableAllScreens();
		creditsScreen.SetActive(true);
	}

	public void ShowPage(int pageNumber){
		if (levelManager.worlds[0].activeSelf){
			levelManager.levelPages[pageNumber-1].SetActive(true);
		}
		else if (levelManager.worlds[1].activeSelf){
			levelManager.levelPages[pageNumber+2].SetActive(true);
		}
		
		levelManager.pageTracker.transform.GetChild(pageNumber-1).gameObject.SetActive(true);
	}

	public void ShowNextPage(int pageNumber){
		HidePages();
		ShowPage(pageNumber);
	}

	void HidePages(){
		for (int i = 0; i < levelManager.levelPages.Length; i++){
			levelManager.levelPages[i].SetActive(false);
			if (i < 3){
				levelManager.pageTracker.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public void ShowNextWorld(int worldNumber){
		HideWorlds();
		ShowWorld(worldNumber);
		HidePages();
		ShowPage(1);
	}

	void ShowWorld(int worldNumber){
		levelManager.worlds[worldNumber-1].SetActive(true);
		SetDefaultPage();
	}

	void HideWorlds(){
		for (int i = 0; i < levelManager.worlds.Length; i++){
			levelManager.worlds[i].SetActive(false);
		}
	}

	void SetDefaultPage(){
		levelManager.levelPages[0].SetActive(true);
		levelManager.levelPages[1].SetActive(false);
		levelManager.levelPages[2].SetActive(false);
	}
}