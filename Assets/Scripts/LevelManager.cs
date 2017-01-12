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
	
	void LateUpdate(){
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

	public void UnlockLevel(string levelName){
		if (PlayerPrefs.GetInt("Level" + levelName + "Score") == 0 || PlayerPrefs.GetInt("Level" + levelName + "Score") < scoreTracker.GetScore()){
			PlayerPrefs.SetInt("Level" + levelName + "Score", scoreTracker.GetScore());
		}
		switch ("Level" + levelName){
			case "Level1":
				if (playerLevel <= 1){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level2":
				if (playerLevel <= 2){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level3":
				if (playerLevel <= 3){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level4":
				if (playerLevel <= 4){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level5":
				if (playerLevel <= 5){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level6":
				if (playerLevel <= 6){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level7":
				if (playerLevel <= 7){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level8":
				if (playerLevel <= 8){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level9":
				if (playerLevel <= 9){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level10":
				if (playerLevel <= 10){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level11":
				if (playerLevel <= 11){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level12":
				if (playerLevel <= 12){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level13":
				if (playerLevel <= 13){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level14":
				if (playerLevel <= 14){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level15":
				if (playerLevel <= 15){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level16":
				if (playerLevel <= 16){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level17":
				if (playerLevel <= 17){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level18":
				if (playerLevel <= 18){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				playOnceMain = false;
				playOnceLevel = false;
				break;
		}
	}
}
