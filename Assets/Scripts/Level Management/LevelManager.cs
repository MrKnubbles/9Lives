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
	private int playerLevel = 1;
	public GameObject HUD;
	public int levelsPerPage = 15;
	public GameObject[] unlockedPages = new GameObject[2];
	public GameObject[] lockedPages = new GameObject[2];
	public GameObject[] unlockedLevels;
	public GameObject[] lockedLevels;
	

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
			// Sets the game objects needed to track and set locked levels, unlocked levels and level stars.
			unlockedLevels = new GameObject[levelsPerPage * unlockedPages.Length];
			lockedLevels = new GameObject[levelsPerPage * lockedPages.Length];
			HUD = GameObject.Find("HUD");
			lockedLevelScript = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<LockedLevels>();
			unlockedPages[0] = HUD.transform.Find("LevelSelectScreen/Page1/Levels").gameObject;
			unlockedPages[1] = HUD.transform.Find("LevelSelectScreen/Page2/Levels").gameObject;
			lockedPages[0] = HUD.transform.Find("LevelSelectScreen/Page1/LockedLevels").gameObject;
			lockedPages[1] = HUD.transform.Find("LevelSelectScreen/Page2/LockedLevels").gameObject;
			GameObject.Find("LevelSelectScreen").gameObject.SetActive(false);

			playerLevel = PlayerPrefs.GetInt("PlayerLevel");
			starManager = HUD.transform.Find("LevelSelectScreen").gameObject.GetComponent<StarManager>();
			maxLevels = PlayerPrefs.GetInt("PlayerLevel");
			lockedLevelScript.FindLockedLevels();
			lockedLevelScript.CheckUnlockedLevels();
			starManager.SetStarsForCompletedLevels();

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