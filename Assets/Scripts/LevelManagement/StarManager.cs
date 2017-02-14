using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarManager : MonoBehaviour {
	private LevelManager levelManager;
	private int savedScore;
	public int starCounter;
	public int starsWorld1;
	public int starsWorld2;
	public int starsWorld3;
	public Text totalStarsText;
	public Text starsTextWorld1;
	public Text starsTextWorld2;
	public Text starsTextWorld3;

	void Start(){
		levelManager = LevelManager.Instance;
		starsTextWorld1 = GameObject.Find("WorldSelectScreen").transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();
		starsTextWorld2 = GameObject.Find("WorldSelectScreen").transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
		starsTextWorld3 = GameObject.Find("WorldSelectScreen").transform.GetChild(3).GetChild(0).GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>();
		totalStarsText = GameObject.Find("WorldSelectScreen").transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>();
	}

	public void SetWorldStars(){
		int tempTotalMax = 3 * PlayerPrefs.GetInt("PlayerLevel");
		starCounter = starsWorld1 + starsWorld2 + starsWorld3;
		starsTextWorld1.text = "" + starsWorld1 + "/135";
		starsTextWorld2.text = "" + starsWorld2 + "/135";
		starsTextWorld3.text = "" + starsWorld2 + "/135";
		totalStarsText.text = "" + starCounter + "/" + tempTotalMax;
	}

	public void SetStarsForCompletedLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.levelPages.Length; i++){
			savedScore = PlayerPrefs.GetInt("Level"+(i+1)+"Score");
			// Sets the stars for levels 1-15 on LevelSelectScreen.
			if (i < levelManager.levelsPerPage){
				levelManager.levelButtons[i] = levelManager.levelPages[0].transform.GetChild(0).transform.GetChild(i).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1++;
				}
			}
			// Sets the stars for levels 16-30 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.levelButtons[i] = levelManager.levelPages[1].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1++;
				}
			}
			// Sets the stars for levels 31-45 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage * 2 && i < levelManager.levelsPerPage * 3){
				levelManager.levelButtons[i] = levelManager.levelPages[2].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1++;
				}
			}
		}
	}
}