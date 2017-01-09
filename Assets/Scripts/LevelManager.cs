using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	private static LevelManager m_instance = null;
	public static LevelManager Instance { get { return m_instance; } }
	//public GameObject levelManager;
	static bool GameBegin = false;
	// To control which levels the player has unlocked.
	//static GameManager gameManager;
	public LockedLevels lockedLevels;
	public StarManager starManager;
	public ScoreTracker scoreTracker;
	//public GameObject lockedLev;
	//private GameObject HUD;
	//public GameObject levelController;
	private bool playOnceMain = false;
	public bool playOnceLevel = false;
	private int playerLevel = 1;

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
			//HUD = GameObject.Find("HUD");
			//lockedLev = HUD.transform.Find("LevelSelectScreen").gameObject;
			lockedLevels = GameObject.Find("HUD").transform.Find("LevelSelectScreen").gameObject.GetComponent<LockedLevels>();
			playerLevel = PlayerPrefs.GetInt("PlayerLevel");
			starManager = GameObject.Find("HUD").transform.Find("LevelSelectScreen").gameObject.GetComponent<StarManager>();

			// TODO: Make a for loop to set all stars and unlock each level.
			// for (int i = 1; i < 17; i++){
			// 	if (PlayerPrefs.GetInt("Level" + i + "Score") > 0){
			// 		starManager.level1.transform.Find("Star1").gameObject.SetActive(true);
			// 		starManager.transform.Find("Level" + i);
			// 	}
			// 	if (PlayerPrefs.GetInt("Level" + i + "Score") >= 350){
			// 		starManager.level1.transform.Find("Star2").gameObject.SetActive(true);
			// 	}
			// 	if (PlayerPrefs.GetInt("Level" + i + "Score") >= 650){
			// 		starManager.level1.transform.Find("Star3").gameObject.SetActive(true);
			// 	}
			// }
			CheckUnlockedLevels();

			playOnceMain = true;
		}
		if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main") && scoreTracker == null){
			scoreTracker = GameObject.Find("HUD").GetComponent<ScoreTracker>();
		}
	}

	public void CheckUnlockedLevels(){
		if (PlayerPrefs.GetInt("PlayerLevel") > 1){
			lockedLevels.locked2.SetActive(false);
			if (PlayerPrefs.GetInt("Level1Score") > 0){
				starManager.level1.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level1Score") >= 350){
				starManager.level1.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level1Score") >= 650){
				starManager.level1.transform.Find("Star3").gameObject.SetActive(true);
			}
		}

		if (PlayerPrefs.GetInt("PlayerLevel") > 2){
			lockedLevels.locked3.SetActive(false);
			if (PlayerPrefs.GetInt("Level2Score") > 0){
				starManager.level2.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level2Score") >= 350){
				starManager.level2.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level2Score") >= 650){
				starManager.level2.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 3){
			lockedLevels.locked4.SetActive(false);
			if (PlayerPrefs.GetInt("Level3Score") > 0){
				starManager.level3.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level3Score") >= 350){
				starManager.level3.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level3Score") >= 650){
				starManager.level3.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 4){
			lockedLevels.locked5.SetActive(false);
			if (PlayerPrefs.GetInt("Level4Score") > 0){
				starManager.level4.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level4Score") >= 350){
				starManager.level4.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level4Score") >= 650){
				starManager.level4.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 5){
			lockedLevels.locked6.SetActive(false);
			if (PlayerPrefs.GetInt("Level5Score") > 0){
				starManager.level5.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level5Score") >= 350){
				starManager.level5.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level5Score") >= 650){
				starManager.level5.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 6){
			lockedLevels.locked7.SetActive(false);
			if (PlayerPrefs.GetInt("Level6Score") > 0){
				starManager.level6.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level6Score") >= 350){
				starManager.level6.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level6Score") >= 650){
				starManager.level6.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 7){
			lockedLevels.locked8.SetActive(false);
			if (PlayerPrefs.GetInt("Level7Score") > 0){
				starManager.level7.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level7Score") >= 350){
				starManager.level7.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level7Score") >= 650){
				starManager.level7.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 8){
			lockedLevels.locked9.SetActive(false);
			if (PlayerPrefs.GetInt("Level8Score") > 0){
				starManager.level8.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level8Score") >= 350){
				starManager.level8.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level8Score") >= 650){
				starManager.level8.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 9){
			lockedLevels.locked10.SetActive(false);
			if (PlayerPrefs.GetInt("Level9Score") > 0){
				starManager.level9.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level9Score") >= 350){
				starManager.level9.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level9Score") >= 650){
				starManager.level9.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 10){
			lockedLevels.locked11.SetActive(false);
			if (PlayerPrefs.GetInt("Level10Score") > 0){
				starManager.level10.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level10Score") >= 350){
				starManager.level10.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level10Score") >= 650){
				starManager.level10.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 11){
			lockedLevels.locked12.SetActive(false);
			if (PlayerPrefs.GetInt("Level11Score") > 0){
				starManager.level11.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level11Score") >= 350){
				starManager.level11.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level11Score") >= 650){
				starManager.level11.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 12){
			lockedLevels.locked13.SetActive(false);
			if (PlayerPrefs.GetInt("Level12Score") > 0){
				starManager.level12.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level12Score") >= 350){
				starManager.level12.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level12Score") >= 650){
				starManager.level12.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 13){
			lockedLevels.locked14.SetActive(false);
			if (PlayerPrefs.GetInt("Level13Score") > 0){
				starManager.level13.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level13Score") >= 350){
				starManager.level13.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level13Score") >= 650){
				starManager.level13.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 14){
			lockedLevels.locked15.SetActive(false);
			if (PlayerPrefs.GetInt("Level14Score") > 0){
				starManager.level14.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level14Score") >= 350){
				starManager.level14.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level14Score") >= 650){
				starManager.level14.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 15){
			lockedLevels.locked16.SetActive(false);
			if (PlayerPrefs.GetInt("Level15Score") > 0){
				starManager.level15.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level15Score") >= 350){
				starManager.level15.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level15Score") >= 650){
				starManager.level15.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 16){
			lockedLevels.locked17.SetActive(false);
			if (PlayerPrefs.GetInt("Level16Score") > 0){
				starManager.level16.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level16Score") >= 350){
				starManager.level16.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level16Score") >= 650){
				starManager.level16.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 17){
			lockedLevels.locked18.SetActive(false);
			if (PlayerPrefs.GetInt("Level17Score") > 0){
				starManager.level17.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level17Score") >= 350){
				starManager.level17.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level17Score") >= 650){
				starManager.level17.transform.Find("Star3").gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("PlayerLevel") > 18){
			//lockedLevels.locked19.SetActive(false);
			if (PlayerPrefs.GetInt("Level18Score") > 0){
				starManager.level18.transform.Find("Star1").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level18Score") >= 350){
				starManager.level18.transform.Find("Star2").gameObject.SetActive(true);
			}
			if (PlayerPrefs.GetInt("Level18Score") >= 650){
				starManager.level18.transform.Find("Star3").gameObject.SetActive(true);
			}
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
				// if (PlayerPrefs.GetInt("Level1Score") == 0 || PlayerPrefs.GetInt("Level1Score") < scoreTracker.GetScore()){
				// 	PlayerPrefs.SetInt("Level1Score", scoreTracker.GetScore());
				// }
				playOnceMain = false;
				playOnceLevel = false;
				break;
			case "Level2":
				if (playerLevel <= 2){
					playerLevel++;
					PlayerPrefs.SetInt("PlayerLevel", playerLevel);
				}
				// if (PlayerPrefs.GetInt("Level2Score") == 0 || PlayerPrefs.GetInt("Level2Score") < scoreTracker.GetScore()){
				// 	PlayerPrefs.SetInt("Level1Score", scoreTracker.GetScore());
				// }
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
