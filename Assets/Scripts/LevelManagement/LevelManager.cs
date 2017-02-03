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
	// To control which levels the player has unlocked.
	public int maxLevels;
	public LockedLevels lockedLevelScript;
	public StarManager starManager;
	private int playerLevel = 0;
	public GameObject HUD;
	public int levelsPerPage = 15;
	public int worldNumber;
	public GameObject pageTracker;
	public GameObject[] levelPages = new GameObject[6];
	public GameObject[] levelButtons;
	public GameObject[] worlds = new GameObject[2];
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
	}
	
	void Update(){
		if (!playOnceMain && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			// Sets the game objects needed to track and set each world page, locked levels, unlocked levels and level stars.
			levelButtons = new GameObject[levelsPerPage * levelPages.Length];
			HUD = GameObject.Find("HUD");
			worlds[0] = HUD.transform.Find("LevelSelectScreen/World1").gameObject;
			worlds[1] = HUD.transform.Find("LevelSelectScreen/World2").gameObject;
			nextWorldButtons[0] = worlds[0].transform.Find("NextWorldButton").gameObject;
			nextWorldButtons[1] = worlds[1].transform.Find("NextWorldButton").gameObject;
			pageTracker = HUD.transform.Find("LevelSelectScreen/PageTracker").gameObject;
			lockedLevelScript = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<LockedLevels>();
			levelPages[0] = HUD.transform.Find("LevelSelectScreen/World1/Page1").gameObject;
			levelPages[1] = HUD.transform.Find("LevelSelectScreen/World1/Page2").gameObject;
			levelPages[2] = HUD.transform.Find("LevelSelectScreen/World1/Page3").gameObject;
			levelPages[3] = HUD.transform.Find("LevelSelectScreen/World2/Page1").gameObject;
			levelPages[4] = HUD.transform.Find("LevelSelectScreen/World2/Page2").gameObject;
			levelPages[5] = HUD.transform.Find("LevelSelectScreen/World2/Page3").gameObject;
			GameObject.Find("LevelSelectScreen").gameObject.SetActive(false);

			// Sets the player to level 1 the first time they play.
			if (PlayerPrefs.GetInt("PlayerLevel") <= 0){
				PlayerPrefs.SetInt("PlayerLevel", 1);
				PlayerPrefs.SetInt("MusicMuted", 1);
				PlayerPrefs.SetInt("SFXMuted", 1);
				PlayerPrefs.SetInt("StarsCollected", 0);
				worldNumber = 1;
			}
			else{
				playerLevel = PlayerPrefs.GetInt("PlayerLevel");
				PlayerPrefs.GetInt("MusicMuted");
				PlayerPrefs.GetInt("SFXMuted");
				PlayerPrefs.GetInt("StarsCollected");
			}

			starManager = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<StarManager>();
			maxLevels = PlayerPrefs.GetInt("PlayerLevel");
			lockedLevelScript.FindLevels();
			lockedLevelScript.CheckUnlockedLevels();
			starManager.SetStarsForCompletedLevels();

			if (starManager.starCounter >= 100 && playerLevel >= 46){
				nextWorldButtons[0].SetActive(true);
			}

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
		if (playerLevel <= levelName){
			playerLevel++;
			PlayerPrefs.SetInt("PlayerLevel", playerLevel);
		}
		playOnceMain = false;
		playOnceLevel = false;
	}
}