using UnityEngine;
using System.Collections;

public class LockedLevels : MonoBehaviour {
	public LevelManager levelManager;
	private GameObject HUD;

	void Start(){
		levelManager = LevelManager.Instance;
	}

	public void FindLockedLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.lockedPages.Length; i++){
			// Sets what the locked level game objects are for Levels 1-15;
			if (i < levelManager.levelsPerPage){
				levelManager.lockedLevels[i] = levelManager.lockedPages[0].transform.GetChild(i).gameObject;
			}
			// Sets what the locked level game objects are for Levels 16-30;
			else if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.lockedLevels[i] = levelManager.lockedPages[1].transform.GetChild(i-levelManager.levelsPerPage).gameObject;
			}
			// Sets what the locked level game objects are for Levels 31-45;
			// else if (i >= levelManager.levelsPerPage*2 && i < levelManager.levelsPerPage * 3){
			// 	levelManager.lockedLevels[i] = levelManager.lockedPages[1].transform.GetChild(i-levelManager.levelsPerPage).gameObject;
			// }
		}
	}

	public void CheckUnlockedLevels(){
		int savedScore;
		// Checks the scores for each level then unlocks whichever levels the player has already unlocked.
		for (int i = 0; i < levelManager.maxLevels; i++){
			savedScore = PlayerPrefs.GetInt("Level" + i + "Score");
			levelManager.lockedLevels[i].SetActive(false);
		}
	}
}