using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class Bank : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;
	// Bank
	public GameObject objectWindow;
	[SerializeField] Text objectDescription;
	private float convertToHours = 3600f;
	private int maxRank = 3;
	// Gold Regen
	[SerializeField] GameObject cooldownLocked;
	[SerializeField] Image regenBar;
	[SerializeField] Text regenTimer;
	private float currentRegenTime = 0;
	private float goldRegenTimer = 60; // Generate gold every 60 seconds.
	private int goldRegenAmount = 1; // Generate 1 gold every tick.
	private int maxGold = 100;
	// Upgrade
	[SerializeField] GameObject upgradeLocked;
	[SerializeField] Image upgradeBar;
	[SerializeField] Text upgradeCost;
	[SerializeField] Text upgradeTimer;
	private float currentUpgradeTime = 10800; // 3 hours * 3600 = converted to seconds
	// Rank and Level
	[SerializeField] Image rankBar;
	[SerializeField] Text levelNumber;

	// Player Prefs for upgrades.
	public int savedUpgradeCost;
	public int savedUpgradeLevel = 1;
	public int savedUpgradePower = 0;
	public int savedUpgradeRank = 0;
	public int savedUpgradeCap = 0;
	public float savedUpgradeTime = 10800; // 3 hours * 3600 = converted to seconds
	public float savedRegenTime = 0;
	public int savedGoldGenerated = 0;
    // Real time tracking stuff
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

	void Awake(){
		playerStats = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvas>();
		CheckTimers();
	}

	void Start(){
		// If the object is still upgrading, lock the button.
		if (PlayerPrefs.GetInt("bIsUp") == 1){
			upgradeLocked.SetActive(true);
		}
		// If the bank is empty, lock the button.
		if (PlayerPrefs.GetFloat("bGoldGen") == 0){
			cooldownLocked.SetActive(true);
		}
		else{
			cooldownLocked.SetActive(false);
		}
		UpdateCostText();
		UpdateRankBar();
	}

	void Update(){
		UpgradeCountdown();
		RegenCountdown();
	}

	void OnApplicationQuit() {
        SavePrefs();
	}

	public void GetPrefs(){
		savedUpgradeCost = PlayerPrefs.GetInt("bUpCost");
		savedUpgradeLevel = PlayerPrefs.GetInt("bLevel");
		savedUpgradePower = PlayerPrefs.GetInt("bUpPow");
		savedUpgradeRank = PlayerPrefs.GetInt("bUpRank");
		savedUpgradeCap = PlayerPrefs.GetInt("bUpCap");
		savedUpgradeTime = PlayerPrefs.GetFloat("bUpTime");
		savedRegenTime = PlayerPrefs.GetFloat("bRegTime");
		savedGoldGenerated = PlayerPrefs.GetInt("bGoldGen");
	}

	public void SetPrefs(){
		PlayerPrefs.SetInt("bUpCost", 100);
		PlayerPrefs.SetInt("bLevel", 1);
		PlayerPrefs.SetInt("bUpPow", 0);
		PlayerPrefs.SetInt("bUpRank", 0);
		PlayerPrefs.SetInt("bUpCap", 0);
		PlayerPrefs.SetFloat("bUpTime", 7200);
		PlayerPrefs.SetInt("bIsUp", 0);
	}

	public void SavePrefs(){
		string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
		PlayerPrefs.SetFloat("bUpTimer", currentUpgradeTime);
        PlayerPrefs.SetFloat("bRegTimer", currentRegenTime);
		PlayerPrefs.SetInt("bGoldGen", savedGoldGenerated);
	}

	// Opens the window for the Bank.
	public void ShowObjectWindow(){
		// TODO: Slide window onto screen.
		UpdateDescription();
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
	}

	// Closes the window for the Bank.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}

	// Uses the Bank, which claims all generated gold.
	public void UseObject(){
		PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + savedGoldGenerated);
		savedGoldGenerated = 0;
		PlayerPrefs.SetInt("bGoldGen", savedGoldGenerated);
		mainMenu.UpdateCurrencies();
		UpdateDescription();
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
			PlayerPrefs.SetInt("bIsUp", 1);
			mainMenu.CloseConfirmationWindow();
		}
		else {
			// TODO: Prompt Gold purchase in Shop.
		}
	}

	// Upgrades the Bank.
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
		PlayerPrefs.SetInt("bUpCost", savedUpgradeCost);
	}

	// Used to increase the upgrade timer.
	void IncreaseTime(float time){
		savedUpgradeTime += time * convertToHours;
		PlayerPrefs.SetFloat("bUpTime", savedUpgradeTime);
	}

	// Ranks up the Bank.
	void RankUp(){
		PlayerPrefs.SetInt("bIsUp", 0);
		savedUpgradeRank += 1;
		savedUpgradeCap += 1;
		PlayerPrefs.SetInt("bUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("bUpCap", savedUpgradeCap);
		if (savedUpgradeRank <= 3){
			IncreaseCost(500);
			IncreaseTime(1);
		}
		else {
			IncreaseCost(1000);
			IncreaseTime(2);
		}
		UpdateRankBar();
		UpdateCostText();
	}

	// Levels up the Bank once it reaches max Rank.
	void LevelUp(){
		PlayerPrefs.SetInt("bIsUp", 0);
		savedUpgradeRank = 0;
		savedUpgradeLevel += 1;
		savedUpgradePower += 1;
		PlayerPrefs.SetInt("bUpRank", savedUpgradeRank);
		PlayerPrefs.SetInt("bUpLevel", savedUpgradeLevel);
		PlayerPrefs.SetInt("bUpPow", savedUpgradePower);
		IncreaseCost(500);
		IncreaseTime(1);
		UpdateRankBar();
		UpdateCostText();
	}

	// Countdown timer to upgrade Rank of object.
	void UpgradeCountdown(){
		if (PlayerPrefs.GetInt("bIsUp") == 1){
			if (currentUpgradeTime > 0){
				currentUpgradeTime -= Time.deltaTime;
				UpdateUpgradeText();
			}
			else {
				UpgradeObject();
				PlayerPrefs.SetInt("bIsUp", 0);
				currentUpgradeTime = savedUpgradeTime;
				UpdateUpgradeText();
			}

		}
	}

	// Regen timer to track gold regeneration.
	void RegenCountdown(){
		if (currentRegenTime > 0){
			currentRegenTime -= Time.deltaTime;
			UpdateRegenText();
		}
		else {
			UnlockObject();
			savedGoldGenerated += goldRegenAmount;
			PlayerPrefs.SetInt("bGoldGen", savedGoldGenerated);
			currentRegenTime = goldRegenTimer;
			UpdateRegenText();
			UpdateDescription();
		}
	}

	// Unlocks the Bank to be used again.
	void UnlockObject(){
		cooldownLocked.SetActive(false);
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

		float tempUpgradeTime = PlayerPrefs.GetFloat("bUpTimer");
		currentUpgradeTime = tempUpgradeTime - timeSinceLastOpenedGame;
		float tempRegenTime = PlayerPrefs.GetFloat("bRegTimer");
		currentRegenTime = tempRegenTime - timeSinceLastOpenedGame;
		// If there is still time remaining on upgrade, set it to "is upgrading"
		if (currentUpgradeTime > 0){
			PlayerPrefs.SetInt("bIsUp", 1);
		}
		if (timeSinceLastOpenedGame > (goldRegenTimer * (maxGold / goldRegenAmount))){
			savedGoldGenerated = maxGold;
			PlayerPrefs.SetInt("bGoldGen", savedGoldGenerated);
			UpdateDescription();
		}
		else {
			// Calculate how many gold regen intervals have passed as a single integer
			int index = (int)(timeSinceLastOpenedGame / goldRegenTimer);
			savedGoldGenerated = index *= goldRegenAmount;
			PlayerPrefs.SetInt("bGoldGen", savedGoldGenerated);
			UpdateDescription();
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

	// Updates the regen timer to reflect the remaining time in hours:minutes:seconds.
	void UpdateRegenText(){
		System.TimeSpan t = System.TimeSpan.FromSeconds(currentRegenTime);
     	regenTimer.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
		UpdateRegenBar();
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

	// Updates regen bar to show % fill amount.
	void UpdateRegenBar(){
		regenBar.fillAmount = 1 - (currentRegenTime / goldRegenTimer);
	}

	// Updates upgrade bar to show % fill amount.
	void UpdateUpgradeBar(){
		upgradeBar.fillAmount = 1 - (currentUpgradeTime / savedUpgradeTime);
	}

	// Updates description to show current gold to be claimed.
	void UpdateDescription(){
		objectDescription.text = "Collect your savings, earning " + (savedGoldGenerated) + " gold.\nMaximum " + (maxGold + (50 * savedUpgradeCap)) + " gold.";
	}
}