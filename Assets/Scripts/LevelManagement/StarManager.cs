using UnityEngine;
using UnityEngine.UI;
using System;

public class StarManager : MonoBehaviour {
	private LevelManager levelManager;
	private int savedScore;
	public int starCounter;
	private int maxLevelStars = 45;
	public int[] starsWorld1;
	public int[] starsWorld2;
	public int[] starsWorld3;
	public Text totalStarsText;
	public Text[] starsTextWorld1;
	public Text[] starsTextWorld2;
	public Text[] starsTextWorld3;
	public int starQuotaWorld2;
	public int starQuotaWorld3;
	public GameObject[] lockedWorld1;
	public GameObject[] lockedWorld2;
	public GameObject[] lockedWorld3;

	void Start(){
		levelManager = LevelManager.Instance;
		starsTextWorld1[0] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld1[1] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld1[2] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld2[0] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld2[1] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld2[2] = GameObject.Find("LevelSelectScreen").transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>();
		starsTextWorld3[0] = GameObject.Find("WorldSelectScreen").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>();
		starsTextWorld3[1] = GameObject.Find("WorldSelectScreen").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(7).GetChild(1).GetChild(0).GetComponent<Text>();
		starsTextWorld3[2] = GameObject.Find("WorldSelectScreen").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(8).GetChild(1).GetChild(0).GetComponent<Text>();
		totalStarsText = GameObject.Find("WorldSelectScreen").transform.GetChild(2).GetChild(0).GetComponent<Text>();
		lockedWorld1[0] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(2).gameObject;
		lockedWorld1[1] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(2).gameObject;
		lockedWorld2[0] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(3).GetChild(2).gameObject;
		lockedWorld2[1] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(4).GetChild(2).gameObject;
		lockedWorld2[2] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(5).GetChild(2).gameObject;
		lockedWorld3[0] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(6).GetChild(3).gameObject;
		lockedWorld3[1] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(7).GetChild(3).gameObject;
		lockedWorld3[2] = GameObject.Find("HUD").transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetChild(8).GetChild(3).gameObject;
		starQuotaWorld2 = Convert.ToInt32(lockedWorld2[0].transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
		starQuotaWorld3 = Convert.ToInt32(lockedWorld3[0].transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
	}

	public void SetWorldStars(){
		int tempTotalMax = 3 * (PlayerPrefs.GetInt("World1PlayerLevel") + PlayerPrefs.GetInt("World2PlayerLevel") + PlayerPrefs.GetInt("World3PlayerLevel"));
		for (int i = 0; i < 3; i++){
			starCounter += starsWorld1[i] + starsWorld2[i] + starsWorld3[i];
		}
		starsTextWorld1[0].text = "" + starsWorld1[0] + "/" + maxLevelStars;
		starsTextWorld1[1].text = "" + starsWorld1[1] + "/" + maxLevelStars;
		starsTextWorld1[2].text = "" + starsWorld1[2] + "/" + maxLevelStars;
		starsTextWorld2[0].text = "" + starsWorld2[0] + "/" + maxLevelStars;
		starsTextWorld2[1].text = "" + starsWorld2[1] + "/" + maxLevelStars;
		starsTextWorld2[2].text = "" + starsWorld2[2] + "/" + maxLevelStars;
		starsTextWorld3[0].text = "" + starsWorld3[0] + "/" + maxLevelStars;
		starsTextWorld3[1].text = "" + starsWorld2[1] + "/" + maxLevelStars;
		starsTextWorld3[2].text = "" + starsWorld2[2] + "/" + maxLevelStars;
		totalStarsText.text = "" + starCounter + "/" + tempTotalMax;
		// If the player has beaten 15 World 1 levels, unlock Building 2.
		if (PlayerPrefs.GetInt("World1PlayerLevel") > 15){
			lockedWorld1[0].SetActive(false);
			// If the player has beaten 30 World 1 levels, unlocked Building 3.
			if (PlayerPrefs.GetInt("World1PlayerLevel") > 30){
				lockedWorld1[1].SetActive(false);
			}
		}
		// If the player has enough stars, unlock World 2.
		if (starCounter >= starQuotaWorld2){
			lockedWorld2[0].SetActive(false);
			// Set World 2 to "unlocked".
			if (PlayerPrefs.GetInt("World2PlayerLevel") <= 0){
				PlayerPrefs.SetInt("World2PlayerLevel", 1);
			}
			// If the player has beaten 15 World 2 levels, unlock Building 2.
			if (PlayerPrefs.GetInt("World2PlayerLevel") > 15){
				lockedWorld2[1].SetActive(false);
				// If the player has beaten 30 World 2 levels, unlock Building 3.
				if (PlayerPrefs.GetInt("World2PlayerLevel") > 30){
					lockedWorld2[2].SetActive(false);
				}
			}
			// If the player has enough stars, unlock World 3.
			if (starCounter >= starQuotaWorld3){
				lockedWorld3[0].SetActive(false);
				lockedWorld3[1].SetActive(false);
				lockedWorld3[2].SetActive(false);
				if (PlayerPrefs.GetInt("World3PlayerLevel") <= 0){
					PlayerPrefs.SetInt("World3PlayerLevel", 1);
				}
			}
		}
	}

	public void SetStarsForCompletedLevels(){
		for (int i = 0; i < levelManager.levelsPerPage * levelManager.levelPages.Length; i++){
			savedScore = PlayerPrefs.GetInt("Level"+(i+1)+"Score");
			// Sets the stars for levels 1-15 on LevelSelectScreen.
			if (i < levelManager.levelsPerPage){
				levelManager.levelButtons[i] = levelManager.levelPages[0].transform.GetChild(0).transform.GetChild(i).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1[0]++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1[0]++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1[0]++;
				}
			}
			// Sets the stars for levels 16-30 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage && i < levelManager.levelsPerPage * 2){
				levelManager.levelButtons[i] = levelManager.levelPages[1].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1[1]++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1[1]++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1[1]++;
				}
			}
			// Sets the stars for levels 31-45 on LevelSelectScreen.
			if (i >= levelManager.levelsPerPage * 2 && i < levelManager.levelsPerPage * 3){
				levelManager.levelButtons[i] = levelManager.levelPages[2].transform.GetChild(0).transform.GetChild(i-levelManager.levelsPerPage*2).gameObject;
				if (savedScore > 0){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
					starsWorld1[2]++;
				}
				if (savedScore >= 350){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
					starsWorld1[2]++;
				}
				if (savedScore >= 650){
					levelManager.levelButtons[i].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);
					starsWorld1[2]++;
				}
			}
		}
	}
}