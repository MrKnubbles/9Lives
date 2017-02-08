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
	public GameObject page3;
	// public AudioSource music;
	// public AudioSource sound;
	public GameManager gameManager;
	public GameObject gameMan;
	public LevelManager levelManager;
	public GameObject HUD;
	public float currentLevel;
	public string currentLevelName;
	public string nextLevelName;
	public UnityAds unityAds;
	private AudioManager audioManager;

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
			gameMan = GameObject.Find("GameManager");
			gameManager = gameMan.GetComponent<GameManager>();
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
		levelManager.levelCounter++;
		if (levelManager.levelCounter >= 5){
			unityAds.ShowAd("next");
			levelManager.levelCounter = 0;
		}
		else{
			Time.timeScale = 1;
			gameManager.isPaused = false;
			SceneManager.LoadScene(nextLevelName);
		}
	}

	public void LoadLevelAfterAd(){
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
	}

	public void ReplayLevel(){
		levelManager.replayCounter++;
		if (levelManager.replayCounter >= 3){
			unityAds.ShowAd("restart");
			levelManager.replayCounter = 0;
		}
		else{
			Time.timeScale = 1;
			gameManager.isPaused = false;
			SceneManager.LoadScene(currentLevelName);
		}
	}

	// Only use this within UnityAds to resume gameplay after ad finishes.
	public void RestartLevelAfterAd(){
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
		shopScreen.SetActive(false);
		mainMenuScreen.SetActive(true);
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

		// if (musicMuted.activeSelf){
		// 	musicMuted.SetActive(false);
		// }
		// else{
		// 	musicMuted.SetActive(true);
		// }
	}

	public void MuteSound(){
		audioManager.MuteSFX();

		// if (soundMuted.activeSelf){
		// 	soundMuted.SetActive(false);
		// }
		// else{
		// 	soundMuted.SetActive(true);
		// }
	}

	public void ShowOptions(){
		optionsMenuScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void ShowStore(){
		shopScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
		shopScreen.GetComponent<Shop>().CharactersButton();
	}

	public void ShowCredits(){
		optionsMenuScreen.SetActive(false);
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