using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class CatBed : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;

	// Cat Bed
	public GameObject catBedWindow;
	public Text catBedDescription;
	private int catBedLives = 1;
	private int catBedCooldown = 36;
	private int catBedExp;
	private int catBedMaxExp = 3;

	// Player Prefs for upgrades.
	public int catBedUpgradeRank = 0;
	public int catBedUpgradeExp = 0;
	public int catBedUpgradePower = 0;
	public int catBedUpgradeSpeed = 0;
	public float catBedUpgradeCost;
	public float catBedUpgradeTime;

	void Start(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
	}

	// Opens the window for the Cat Bed.
	public void ShowCatBedWindow(){
		// TODO: Slide window on to screen over 1 second.
		if (catBedUpgradePower > 0){
			catBedDescription.text = "Take a cat nap, restoring " + (catBedLives + catBedUpgradePower) + "lives./n/n" + (catBedCooldown - catBedUpgradeSpeed) + "hour cooldown.";
		}
		catBedWindow.SetActive(true);
	}

	// Uses the Cat Bed, which restores lives with a cooldown.
	public void UseCatBed(){
		// TODO: Check if cooldown has expired.
		if (playerStats.GetLives() < 9){
			playerStats.AddLives(1 + catBedUpgradePower);
		}
		else{
			// TODO: Prompt the player that their lives are full.
		}
	}

	// Upgrades the Cat Bed.
	public void UpgradeCatBed(){
		// TODO: Check if the player has enough money to afford the upgrade.
		// TODO: Set the object to begin upgrading based on the upgrade time.
		if (catBedExp < catBedMaxExp){
			// TODO: Deduct cost from player money here.
			catBedExp += 1;
			catBedUpgradeSpeed += 1;
			PlayerPrefs.SetInt("cdUpSpd", catBedUpgradeSpeed);
			catBedUpgradeTime += 2f;
			if (catBedExp <= 2){
				catBedUpgradeCost *= 1.2f;
			}
			else {
				catBedUpgradeCost *= 1.5f;
			}
		}
		else if (catBedExp >= catBedMaxExp){
			catBedExp = 0;
			catBedUpgradeRank += 1;
			catBedUpgradePower += 1;
			catBedUpgradeSpeed += 1;
			PlayerPrefs.SetInt("cdUpRank", catBedUpgradeRank);
			PlayerPrefs.SetInt("cdUpPow", catBedUpgradePower);
			PlayerPrefs.SetInt("cdUpSpd", catBedUpgradeSpeed);
			catBedUpgradeCost *= 1.2f;
			catBedUpgradeTime += 2f;
		}
	}
}
