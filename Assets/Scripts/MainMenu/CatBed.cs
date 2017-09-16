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
	[SerializeField] Text objectDescription;
	private float convertToHours = 3600f;
	private int livesRestoreAmount = 1;
	private int maxRank = 3;
	private float currentBoostDuration;
	// Cooldown
	[SerializeField] GameObject cooldownLocked;
	[SerializeField] Image cooldownBar;
	[SerializeField] Text cooldownTimer;
	private float currentCooldownTime = 129600; // 36 hours * 3600 = converted to seconds
	private int objectCooldown = 129600; // 36 hours * 3600 = converted to seconds
	// Upgrade
	[SerializeField] GameObject upgradeLocked;
	[SerializeField] Image upgradeBar;
	[SerializeField] Text upgradeCost;
	[SerializeField] Text upgradeTimer;
	private float currentUpgradeTime = 7200; // 2 hours * 3600 = converted to seconds
	// Rank and Level
	[SerializeField] Image rankBar;
	[SerializeField] Text levelNumber;

	// Player Prefs for upgrades.
	public int savedUpgradeCost;
	public int savedUpgradeLevel = 1;
	public int savedUpgradePower = 0;
	public int savedUpgradeRank = 0;
	public int savedUpgradeDuration = 0;
	public float savedUpgradeTime = 7200; // 2 hours * 3600 = converted to seconds
    // Real time tracking stuff
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

	void Awake(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
		CheckTimers();
	}

	void Start(){
		// If the object is still upgrading, lock the button.
		if (PlayerPrefs.GetInt("cbIsUp") == 1){
			upgradeLocked.SetActive(true);
		}
		// If the object is still on cooldown, lock the button.
		if (PlayerPrefs.GetInt("cbIsOnCd") == 1){
			cooldownLocked.SetActive(true);
		}
		UpdateCostText();
		UpdateRankBar();
	}

	void Update(){
		UpgradeCountdown();
		CooldownCountdown();
	}

	void OnApplicationQuit() {
        SavePrefs();
	}

	public void GetPrefs(){
		savedUpgradeCost = PlayerPrefs.GetInt("cbUpCost");
		savedUpgradeLevel = PlayerPrefs.GetInt("cbLevel");
		savedUpgradePower = PlayerPrefs.GetInt("cbUpPow");
		savedUpgradeRank = PlayerPrefs.GetInt("cbUpRank");
		savedUpgradeDuration = PlayerPrefs.GetInt("cbUpDur");
		savedUpgradeTime = PlayerPrefs.GetFloat("cbUpTime");
	}

	public void SetPrefs(){
		PlayerPrefs.SetInt("cbUpCost", 200);
		PlayerPrefs.SetInt("cbLevel", 1);
		PlayerPrefs.SetInt("cbUpPow", 0);
		PlayerPrefs.SetInt("cbUpRank", 0);
		PlayerPrefs.SetInt("cbUpSpd", 0);
		PlayerPrefs.SetFloat("cbUpTime", 7200);
		PlayerPrefs.SetInt("cbIsOnCd", 0);
		PlayerPrefs.SetInt("cbIsUp", 0);
	}

	public void SavePrefs(){
		string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
		PlayerPrefs.SetFloat("cbUpTimer", currentUpgradeTime);
        PlayerPrefs.SetFloat("cbCdTimer", currentCooldownTime);
		PlayerPrefs.SetFloat("ExpDuration", currentBoostDuration);
	}

	// Opens the window for the Cat Bed.
	public void ShowObjectWindow(){
		// TODO: Slide window on to screen over 1 second.
		objectDescription.text = "Take a cat nap, granting " + (20 + (10 * savedUpgradePower)) + "% bonus experience for " + (6 + (1 * savedUpgradeDuration)) + " minutes.";
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
	}

	// Closes the window for the Cat Bed.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}

	// Uses the Cat Bed, which grants bonus exp for a short duration.
	public void UseObject(){
		PlayerPrefs.SetInt("ExpBoost", 20 + (10 * savedUpgradePower));
		PlayerPrefs.SetFloat("ExpDuration", 6 + (1 * savedUpgradeDuration));
		cooldownLocked.SetActive(true);
		PlayerPrefs.SetInt("cbIsOnCd", 1);
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
			PlayerPrefs.SetInt("cbIsUp", 1);
			mainMenu.CloseConfirmationWindow();
		}
		else {
			// TODO: Prompt Gold purchase in Shop.
		}
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
		savedUpgradeTime += time * convertToHours;
		PlayerPrefs.SetFloat("cbUpTime", savedUpgradeTime);
	}

	// Ranks up the Cat Bed.
	void RankUp(){
		PlayerPrefs.SetInt("cbIsUp", 0);
		savedUpgradeRank += 1;
		savedUpgradeDuration += 1;
		PlayerPrefs.SetInt("cbUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("cbUpDur", savedUpgradeDuration);
		if (savedUpgradeRank <= 3){
			IncreaseCost(200);
			IncreaseTime(1);
		}
		else {
			IncreaseCost(500);
			IncreaseTime(2);
		}
		UpdateRankBar();
		UpdateCostText();
		GetPrefs();
	}

	// Levels up the Cat Bed once it reaches max Rank.
	void LevelUp(){
		PlayerPrefs.SetInt("cbIsUp", 0);
		savedUpgradeRank = 0;
		savedUpgradeLevel += 1;
		savedUpgradePower += 1;
		PlayerPrefs.SetInt("cbUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("cbUpLevel", savedUpgradeLevel);
		PlayerPrefs.SetInt("cbUpPow", savedUpgradePower);
		IncreaseCost(200);
		IncreaseTime(1);
		UpdateRankBar();
		UpdateCostText();
		GetPrefs();
	}

	// Countdown timer to upgrade Rank of object.
	void UpgradeCountdown(){
		if (PlayerPrefs.GetInt("cbIsUp") == 1){
			if (currentUpgradeTime > 0){
				currentUpgradeTime -= Time.deltaTime;
				UpdateUpgradeText();
			}
			else {
				UpgradeObject();
				PlayerPrefs.SetInt("cbIsUp", 0);
				currentUpgradeTime = savedUpgradeTime;
				UpdateUpgradeText();
			}

		}
	}

	// Cooldown timer to refresh use of object.
	void CooldownCountdown(){
		if (PlayerPrefs.GetInt("cbIsOnCd") == 1){
			if (currentCooldownTime > 0){
				currentCooldownTime -= Time.deltaTime;
				UpdateCooldownText();
			}
			else {
				UnlockObject();
				PlayerPrefs.SetInt("cbIsOnCd", 0);
				currentCooldownTime = objectCooldown;
				UpdateCooldownText();
			}
		}
	}

	// Timer for duration of exp boost.
	void ExpBoostCountdown(){
		if (PlayerPrefs.GetFloat("ExpDuration") > 0){
			if (currentBoostDuration > 0){
				currentBoostDuration -= Time.deltaTime;
				//UpdateDurationText();
			}
			else {
				PlayerPrefs.SetFloat("ExpDuration", 0);
				currentBoostDuration = 0;
				//UpdateDurationText();
			}
		}
	}

	// Unlocks the Cat Bed to be used again.
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

		float tempUpgradeTime = PlayerPrefs.GetFloat("cbUpTimer");
		currentUpgradeTime = tempUpgradeTime - timeSinceLastOpenedGame;
		float tempCooldownTime = PlayerPrefs.GetFloat("cbCdTimer");
		currentCooldownTime = tempCooldownTime - timeSinceLastOpenedGame;
		float tempDurationTime = PlayerPrefs.GetFloat("ExpDuration");
		currentBoostDuration = tempDurationTime - timeSinceLastOpenedGame;
		// If there is still time remaining on upgrade, set it to "is upgrading"
		if (currentUpgradeTime > 0){
			PlayerPrefs.SetInt("cbIsUp", 1);
		}
		// If there is still time remaining on cooldown, set it to "is on cooldown"
		if (currentCooldownTime > 0){
			PlayerPrefs.SetInt("cbIsOnCd", 1);
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