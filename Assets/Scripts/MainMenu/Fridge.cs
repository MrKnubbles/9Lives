using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class Fridge : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;
	// Fridge
	public GameObject objectWindow;
	[SerializeField] Text objectDescription;
	private float convertToHours = 3600f;
	private int healthRestoreAmount = 25; // percentage of max health.
	private int maxRank = 3;
	// Cooldown
	[SerializeField] GameObject cooldownLocked;
	[SerializeField] Image cooldownBar;
	[SerializeField] Text cooldownTimer;
	private float currentCooldownTime = 72000; // 20 hours * 3600 = converted to seconds
	private int objectCooldown = 72000; // 20 hours * 3600 = converted to seconds
	// Upgrade
	[SerializeField] GameObject upgradeLocked;
	[SerializeField] Image upgradeBar;
	[SerializeField] Text upgradeCost;
	[SerializeField] Text upgradeTimer;
	private float currentUpgradeTime = 1800; // .5 hours * 3600 = converted to seconds
	// Rank and Level
	[SerializeField] Image rankBar;
	[SerializeField] Text levelNumber;

	// Player Prefs for upgrades.
	public int savedUpgradeCost;
	public int savedUpgradeLevel = 1;
	public int savedUpgradePower = 0;
	public int savedUpgradeRank = 0;
	public int savedUpgradeSpeed = 0;
	public float savedUpgradeTime = 1800; // .5 hours * 3600 = converted to seconds
    // Real time tracking stuff
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

	void Awake(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
		CheckTimers();
	}

	void Start(){
		// If the object is still upgrading, lock the button.
		if (PlayerPrefs.GetInt("fIsUp") == 1){
			upgradeLocked.SetActive(true);
		}
		// If the object is still on cooldown, lock the button.
		if (PlayerPrefs.GetInt("fIsOnCd") == 1){
			cooldownLocked.SetActive(true);
		}
	}

	void Update(){
		UpgradeCountdown();
		CooldownCountdown();
	}

	void OnApplicationQuit() {
        SavePrefs();
	}

	public void GetPrefs(){
		savedUpgradeCost = PlayerPrefs.GetInt("fUpCost");
		savedUpgradeLevel = PlayerPrefs.GetInt("fLevel");
		savedUpgradePower = PlayerPrefs.GetInt("fUpPow");
		savedUpgradeRank = PlayerPrefs.GetInt("fUpRank");
		savedUpgradeSpeed = PlayerPrefs.GetInt("fUpSpd");
		savedUpgradeTime = PlayerPrefs.GetFloat("fUpTime");
	}

	public void SetPrefs(){
		PlayerPrefs.SetInt("fUpCost", 0);
		PlayerPrefs.SetInt("fLevel", 1);
		PlayerPrefs.SetInt("fUpPow", 0);
		PlayerPrefs.SetInt("fUpRank", 0);
		PlayerPrefs.SetInt("fUpSpd", 0);
		PlayerPrefs.SetFloat("fUpTime", 7200);
		PlayerPrefs.SetInt("fIsOnCd", 0);
		PlayerPrefs.SetInt("fIsUp", 0);
	}

	public void SavePrefs(){
		string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
		PlayerPrefs.SetFloat("fUpTimer", currentUpgradeTime);
        PlayerPrefs.SetFloat("fCdTimer", currentCooldownTime);
	}

	// Opens the window for the Fridge.
	public void ShowObjectWindow(){
		// TODO: Slide window onto screen.
		if (savedUpgradePower < 6){
			objectDescription.text = "Have a snack, restoring " + (savedUpgradePower - 5) + " lives.";
		}
		else {
			objectDescription.text = "Have a snack, restoring " + (healthRestoreAmount + (savedUpgradePower * 15)) + "% health.";
		}
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
	}

	// Closes the window for the Fridge.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}

	// Uses the Fridge, which restores health or lives (based on level) with a cooldown.
	public void UseObject(){
		if (savedUpgradePower < 6){
			if (playerStats.GetHealth() < playerStats.GetMaxHealth()){
				playerStats.AddPercentHealth(50 + (5 * savedUpgradePower));
				cooldownLocked.SetActive(true);
				PlayerPrefs.SetInt("fIsOnCd", 1);
			}
			else{
				// TODO: Prompt the player that their health is full.
			}

		}
		else {
			if (playerStats.GetLives() < 9){
				playerStats.AddLives(savedUpgradePower - 5);
				cooldownLocked.SetActive(true);
				PlayerPrefs.SetInt("fIsOnCd", 1);
			}
			else{
				// TODO: Prompt the player that their lives are full.
			}
		}
	}

	// Initiates the upgrade process
	public void BeginUpgrade(){
		// If player has enough money...
		// Deduct cost from player and begin upgrading.
		if (PlayerPrefs.GetInt("Coins") >= savedUpgradeCost){
			int tempGold = PlayerPrefs.GetInt("Coins");
			tempGold -= savedUpgradeCost;
			PlayerPrefs.SetInt("Coins", tempGold);
			upgradeLocked.SetActive(true);
			PlayerPrefs.SetInt("fIsUp", 1);
			mainMenu.CloseConfirmationWindow();
		}
		else {
			// TODO: Prompt Gold purchase in Shop.
		}
	}

	// Upgrades the Fridge.
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
		PlayerPrefs.SetInt("fUpCost", savedUpgradeCost);
	}

	// Used to increase the upgrade timer.
	void IncreaseTime(float time){
		savedUpgradeTime += time * convertToHours;
		PlayerPrefs.SetFloat("fUpTime", savedUpgradeTime);
	}

	// Ranks up the Fridge.
	void RankUp(){
		PlayerPrefs.SetInt("fIsUp", 0);
		savedUpgradeRank += 1;
		savedUpgradeSpeed += 1;
		PlayerPrefs.SetInt("fUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("fUpSpd", savedUpgradeSpeed);
		if (savedUpgradeRank <= 3){
			IncreaseCost(100);
			IncreaseTime(1);
		}
		else {
			IncreaseCost(250);
			IncreaseTime(2);
		}
		UpdateRankBar();
		UpdateCostText();
	}

	// Levels up the Fridge once it reaches max Rank.
	void LevelUp(){
		PlayerPrefs.SetInt("fIsUp", 0);
		savedUpgradeRank = 0;
		savedUpgradeLevel += 1;
		savedUpgradePower += 1;
		PlayerPrefs.SetInt("fUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("fUpLevel", savedUpgradeLevel);
		PlayerPrefs.SetInt("fUpPow", savedUpgradePower);
		IncreaseCost(100);
		IncreaseTime(1);
		UpdateRankBar();
		UpdateCostText();
	}

	// Countdown timer to upgrade Rank of object.
	void UpgradeCountdown(){
		if (PlayerPrefs.GetInt("fIsUp") == 1){
			if (currentUpgradeTime > 0){
				currentUpgradeTime -= Time.deltaTime;
				UpdateUpgradeText();
			}
			else {
				UpgradeObject();
				PlayerPrefs.SetInt("fIsUp", 0);
				currentUpgradeTime = savedUpgradeTime;
				UpdateUpgradeText();
			}

		}
	}

	// Cooldown timer to refresh use of object.
	void CooldownCountdown(){
		if (PlayerPrefs.GetInt("fIsOnCd") == 1){
			if (currentCooldownTime > 0){
				currentCooldownTime -= Time.deltaTime;
				UpdateCooldownText();
			}
			else {
				UnlockObject();
				PlayerPrefs.SetInt("fIsOnCd", 0);
				currentCooldownTime = objectCooldown;
				UpdateCooldownText();
			}
		}
	}

	// Unlocks the Fridge to be used again.
	void UnlockObject(){
		cooldownLocked.SetActive(false);
		ResetCooldownTimer();
	}

	// Sets cooldown timer to max cooldown.
	void ResetCooldownTimer(){
		System.TimeSpan t = System.TimeSpan.FromSeconds(objectCooldown);
     	if (t.Days > 0){
     		cooldownTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours + 24, t.Minutes, t.Seconds);
		}
		else{
     		cooldownTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
		}
	}

	// Checks to see if the cooldown and upgrade timers
	// have finished since the player's last exit.
	void CheckTimers(){
		// Load time since game was last opened
        System.DateTime dateTime;
        bool didParse = System.DateTime.TryParse(PlayerPrefs.GetString("DateTime"), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);
        if (didParse) {
            System.DateTime now = System.DateTime.UtcNow;
            System.TimeSpan timeSpan = now - dateTime;
            timeSinceLastOpenedGame = (float)timeSpan.TotalSeconds;
        }

		float tempUpgradeTime = PlayerPrefs.GetFloat("fUpTimer");
		currentUpgradeTime = tempUpgradeTime - timeSinceLastOpenedGame;
		float tempCooldownTime = PlayerPrefs.GetFloat("fCdTimer");
		currentCooldownTime = tempCooldownTime - timeSinceLastOpenedGame;
		// If there is still time remaining on upgrade, set it to "is upgrading"
		if (currentUpgradeTime > 0){
			PlayerPrefs.SetInt("fIsUp", 1);
		}
		// If there is still time remaining on cooldown, set it to "is on cooldown"
		if (currentCooldownTime > 0){
			PlayerPrefs.SetInt("fIsOnCd", 1);
		}
	}

	// Updates the upgrade timer to reflect the remaining time in hours:minutes:seconds.
	void UpdateUpgradeText(){
		System.TimeSpan t = System.TimeSpan.FromSeconds(currentUpgradeTime);
     	if (t.Days > 0){
     		upgradeTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours + 24, t.Minutes, t.Seconds);
		}
		else{
     		upgradeTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
		}
		UpdateUpgradeBar();
	}

	// Updates the cooldown timer to reflect the remaining time in hours:minutes:seconds.
	void UpdateCooldownText(){
		System.TimeSpan t = System.TimeSpan.FromSeconds(currentCooldownTime);
		if (t.Days > 0){
     		cooldownTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours + 24, t.Minutes, t.Seconds);
		}
		else{
     		cooldownTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
		}
		UpdateCooldownBar();
	}

	// Updates the Rank bar to show current rank and level.
	void UpdateRankBar(){
		levelNumber.text = "" + savedUpgradeLevel;
		rankBar.fillAmount = savedUpgradeRank / maxRank;
	}

	// Updates UI to reflect current upgrade costs.
	void UpdateCostText(){
		upgradeCost.text = "" + savedUpgradeCost;
	}

	// Updates cooldown bar to show % fill amount.
	void UpdateCooldownBar(){
		cooldownBar.fillAmount = 1 - (currentCooldownTime / objectCooldown);
	}

	// Updates upgrade bar to show % fill amount.
	void UpdateUpgradeBar(){
		upgradeBar.fillAmount = 1 - (currentUpgradeTime / savedUpgradeTime);
	}
}