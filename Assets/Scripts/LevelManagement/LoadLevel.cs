using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
	// Objects required for the Player on the Main Menu
	public PlayerMain playerMain;
	private MovePlayer playerMainMovement;
	public PlayerCanvas playerCanvas;
	[SerializeField] GameObject playerCanvasGO;
	[SerializeField] GameObject playerCanvasPrefab;
	private Vector3 playerMainFacing;
	private Vector2 moveLocation;
	public GameObject startLocation;
	public bool isPlayerMainReady = true;
	private bool isPlayerMainMoving = false;
	private string playerMainDirection;
	private int worldLevelNumber;
	private float distance = 188f;
	private float moveDistance;
	private float speed = 10f;
	// Objects required for the Player on the World Select / Level Select
	public PlayerMain playerLevel;
	private MovePlayer playerMovement;
	private Vector3 playerFacing;
	private bool isPlayerLevelReady = true;
	private bool isPlayerMoving = false;
	private string playerDirection;
	private int houseObjectNumber;
	// Menu Screens
	public GameObject HUD;
	public GameObject worldSelectScreen;
	public GameObject levelSelectScreen;
	public GameObject[] levelScreens;
	public GameObject controlsScreen;
	public GameObject mainMenuScreen;
	public GameObject pauseMenuScreen;
	public GameObject optionsMenuScreen;
	public GameObject creditsScreen;
	public GameObject devOptionsScreen;
	// UI
	public GameObject musicMuted;
	public GameObject soundMuted;
	public GameObject pauseButton;
	public ScoreTracker scoreTracker;
	public MainMenu mainMenu;
	// Managers
	public GameManager gameManager;
	public LevelManager levelManager;
	private AudioManager audioManager;
	// Level Management
	public float currentLevel;
	public string currentLevelName;
	public string nextLevelName;
	// Ads
	public UnityAds unityAds;

	void Start(){
		Time.timeScale = 1;
		HUD = GameObject.Find("HUD");
		InitPlayerCanvas();
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			musicMuted = HUD.transform.Find("OptionsScreen/MuteMusicButton/MusicMuted").gameObject;
			soundMuted = HUD.transform.Find("OptionsScreen/MuteSoundButton/SoundMuted").gameObject;
			playerMovement = playerLevel.transform.gameObject.GetComponent<MovePlayer>();
			playerMovement.SetSpeed(speed);
			playerMainMovement = playerMain.transform.gameObject.GetComponent<MovePlayer>();
			playerMainMovement.SetSpeed(speed);
			playerFacing = playerLevel.transform.localScale;
			playerMainFacing = playerMain.transform.localScale;
			mainMenu = HUD.transform.GetChild(0).GetComponent<MainMenu>();
			mainMenu.GetUpgradeRanks();
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
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			// Controls player movement on the World / Level Select Screen.
			// Lets the player move to one location at a time.
			if (worldSelectScreen.activeSelf){
				if (playerDirection == "right"){
					if (playerLevel.transform.position.x < moveLocation.x && isPlayerMoving && !playerMovement.isObjectMoving()){
						playerMovement.SetDistanceX(moveDistance);
						playerMovement.Move();
						isPlayerMoving = false;
						isPlayerLevelReady = true;
					}
					else if (!playerMovement.isObjectMoving() && playerMovement.isDoneMoving() && isPlayerLevelReady){
						ShowLevelSelect(worldLevelNumber);
						playerLevel.animator.SetBool("isRunning", false);
						isPlayerLevelReady = false;
					}
				}
				else if (playerDirection == "left"){
					if (playerLevel.transform.position.x > moveLocation.x && isPlayerMoving && !playerMovement.isObjectMoving()){
						playerMovement.SetDistanceX(moveDistance);
						playerMovement.Move();
						isPlayerMoving = false;
						isPlayerLevelReady = true;
					}
					else if (!playerMovement.isObjectMoving() && playerMovement.isDoneMoving() && isPlayerLevelReady){
						ShowLevelSelect(worldLevelNumber);
						playerLevel.animator.SetBool("isRunning", false);
						isPlayerLevelReady = false;
					}
				}
				else if (playerDirection == "none"){
					isPlayerMoving = false;
					ShowLevelSelect(worldLevelNumber);
					playerLevel.animator.SetBool("isRunning", false);
				}
			}

			// Controls player movement on the Main Menu.
			// Lets the player move one location at a time.
			else if (mainMenuScreen.activeSelf){
				if (playerMainDirection == "right"){
					if (playerMain.transform.position.x < moveLocation.x && isPlayerMainMoving && !playerMainMovement.isObjectMoving()){
						playerMainMovement.SetDistanceX(moveDistance);
						playerMainMovement.Move();
						isPlayerMainMoving = false;
						isPlayerMainReady = true;
					}
					else if (!playerMainMovement.isObjectMoving() && playerMainMovement.isDoneMoving() && isPlayerMainReady){
						ShowMainMenuObject(houseObjectNumber);
						playerMain.animator.SetBool("isRunning", false);
						isPlayerMainReady = false;
					}
				}
				else if (playerMainDirection == "left"){
					if (playerMain.transform.position.x > moveLocation.x && isPlayerMainMoving && !playerMainMovement.isObjectMoving()){
						playerMainMovement.SetDistanceX(moveDistance);
						playerMainMovement.Move();
						isPlayerMainMoving = false;
						isPlayerMainReady = true;
					}
					else if (!playerMainMovement.isObjectMoving() && playerMainMovement.isDoneMoving() && isPlayerMainReady){
						ShowMainMenuObject(houseObjectNumber);
						playerMain.animator.SetBool("isRunning", false);
						isPlayerMainReady = false;
						playerMainDirection = "none";
					}
				}
				else if (playerMainDirection == "none" && !isPlayerMainReady){
					isPlayerMainMoving = false;
					ShowMainMenuObject(houseObjectNumber);
					playerMain.animator.SetBool("isRunning", false);
					isPlayerMainReady = true;
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

	// Sets the location for the player to move to on the WorldSelectScreen.
	public void SetPlayerMoveLocation(int worldNumber){
		int roundedValue = RoundDown(worldNumber);
		int firstDigit = roundedValue/10;
		int secondDigit = worldNumber - roundedValue;
		// Move location is the building you selected.
		moveLocation = GameObject.Find("HUD/WorldSelectScreen/WorldsScrollView/Viewport/WorldsSet").transform.GetChild((firstDigit * 3) - (4 - secondDigit)).transform.position;
		moveDistance = (moveLocation.x - playerLevel.transform.position.x);
		worldLevelNumber = worldNumber;
		isPlayerMoving = true;
		// Makes player run to the right.
		if (playerLevel.transform.position.x < moveLocation.x){
			playerDirection = "right";
			playerLevel.animator.SetBool("isRunning", true);
			if (playerFacing.x < 0){
				playerFacing.x *= -1;
				playerLevel.transform.localScale = playerFacing;
			}
		}
		// Makes player run to the left.
		else if (playerLevel.transform.position.x > moveLocation.x){
			playerDirection = "left";
			playerLevel.animator.SetBool("isRunning", true);
			if (playerFacing.x > 0){
				playerFacing.x *= -1;
				playerLevel.transform.localScale = playerFacing;
			}
		}
		else {
			playerDirection = "none";
			playerLevel.animator.SetBool("isRunning", false);
		}
	}

	// Sets the location for the player to move to on the Main Menu.
	public void SetPlayerMainMoveLocation(int objectNumber){
		moveLocation = GameObject.Find("HUD/MainMenuScreen").transform.GetChild(objectNumber).transform.position;
		moveDistance = (moveLocation.x - playerMain.transform.position.x);
		houseObjectNumber = objectNumber;
		isPlayerMainMoving = true;
		// Makes player run to the right.
		if (playerMain.transform.position.x < moveLocation.x){
			playerMainDirection = "right";
			playerMain.animator.SetBool("isRunning", true);
			if (playerMainFacing.x < 0){
				playerMainFacing.x *= -1;
				playerMain.transform.localScale = playerMainFacing;
			}
		}
		// Makes player run to the left.
		else if (playerMain.transform.position.x > moveLocation.x){
			playerMainDirection = "left";
			playerMain.animator.SetBool("isRunning", true);
			if (playerMainFacing.x > 0){
				playerMainFacing.x *= -1;
				playerMain.transform.localScale = playerMainFacing;
			}
		}
		else {
			playerMainDirection = "none";
			playerMain.animator.SetBool("isRunning", false);
			isPlayerMainReady = false;
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

	public void ShowMainMenuObject(int objectNumber){
		switch(objectNumber) {
			// Background is in slot 0 so don't use this.
			case 0:
				break;

			// Exit Door - World Select / Level Select
            case 1:
				playerMain.transform.position = startLocation.transform.position;
                mainMenu.ShowWorldSelect();
                break;
			
			// Cat Bed - Restore life/lives with cooldown.
            case 2:
			// Open window that asks if you want to nap or not
			// Also displays upgrade.
                mainMenu.catBed.ShowObjectWindow();
                break;

			// Fridge - Restore health with cooldown.
			case 3:
			// Open window that asks if you want to eat or not
			// Also displays upgrade.
				mainMenu.fridge.ShowObjectWindow();
				break;

			// Wardrobe - Player can change characters, equip hats and make purchases.
			case 4:
			// Open window that displays characters, equipment and other purchasable goods.
				mainMenu.wardrobe.ShowObjectWindow();
				break;

            default:
                
                break;
        }
	}

	int RoundDown(int toRound)
	{
		return toRound - toRound % 10;
	}

	public void ShowDevOptions(){
		DisableAllScreens();
		devOptionsScreen.SetActive(true);
	}

	public void ShowControls(){
		DisableAllScreens();
		controlsScreen.SetActive(true);
	}

	public void BackButton(){
		DisableAllScreens();
		mainMenuScreen.SetActive(true);
		mainMenu.CloseWindows();
	}

	// Use this before activating any screen.
	public void DisableAllScreens(){
		levelSelectScreen.SetActive(false);
		worldSelectScreen.SetActive(false);
		controlsScreen.SetActive(false);
		optionsMenuScreen.SetActive(false);
		creditsScreen.SetActive(false);
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
	}

	void HidePages(){
		for (int i = 0; i < levelManager.levelPages.Length; i++){
			levelManager.levelPages[i].SetActive(false);
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
}