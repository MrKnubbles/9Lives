using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {
	private LevelManager levelManager;
	private int savedScore;
	public int starCounter;

	void Start(){
		levelManager = LevelManager.Instance;
	}

	public void SetStarsForCompletedLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.levelPages.Length; i++){
			savedScore = PlayerPrefs.GetInt("Level"+(i+1)+"Score");
			// Sets the stars for levels 1-15 on LevelSelectScreen.
			if (i < levelManager.levelsPerPage){
				levelManager.levelButtons[i] = levelManager.levelPages[0].transform.GetChild(0).transform.GetChild(i).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starCounter++;
				}
			}
			// Sets the stars for levels 16-30 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.levelButtons[i] = levelManager.levelPages[1].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starCounter++;
				}
			}
			// Sets the stars for levels 31-45 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage * 2 && i < levelManager.levelsPerPage * 3){
				levelManager.levelButtons[i] = levelManager.levelPages[2].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starCounter++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starCounter++;
				}
			}
		}
	}
}