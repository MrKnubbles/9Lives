using UnityEngine;
using System.Collections;

public class ClearData : MonoBehaviour {

	public void ClearAllData(){
		PlayerPrefs.SetInt("PlayerLevel", 0);
		print("PlayerLevel == " + PlayerPrefs.GetInt("PlayerLevel"));
		for (int i = 1; i < 20; i++){
			PlayerPrefs.SetInt("Level" + i + "Score", 0);
		}
		print("All scores cleared.");
	}

	public void UnlockAllLevels(){
		PlayerPrefs.SetInt("PlayerLevel", 100);
		print("PlayerLevel == " + PlayerPrefs.GetInt("PlayerLevel"));
	}

	public void DisplayAllScores(){
		for (int i = 1; i < 20; i++){
			print("Level " + i + " " + PlayerPrefs.GetInt("Level" + i + "Score"));
		}
	}
}