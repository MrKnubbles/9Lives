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
			// Sets what the locked level game objects are for Levels 1-15;
			if (i < levelManager.levelsPerPage){
				levelManager.levelButtons[i] = levelManager.levelPages[0].transform.GetChild(i).gameObject;
			}
			// Sets what the locked level game objects are for Levels 16-30;
			else if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.levelButtons[i] = levelManager.levelPages[1].transform.GetChild(i-levelManager.levelsPerPage).gameObject;
			}
			// Sets what the locked level game objects are for Levels 31-45;
			else if (i >= (levelManager.levelsPerPage * 2) && i < levelManager.levelsPerPage * 3){
				levelManager.levelButtons[i] = levelManager.levelPages[2].transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
			}
		}
	}

	public void CheckUnlockedLevels(){
		// Checks the scores for each level then unlocks whichever levels the player has already unlocked.
		for (int i = 0; i < levelManager.maxLevels; i++){
			levelManager.levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
			levelManager.levelButtons[i].transform.GetChild(2).gameObject.SetActive(true);
		}
	}
}