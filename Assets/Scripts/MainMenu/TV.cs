using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class TV : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;
	[SerializeField] private UnityAds unityAds;
	// TV
	public GameObject objectWindow;
	[SerializeField] Text objectDescription;
	private float convertToHours = 3600f;
	private int maxRank = 3;
	// Reward
	[SerializeField] private GameObject rewardWindow;
	[SerializeField] private GameObject noRewardWindow;
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
	public int savedUpgradeSpeed = 0;
	public float savedUpgradeTime = 7200; // 2 hours * 3600 = converted to seconds
    // Real time tracking stuff
    float timeSinceLastOpenedGame;


	void Awake(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
		CheckTimers();
	}

	void Start(){
		// If the object is still upgrading, lock the button.
		if (PlayerPrefs.GetInt("tvIsUp") == 1){
			upgradeLocked.SetActive(true);
		}
		// If the object is still on cooldown, lock the button.
		if (PlayerPrefs.GetInt("tvIsOnCd") == 1){
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
		savedUpgradeCost = PlayerPrefs.GetInt("tvUpCost");
		savedUpgradeLevel = PlayerPrefs.GetInt("tvLevel");
		savedUpgradePower = PlayerPrefs.GetInt("tvUpPow");
		savedUpgradeRank = PlayerPrefs.GetInt("tvUpRank");
		savedUpgradeSpeed = PlayerPrefs.GetInt("tvUpSpd");
		savedUpgradeTime = PlayerPrefs.GetFloat("tvUpTime");
	}

	public void SetPrefs(){
		PlayerPrefs.SetInt("tvUpCost", 500);
		PlayerPrefs.SetInt("tvLevel", 1);
		PlayerPrefs.SetInt("tvUpPow", 0);
		PlayerPrefs.SetInt("tvUpRank", 0);
		PlayerPrefs.SetInt("tvUpSpd", 0);
		PlayerPrefs.SetFloat("tvUpTime", 7200);
		PlayerPrefs.SetInt("tvIsOnCd", 0);
		PlayerPrefs.SetInt("tvIsUp", 0);
	}

	public void SavePrefs(){
		string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
		PlayerPrefs.SetFloat("tvUpTimer", currentUpgradeTime);
        PlayerPrefs.SetFloat("tvCdTimer", currentCooldownTime);
	}

	// Opens the window for the TV.
	public void ShowObjectWindow(){
		// TODO: Slide window on to screen over 1 second.
		if (savedUpgradeSpeed > 3){
			objectDescription.text = "Watch an Ad, earning " + (100 + savedUpgradePower) + " Gold and " + (savedUpgradeSpeed - 3) + " Gems.";
		}
		else if (savedUpgradePower > 0){
			objectDescription.text = "Watch an Ad, earning " + (100 + savedUpgradePower) + " Gold.";
		}
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
	}

	// Closes the window for the TV.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}

	public void CloseRewardWindows(){
		rewardWindow.SetActive(false);
		noRewardWindow.SetActive(false);
	}

	// Uses the TV, which prompts the user to watch an Ad for a reward.
	public void UseObject(){
		mainMenu.PromptAdConfirmation();
	}

	// Show Ad.
	public void ShowAd(){
		unityAds.ShowAd("reward");
	}

	// Grants the player a reward after watching the video Ad.
	public void RewardForAd(){
		// Grants Gold
		int tempGold = PlayerPrefs.GetInt("Coins");
		tempGold += (100 + savedUpgradePower);
		PlayerPrefs.SetInt("Coins", tempGold);
		// Also grants Gems if TV Level is 5+
		if (savedUpgradeSpeed > 4){
			int tempGems = PlayerPrefs.GetInt("Gems");
			tempGems += (savedUpgradeSpeed - 3);
			PlayerPrefs.SetInt("Gems", tempGems);
		}
		mainMenu.UpdateCurrencies();
		mainMenu.CloseWatchAdWindow();
		CloseRewardWindows();
		rewardWindow.SetActive(true);
		// TODO: Grant bonus Gold for watching multiple times in a row.
	}

	// Grants no reward since the player skipped the Ad or it failed to load.
	public void NoRewardForAd(){
		mainMenu.CloseWatchAdWindow();
		CloseRewardWindows();
		noRewardWindow.SetActive(true);
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
			PlayerPrefs.SetInt("tvIsUp", 1);
			mainMenu.CloseConfirmationWindow();
		}
		else {
			// TODO: Prompt Gold purchase in Shop.
		}
	}

	// Upgrades the TV.
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
		PlayerPrefs.SetInt("tvUpCost", savedUpgradeCost);
	}

	// Used to increase the upgrade timer.
	void IncreaseTime(float time){
		savedUpgradeTime += time * convertToHours;
		PlayerPrefs.SetFloat("tvUpTime", savedUpgradeTime);
	}

	// Ranks up the TV.
	void RankUp(){
		PlayerPrefs.SetInt("tvIsUp", 0);
		savedUpgradeRank += 1;
		savedUpgradePower += 1;
		PlayerPrefs.SetInt("tvUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("tvUpPow", savedUpgradePower);
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
	}

	// Levels up the TV once it reaches max Rank.
	void LevelUp(){
		PlayerPrefs.SetInt("tvIsUp", 0);
		savedUpgradeRank = 0;
		savedUpgradeLevel += 1;
		savedUpgradeSpeed += 1;
		PlayerPrefs.SetInt("tvUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("tvUpSpd", savedUpgradeSpeed);
		PlayerPrefs.SetInt("tvUpLevel", savedUpgradeLevel);
		IncreaseCost(200);
		IncreaseTime(1);
		UpdateRankBar();
		UpdateCostText();
	}

	// Countdown timer to upgrade Rank of TV.
	void UpgradeCountdown(){
		if (PlayerPrefs.GetInt("tvIsUp") == 1){
			if (currentUpgradeTime > 0){
				currentUpgradeTime -= Time.deltaTime;
				UpdateUpgradeText();
			}
			else {
				UpgradeObject();
				PlayerPrefs.SetInt("tvIsUp", 0);
				currentUpgradeTime = savedUpgradeTime;
				UpdateUpgradeText();
			}

		}
	}

	// Cooldown timer to refresh use of TV.
	void CooldownCountdown(){
		if (PlayerPrefs.GetInt("tvIsOnCd") == 1){
			if (currentCooldownTime > 0){
				currentCooldownTime -= Time.deltaTime;
				UpdateCooldownText();
			}
			else {
				UnlockObject();
				PlayerPrefs.SetInt("tvIsOnCd", 0);
				currentCooldownTime = objectCooldown;
				UpdateCooldownText();
			}
		}
	}

	// Unlocks the TV to be used again.
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

		float tempUpgradeTime = PlayerPrefs.GetFloat("tvUpTimer");
		currentUpgradeTime = tempUpgradeTime - timeSinceLastOpenedGame;
		float tempCooldownTime = PlayerPrefs.GetFloat("tvCdTimer");
		currentCooldownTime = tempCooldownTime - timeSinceLastOpenedGame;
		// If there is still time remaining on upgrade, set it to "is upgrading"
		if (currentUpgradeTime > 0){
			PlayerPrefs.SetInt("tvIsUp", 1);
		}
		// If there is still time remaining on cooldown, set it to "is on cooldown"
		if (currentCooldownTime > 0){
			PlayerPrefs.SetInt("tvIsOnCd", 1);
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