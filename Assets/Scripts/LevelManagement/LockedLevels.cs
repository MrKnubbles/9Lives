using UnityEngine;
using System.Collections;

public class LockedLevels : MonoBehaviour {
	private LevelManager levelManager;
	private GameObject HUD;

	void Start(){
		levelManager = LevelManager.Instance;
	}

	public void FindLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.levelPages.Length; i++){
			// Sets what the locked level game objects are for World 1 Levels 1-15;
			if (i < levelManager.levelsPerPage){
				levelManager.levelButtons[i] = levelManager.levelPages[0].transform.GetChild(0).transform.GetChild(i).gameObject;
			}
			// Sets what the locked level game objects are for World 1 Levels 16-30;
			else if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.levelButtons[i] = levelManager.levelPages[1].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage).gameObject;
			}
			// Sets what the locked level game objects are for World 1 Levels 31-45;
			else if (i >= (levelManager.levelsPerPage * 2) && i < levelManager.levelsPerPage * 3){
				levelManager.levelButtons[i] = levelManager.levelPages[2].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
			}
			// Sets what the locked level game objects are for World 2 Levels 1-15;
			else if (i >= (levelManager.levelsPerPage * 3) && i < levelManager.levelsPerPage * 4){
				levelManager.levelButtons[i] = levelManager.levelPages[3].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*3).gameObject;
			}
			// Sets what the locked level game objects are for World 2 Levels 16-30;
			else if (i >= (levelManager.levelsPerPage * 4) && i < levelManager.levelsPerPage * 5){
				levelManager.levelButtons[i] = levelManager.levelPages[4].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*4).gameObject;
			}
			// Sets what the locked level game objects are for World 2 Levels 31-45;
			else if (i >= (levelManager.levelsPerPage * 5) && i < levelManager.levelsPerPage * 6){
				levelManager.levelButtons[i] = levelManager.levelPages[5].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*5).gameObject;
			}
		}
	}

	public void CheckUnlockedLevels(){
		// Checks the scores for each level then unlocks whichever levels the player has already unlocked.
		if (PlayerPrefs.GetInt("World1PlayerLevel") >= 1){
			for (int i = 0; i < PlayerPrefs.GetInt("World1PlayerLevel"); i++){
				levelManager.levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
				levelManager.levelButtons[i].transform.GetChild(2).gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("World2PlayerLevel") >= 1){
			for (int i = 0; i < levelManager.world2PlayerLevel; i++){
				levelManager.levelButtons[i+45].transform.GetChild(1).gameObject.SetActive(false);
				levelManager.levelButtons[i+45].transform.GetChild(2).gameObject.SetActive(true);
			}
		}
		if (PlayerPrefs.GetInt("World3PlayerLevel") >= 1){
			for (int i = 0; i < levelManager.world3PlayerLevel; i++){
				levelManager.levelButtons[i+90].transform.GetChild(1).gameObject.SetActive(false);
				levelManager.levelButtons[i+90].transform.GetChild(2).gameObject.SetActive(true);
			}
		}
	}
}