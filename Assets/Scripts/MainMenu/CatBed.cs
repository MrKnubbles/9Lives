using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class CatBed : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;

	// Cat Bed
	public GameObject objectWindow;
	public Text objectDescription;
	public Text cooldownText;
	public Text timerText;
	private int livesRestoreAmount = 1;
	private int objectCooldown = 36;
	private int maxRank = 3;
	private float currentUpgradeTime;
	private float lastUpgradeTime;
	private float currentCooldownTime;
	private float lastCooldownTime;
	// Player Prefs for upgrades.
	public int savedUpgradeLevel = 1;
	public int savedUpgradeRank = 0;
	public int savedUpgradePower = 0;
	public int savedUpgradeSpeed = 0;
	public int savedUpgradeCost;
	public float savedUpgradeTime; // in hours

    // Real time tracking stuff
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;
	//
	float convertToHours = 3600f;

	void Start(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
		CheckTimers();
	}

	void Update(){
		UpgradeCountdown();
	}

	public void GetPrefs(){
		savedUpgradeCost = PlayerPrefs.GetInt("cbUpCost");
		savedUpgradeLevel = PlayerPrefs.GetInt("cbLevel");
		savedUpgradePower = PlayerPrefs.GetInt("cbUpPow");
		savedUpgradeRank = PlayerPrefs.GetInt("cbUpRank");
		savedUpgradeSpeed = PlayerPrefs.GetInt("cbUpSpd");
		savedUpgradeTime = PlayerPrefs.GetFloat("cbUpTime");
	}

	public void SetPrefs(){
		PlayerPrefs.SetInt("cbUpCost", 0);
		PlayerPrefs.SetInt("cbLevel", 1);
		PlayerPrefs.SetInt("cbUpPow", 0);
		PlayerPrefs.SetInt("cbUpRank", 0);
		PlayerPrefs.SetInt("cbUpSpd", 0);
		PlayerPrefs.SetFloat("cbUpTime", 0);
	}

	// Opens the window for the Cat Bed.
	public void ShowObjectWindow(){
		// TODO: Slide window on to screen over 1 second.
		if (savedUpgradePower > 0){
			objectDescription.text = "Take a cat nap, restoring " + (livesRestoreAmount + savedUpgradePower) + " lives.\n\n" + (objectCooldown - savedUpgradeSpeed) + " hour cooldown.";
		}
		objectWindow.SetActive(true);
	}

	// Uses the Cat Bed, which restores lives with a cooldown.
	public void UseObject(){
		// TODO: Check if cooldown has expired.
		if (playerStats.GetLives() < 9){
			playerStats.AddLives(1 + savedUpgradePower);
		}
		else{
			// TODO: Prompt the player that their lives are full.
		}
	}

	// Initiates the upgrade process
	public void BeginUpgrade(){
		// TODO: Check if the player has enough money to afford the upgrade.
		// TODO: Set the object to begin upgrading based on the upgrade time.

	}

	// Upgrades the Cat Bed.
	void UpgradeObject(){
		if (savedUpgradeRank < maxRank){
			RankUp();
		}
		else if (savedUpgradeRank >= maxRank){
			LevelUp();
		}
	}

	// Used to increase the upgrade cost.
	void IncreaseCost(int cost){
		savedUpgradeCost += cost;
		PlayerPrefs.SetInt("cbUpCost", savedUpgradeCost);
	}

	// Used to increase the upgrade timer.
	void IncreaseTime(float time){
		savedUpgradeTime += time;
		PlayerPrefs.SetFloat("cbUpTime", savedUpgradeTime);
	}

	// Ranks up the Cat Bed.
	void RankUp(){
		// TODO: Deduct cost from player money here.
		savedUpgradeRank += 1;
		savedUpgradeSpeed += 1;
		PlayerPrefs.SetInt("cbUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("cbUpSpd", savedUpgradeSpeed);
		if (savedUpgradeRank <= 2){
			IncreaseCost(200);
			IncreaseTime(1);
		}
		else {
			IncreaseCost(500);
			IncreaseTime(2);
		}
	}

	// Levels up the Cat Bed once it reaches max Rank.
	void LevelUp(){
		savedUpgradeRank = 0;
		savedUpgradeLevel += 1;
		savedUpgradePower += 1;
		savedUpgradeSpeed += 1;
		PlayerPrefs.SetInt("cbUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("cbUpLevel", savedUpgradeLevel);
		PlayerPrefs.SetInt("cbUpPow", savedUpgradePower);
		PlayerPrefs.SetInt("cbUpSpd", savedUpgradeSpeed);
		IncreaseCost(200);
		IncreaseTime(1);
	}

	// Countdown timer to upgrade Rank of object.
	void UpgradeCountdown(){
		if (currentUpgradeTime > 0){
			currentUpgradeTime -= Time.deltaTime;
			UpdateTimerText();
		}
		else {
			UpgradeObject();
			currentUpgradeTime = savedUpgradeTime;
			lastUpgradeTime = System.DateTime.Now.Second;
		}
	}

	// Checks to see if the cooldown and upgrade timers
	// have finished since the player's last exit.
	void CheckTimers(){
		timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
		timeSinceLastOpenedGame = System.DateTime.Now.Second - timeGameWasLastOpened;
		if (timeSinceLastOpenedGame > (savedUpgradeTime * convertToHours)){
			UpgradeCountdown();
		}
		if (timeSinceLastOpenedGame > (objectCooldown * convertToHours)){
			UpgradeObject();
		}
	}

	// Updates the cooldown and upgrade timer to reflect the remaining time.
	void UpdateTimerText(){
		//TODO: update timer text
		timerText.text = currentUpgradeTime.ToString();
		cooldownText.text = currentCooldownTime.ToString();
	}
}