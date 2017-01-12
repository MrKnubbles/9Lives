using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {
	public LevelManager levelManager;
	private int savedScore;

	void Start(){
		levelManager = LevelManager.Instance;
	}

	public void SetStarsForCompletedLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.unlockedPages.Length; i++){
			savedScore = PlayerPrefs.GetInt("Level"+(i+1)+"Score");
			// Sets the stars for levels 1-15 on LevelSelectScreen.
			if (i < levelManager.levelsPerPage){
				levelManager.unlockedLevels[i] = levelManager.unlockedPages[0].transform.GetChild(i).gameObject;
				if (savedScore > 0){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
				}
				if (savedScore >= 350){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
				}
				if (savedScore >= 650){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
				}
			}
			// Sets the stars for levels 16-30 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.unlockedLevels[i] = levelManager.unlockedPages[1].transform.GetChild(i-levelManager.levelsPerPage).gameObject;
				if (savedScore > 0){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
				}
				if (savedScore >= 350){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
				}
				if (savedScore >= 650){
					levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
				}
			}
			// Sets the stars for levels 31-45 on LevelSelectScreen.
			// if (i >= levelManager.levelsPerPage * 2 && i < levelManager.levelsPerPage * 3){
			// 	levelManager.unlockedLevels[i] = levelManager.unlockedPages[1].transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
			// 	if (savedScore > 0){
			// 		levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
			// 	}
			// 	if (savedScore >= 350){
			// 		levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
			// 	}
			// 	if (savedScore >= 650){
			// 		levelManager.unlockedLevels[i].transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
			// 	}
			// }
		}
	}
}