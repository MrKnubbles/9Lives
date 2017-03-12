using UnityEngine;
using System.Collections;

public class DeveloperOptions : MonoBehaviour {

	// Erases all data saved to PlayerPrefs.
	public void ClearAllData(){
		PlayerPrefs.DeleteAll();
	}

	// Locks all levels and sets their scores to 0.
	public void LockAllLevels(){
		for (int i = 1; i <= 3; i++){
			PlayerPrefs.SetInt("World" + i + "PlayerLevel", 0);
			print("World" + i + "PlayerLevel == " + PlayerPrefs.GetInt("World" + i + "PlayerLevel"));
			for (int j = 1; j < 100; j++){
				PlayerPrefs.SetInt("Level" + j + "Score", 0);
			}
		}
		print("All scores cleared.");
	}

	public void UnlockAllLevels(){
		for (int i = 1; i <= 3; i++){
			PlayerPrefs.SetInt("World" + i + "PlayerLevel", 45);
			print("World" + i + "PlayerLevel == " + PlayerPrefs.GetInt("World" + i + "PlayerLevel"));
		}
	}

	public void DisplayAllScores(){
		for (int i = 1; i < PlayerPrefs.GetInt("World1PlayerLevel"); i++){
			print("Level " + i + " " + PlayerPrefs.GetInt("Level" + i + "Score"));
		}
	}

	public void AddGold(){
		PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 500);
		print("Coins = " + PlayerPrefs.GetInt("Coins"));
	}

	public void AddGems(){
		PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 5);
		print("Gems = " + PlayerPrefs.GetInt("Gems"));
	}

	public void RemoveAds(){
		PlayerPrefs.SetInt("RemoveAds", 1);
		print("RemoveAds = " + PlayerPrefs.GetInt("RemoveAds") + (". 1 means true"));
	}
}