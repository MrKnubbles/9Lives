using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
	public PlayerMain player;
	[SerializeField] PlayerCanvas playerCanvas;
	[SerializeField] GameObject playerCanvasGO;
	[SerializeField] GameObject playerCanvasPrefab;
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
	private float speed = 10f;
	private Vector2 moveLocation;
	private Ray ray;
	private float distance = 188f;
	private float moveDistance;
	private bool isPlayerMoving = false;
	private int worldLevelNumber;
	private string playerDirection;
	private MovePlayer playerMovement;
	private Vector3 playerFacing;

	void Start(){
		Time.timeScale = 1;
		HUD = GameObject.Find("HUD");
		InitPlayerCanvas();
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("OptionsScreen/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("OptionsScreen/MuteSoundButton/SoundMuted").gameObject;
			playerMovement = player.transform.gameObject.GetComponent<MovePlayer>();
			playerMovement.SetSpeed(speed);
			playerFacing = player.transform.localScale;
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("Pause Menu/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("Pause Menu/MuteSoundButton/SoundMuted").gameObject;
			pauseButton = HUD.transform.Find("ControllerBar/PauseButton").gameObject;
			pauseMenuScreen = HUD.transform.Find("Pause Menu").gameObject;
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			scoreTracker = HUD.GetComponent<ScoreTracker>();
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

	void LateUpdate(){
		// Controls player movement on the World / Level Select Screen.
		// Lets the player move to one location at a time.
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			if (worldSelectScreen.activeSelf){
				if (playerDirection == "right"){
					if (player.transform.position.x < moveLocation.x && isPlayerMoving && !playerMovement.isObjectMoving()){
						playerMovement.SetDistanceX(moveDistance);
						playerMovement.Move();
						isPlayerMoving = false;
					}
					else if (!playerMovement.isObjectMoving() && playerMovement.isDoneMoving()){
						ShowLevelSelect(worldLevelNumber);
						player.animator.SetBool("isRunning", false);
					}
				}
				else if (playerDirection == "left"){
					if (player.transform.position.x > moveLocation.x && isPlayerMoving && !playerMovement.isObjectMoving()){
						playerMovement.SetDistanceX(moveDistance);
						playerMovement.Move();
						isPlayerMoving = false;
					}
					else if (!playerMovement.isObjectMoving() && playerMovement.isDoneMoving()){
						ShowLevelSelect(worldLevelNumber);
						player.animator.SetBool("isRunning", false);
					}
				}
				else if (playerDirection == "none"){
					isPlayerMoving = false;
					ShowLevelSelect(worldLevelNumber);
					player.animator.SetBool("isRunning", false);
				}
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
		if (playerCanvas.GetLives() <= 0){
			unityAds.ShowAd("next");
		}
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
	}

	public void LoadLevelAfterAd(){
		playerCanvas.AddLives(1);
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(nextLevelName);
	}

	public void ReplayLevel(){
		if (playerCanvas.GetLives() <= 0){
			unityAds.ShowAd("restart");
		}
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(currentLevelName);
	}

	// Only use this within UnityAds to resume gameplay after ad finishes.
	public void RestartLevelAfterAd(){
		playerCanvas.AddLives(1);
		Time.timeScale = 1;
		gameManager.isPaused = false;
		SceneManager.LoadScene(currentLevelName);
	}

	// Sets the location for the player to move to on the WorldSelectScreen.
	public void SetPlayerMoveLocation(int worldNumber){
		int roundedValue = RoundDown(worldNumber);
		int firstDigit = roundedValue/10;
		int secondDigit = worldNumber - roundedValue;
		// Move location is the building you selected.
		moveLocation = GameObject.Find("HUD/WorldSelectScreen/WorldsScrollView/Viewport/WorldsSet").transform.GetChild((firstDigit * 3) - (4 - secondDigit)).transform.position;
		moveDistance = (moveLocation.x - player.transform.position.x);
		worldLevelNumber = worldNumber;
		isPlayerMoving = true;
		// Makes player run to the right.
		if (player.transform.position.x < moveLocation.x){
			playerDirection = "right";
			player.animator.SetBool("isRunning", true);
			if (playerFacing.x < 0){
				playerFacing.x *= -1;
				player.transform.localScale = playerFacing;
			}
		}
		// Makes player run to the left.
		else if (player.transform.position.x > moveLocation.x){
			playerDirection = "left";
			player.animator.SetBool("isRunning", true);
			if (playerFacing.x > 0){
				playerFacing.x *= -1;
				player.transform.localScale = playerFacing;
			}
		}
		else {
			playerDirection = "none";
			player.animator.SetBool("isRunning", false);
		}
	}

	public void ShowLevelSelect(int worldNumber){
		int roundedValue = RoundDown(worldNumber);
		int firstDigit = roundedValue/10;
		int secondDigit = worldNumber - roundedValue;
		HideWorlds();
		levelSelectScreen.SetActive(true);
		levelScreens[worldNumber - (roundedValue + secondDigit) - 1 + (1 * firstDigit)].SetActive(true);
		mainMenuScreen.SetActive(false);
		HidePages();
		ShowPage(secondDigit);
	}

	int RoundDown(int toRound)
	{
		return toRound - toRound % 10;
	}

	public void ShowDevOptions(){
		DisableAllScreens();
		devOptionsScreen.SetActive(true);
	}

	public void ShowWorldSelect(){
		DisableAllScreens();
		worldSelectScreen.SetActive(true);
		player.Start();
	}

	public void ShowControls(){
		DisableAllScreens();
		controlsScreen.SetActive(true);
	}

	public void BackButton(){
		DisableAllScreens();
		mainMenuScreen.SetActive(true);
	}

	// Use this before activating any screen.
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
		// Skips a level in World 1.
		if (Int32.Parse(SceneManager.GetActiveScene().name) <= 44){
			PlayerPrefs.SetInt("World1PlayerLevel", PlayerPrefs.GetInt("World1PlayerLevel") + 1);
		}
		// Skips a level in World 2.
		else if (Int32.Parse(SceneManager.GetActiveScene().name) >= 46 && Int32.Parse(SceneManager.GetActiveScene().name) <= 89){
			PlayerPrefs.SetInt("World2PlayerLevel", PlayerPrefs.GetInt("World2PlayerLevel") + 1);
		}
		// Skips a level in World 3.
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
		
		//levelManager.pageTracker.transform.GetChild(pageNumber-1).gameObject.SetActive(true);
	}

	// public void ShowNextPage(int pageNumber){
	// 	HidePages();
	// 	ShowPage(pageNumber);
	// }

	void HidePages(){
		for (int i = 0; i < levelManager.levelPages.Length; i++){
			levelManager.levelPages[i].SetActive(false);
			// if (i < 3){
			// 	levelManager.pageTracker.transform.GetChild(i).gameObject.SetActive(false);
			// }
		}
	}

	void InitPlayerCanvas() {
		playerCanvasGO = GameObject.Find("PlayerCanvas");
		if(playerCanvasGO == null) {
			GameObject tmp = GameObject.Instantiate(playerCanvasPrefab);
			tmp.name = "PlayerCanvas";
			playerCanvas = tmp.GetComponent<PlayerCanvas>();
		} else {
			playerCanvas = playerCanvasGO.GetComponent<PlayerCanvas>();
		}
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