using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	private static LevelManager m_instance = null;
	public static LevelManager Instance { get { return m_instance; } }
	static bool GameBegin = false;
	public ScoreTracker scoreTracker;
	public bool playOnceMain = false;
	public bool playOnceLevel = false;
	public int replayCounter = 0;
	public int levelCounter = 0;
	// To control which levels the player has unlocked.
	public int world1MaxLevels;
	public int world2MaxLevels;
	public int world3MaxLevels;
	public LockedLevels lockedLevelScript;
	public StarManager starManager;
	public GameManager gameManager;
	// Used to track what levels the player has unlocked in each world.
	//private int playerLevel = 0;
	public int world1PlayerLevel = 0;
	public int world2PlayerLevel = 0;
	public int world3PlayerLevel = 0;
	public GameObject HUD;
	public int levelsPerPage = 15;
	public GameObject pageTracker;
	public GameObject[] levelPages = new GameObject[9];
	public GameObject[] levelButtons;
	public GameObject[] worlds = new GameObject[3];
	public GameObject[] nextWorldButtons = new GameObject[2];

	void Awake(){
		if (m_instance != null && m_instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			m_instance = this;
			m_instance.name = "LevelManager";

			if (!GameBegin){
				DontDestroyOnLoad(gameObject);
				GameBegin = true;
			}
		}
	}

	void Start(){
		if (m_instance != null && m_instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			m_instance = this;
			m_instance.name = "LevelManager";

			if (!GameBegin){
				DontDestroyOnLoad(gameObject);
				GameBegin = true;
			}
		}
		gameManager = GameManager.Instance;
	}
	
	void Update(){
		if (!playOnceMain && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			// Sets the game objects needed to track and set each world page, locked levels, unlocked levels and level stars.
			levelButtons = new GameObject[levelsPerPage * levelPages.Length];
			HUD = GameObject.Find("HUD");
			worlds[0] = HUD.transform.Find("LevelSelectScreen/World1").gameObject;
			worlds[1] = HUD.transform.Find("LevelSelectScreen/World2").gameObject;
			worlds[2] = HUD.transform.Find("LevelSelectScreen/World3").gameObject;
			//pageTracker = HUD.transform.Find("LevelSelectScreen/PageTracker").gameObject;
			lockedLevelScript = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<LockedLevels>();
			levelPages[0] = HUD.transform.Find("LevelSelectScreen/World1/Page1").gameObject;
			levelPages[1] = HUD.transform.Find("LevelSelectScreen/World1/Page2").gameObject;
			levelPages[2] = HUD.transform.Find("LevelSelectScreen/World1/Page3").gameObject;
			levelPages[3] = HUD.transform.Find("LevelSelectScreen/World2/Page1").gameObject;
			levelPages[4] = HUD.transform.Find("LevelSelectScreen/World2/Page2").gameObject;
			levelPages[5] = HUD.transform.Find("LevelSelectScreen/World2/Page3").gameObject;
			levelPages[6] = HUD.transform.Find("LevelSelectScreen/World3/Page1").gameObject;
			levelPages[7] = HUD.transform.Find("LevelSelectScreen/World3/Page2").gameObject;
			levelPages[8] = HUD.transform.Find("LevelSelectScreen/World3/Page3").gameObject;
			GameObject.Find("LevelSelectScreen").gameObject.SetActive(false);
			GameObject.Find("WorldSelectScreen").gameObject.SetActive(false);

			// Sets the player to level 1 the first time they play.
			if (PlayerPrefs.GetInt("World1PlayerLevel") <= 0){
				PlayerPrefs.SetInt("World1PlayerLevel", 1);
				PlayerPrefs.SetInt("World2PlayerLevel", 0);
				PlayerPrefs.SetInt("World3PlayerLevel", 0);
				PlayerPrefs.SetInt("MusicMuted", 1);
				PlayerPrefs.SetInt("SFXMuted", 1);
				PlayerPrefs.SetInt("StarsCollected", 0);
				PlayerPrefs.SetInt("Coins", 0);
				// Unlocks Beta skin!
				// TODO: Remove this when game goes live.
				PlayerPrefs.SetInt("Beta", 1);
				PlayerPrefs.SetString("ActiveChar", "Beta");
			}
			else{
				world1PlayerLevel = PlayerPrefs.GetInt("World1PlayerLevel");
				world2PlayerLevel = PlayerPrefs.GetInt("World2PlayerLevel");
				world3PlayerLevel = PlayerPrefs.GetInt("World3PlayerLevel");
				PlayerPrefs.GetInt("MusicMuted");
				PlayerPrefs.GetInt("SFXMuted");
				PlayerPrefs.GetInt("StarsCollected");
				PlayerPrefs.GetInt("Coins");
			}

			starManager = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<StarManager>();
			// world1MaxLevels = PlayerPrefs.GetInt("World1PlayerLevel");
			// world2MaxLevels = PlayerPrefs.GetInt("World2PlayerLevel");
			// world3MaxLevels = PlayerPrefs.GetInt("World3PlayerLevel");
			lockedLevelScript.FindLevels();
			lockedLevelScript.CheckUnlockedLevels();
			starManager.SetStarsForCompletedLevels();
			starManager.SetWorldStars();

			playOnceMain = true;
		}
		if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main") && scoreTracker == null){
			scoreTracker = GameObject.Find("HUD").GetComponent<ScoreTracker>();
			playOnceMain = false;
		}
	}

	public void UnlockLevel(int levelName){
		if (PlayerPrefs.GetInt("Level" + levelName + "Score") == 0 || PlayerPrefs.GetInt("Level" + levelName + "Score") < scoreTracker.GetScore()){
			PlayerPrefs.SetInt("Level" + levelName + "Score", scoreTracker.GetScore());
		}
		if (world1PlayerLevel <= levelName && levelName <= 45){
			world1PlayerLevel++;
			PlayerPrefs.SetInt("World1PlayerLevel", world1PlayerLevel);
		}
		else if (world2PlayerLevel <= levelName && levelName > 45 && levelName <= 90){
			world2PlayerLevel++;
			PlayerPrefs.SetInt("World2PlayerLevel", world2PlayerLevel);
		}
		else if (world3PlayerLevel <= levelName && levelName > 90 && levelName <= 135){
			world3PlayerLevel++;
			PlayerPrefs.SetInt("World3PlayerLevel", world3PlayerLevel);
		}

		playOnceMain = false;
		playOnceLevel = false;
	}
}