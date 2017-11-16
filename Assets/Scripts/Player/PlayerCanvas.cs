using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

    public static PlayerCanvas singleton = null;
    
    [SerializeField] bool isFirstStartup = true;

    // Level and Experience stuff
    int level;
    float xp;
    float nextLevelUpAmount;
    float lastLevelUpAmount;
    [SerializeField] Text levelText;
    [SerializeField] Text xpText;
    [SerializeField] Image expBar;

    // Health Stuff
	[SerializeField] float maxHealth = 5;
	float health;
    [SerializeField] Text healthText;
	[SerializeField] Image healthBarForeground;
	float healthRegenInterval = 5f;
	float lastHealthRegenTime;
	float currentHealthRegenTime = 5f;

    // Lives stuff
    [SerializeField] int maxLives = 9;
    int lives;
    [SerializeField] Text livesText;
    float currentLifeRegenTime = 20f;
	float lifeRegenInterval = 20f;
	float lastLifeRegenTime;

    // Real time tracking stuff
    string debugMessage = "";
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

    // Getters and Setters
    public float GetHealth() { return health; }
    public int GetLives() { return lives; }
    public float GetMaxHealth() { return maxHealth; }
    public bool IsFirstStartup { get{ return isFirstStartup; }}

    void Awake() {   
        //DeleteKeys();   
        if(singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        CheckFirstStartup();
        Load();     
    }

	void Start () {  
		UpdateHealthBar();
	}

    void CheckFirstStartup() {
        int tmp = PlayerPrefs.GetInt("isFirstStartup");
        switch(tmp) {
            case 0:
                isFirstStartup = true;
                break;

            case 1:
                isFirstStartup = false;
                break;

            default:
                isFirstStartup = true;
                break;
        }
    }	

	void Update () {
        UpdateLivesRegeneration();
	}

    void DeleteKeys() {
        PlayerPrefs.DeleteAll();
    }

    public void Save() {
        string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
        PlayerPrefs.SetInt("isFirstStartup", 1);
        PlayerPrefs.SetInt("PlayerHealth", (int)health);
        PlayerPrefs.SetInt("MaxHealth", (int)maxHealth);
        PlayerPrefs.SetInt("PlayerLives", lives);
        PlayerPrefs.SetFloat("NextLevel", nextLevelUpAmount);
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.SetFloat("XP", xp);
    }
    
    void Load() {
        // Load time since game was last opened
        System.DateTime dateTime;
        bool didParse = System.DateTime.TryParse(PlayerPrefs.GetString ("DateTime"), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);
        if (didParse) {
            System.DateTime now = System.DateTime.UtcNow;
            System.TimeSpan timeSpan = now - dateTime;
            timeSinceLastOpenedGame = (float)timeSpan.TotalSeconds;
            debugMessage = string.Format("{0} seconds have passed since the last save.", timeSinceLastOpenedGame);
            Debug.Log(debugMessage);
        } else {
            debugMessage = "Either the DateTime was invalid or there wasn't a saved time.";
            Debug.Log(debugMessage);
        }
        InitHealth();   
        InitLives();   
        InitLevel();
        InitXP();  
    }

	void OnApplicationQuit() {
        Save();
	}

    public void Die() {
        health = 0;
        UpdateLivesText();
        UpdateHealthBar();
    }

    public float TimeSinceLastHealthRegen(float dateTime) {
         return (dateTime - lastHealthRegenTime);
    }

    void InitHealth() {
        if(isFirstStartup) {
            health = maxHealth;
            PlayerPrefs.SetInt("PlayerHealth", (int)maxHealth);
        } else {
            if(PlayerPrefs.HasKey("MaxHealth")) {
                maxHealth = (float)PlayerPrefs.GetInt("MaxHealth");
            }
            health = (float)PlayerPrefs.GetInt("PlayerHealth");
            timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
            if(timeSinceLastOpenedGame > (healthRegenInterval * maxHealth)) {
                health = maxHealth;
                //Debug.Log("greater then");
            } else {
                float healthIntervalsPassed = (timeSinceLastOpenedGame / healthRegenInterval);
                //Debug.Log(healthIntervalsPassed.ToString());
                if(healthIntervalsPassed > 1) {
                    // Calculate how many health regen intervals have passed as a single integer
                    int extraHealth = (int)healthIntervalsPassed;
                    //Debug.Log("less then");
                    //Debug.Log(extraHealth.ToString());
                    //Debug.Log((healthRegenInterval * maxHealth).ToString());
                    health += extraHealth;
                }
            }
        }
        UpdateHealthBar();
    }

    void InitLives() {
        if(isFirstStartup) {
            lives = maxLives;
            PlayerPrefs.SetInt("PlayerLives", maxLives);
        } else {
            lives = PlayerPrefs.GetInt("PlayerLives");
            if(timeSinceLastOpenedGame > (lifeRegenInterval * maxLives)) {
                lives = maxLives;
                health = maxHealth;
            } else {
                // Calculate how many life regen intervals have passed as a single integer
                int extraLives = (int)(timeSinceLastOpenedGame / lifeRegenInterval);
                lives += extraLives;
                if(lives > maxLives) {
                    lives = maxLives;
                }
            }
        }
        UpdateHealthBar();
        UpdateLivesText();
    }

    void InitXP() {
        if(isFirstStartup) {
            xp = 0;
            nextLevelUpAmount = 20;
            PlayerPrefs.SetFloat("NextLevel", nextLevelUpAmount);
        } else {
            xp = PlayerPrefs.GetFloat("XP");
            nextLevelUpAmount = PlayerPrefs.GetFloat("NextLevel");
        }
        UpdateXPText();
        UpdateXPBar();
    }

    void InitLevel() {
        if(isFirstStartup) {
            level = 1;
            PlayerPrefs.SetInt("PlayerLevel", level);
        } else {
            level = PlayerPrefs.GetInt("PlayerLevel");
        }
        UpdateLevelText();
    }

	void UpdateHealthRegeneration() {
        if(health < maxHealth) {
            if(currentHealthRegenTime > 0) {
                currentHealthRegenTime -= Time.deltaTime;
            } else {
                health += 1;
                UpdateHealthBar();
                currentHealthRegenTime = healthRegenInterval;
                lastHealthRegenTime = System.DateTime.Now.Second;
            }
        }
	}

    void UpdateLivesRegeneration() {
        if(lives < maxLives) {
            if(currentLifeRegenTime > 0) {
                currentLifeRegenTime -= Time.deltaTime;
            } else {
                health = maxHealth;
                lives += 1;
                UpdateLivesText();
                UpdateHealthBar();
                currentLifeRegenTime = lifeRegenInterval;
                lastLifeRegenTime = System.DateTime.Now.Second;
            }
        }
	}

    public void Respawn() {        
        health = maxHealth;
        UpdateHealthBar();
        UpdateLivesText();
    }

    public void TakeDamage(float damage) {        			
        health -= damage;
        UpdateHealthBar();
        if(health <= 0) {
            lives--;
            UpdateLivesText();
        }
	}

	void UpdateHealthBar() {
		float currentFillAmount =  health / maxHealth;
		healthBarForeground.GetComponent<Image>().fillAmount = currentFillAmount;
        healthText.text = "" + health.ToString() + "/" + maxHealth;
	}

	void UpdateLivesText() {
        livesText.text = "x " + lives.ToString();
	}

    void UpdateLevelText() {
        levelText.text = "" + level.ToString();
    }

    void UpdateXPText() {
        xpText.text = "XP: " + xp.ToString() + " / " + nextLevelUpAmount.ToString();
    }

    void UpdateXPBar(){
        float currentFillAmount =  xp / nextLevelUpAmount;
		expBar.GetComponent<Image>().fillAmount = currentFillAmount;
    }

    // Add lives to the player.
    public void AddLives(int value){
        lives += value;
        health = maxHealth;
        if (lives > maxLives){
            lives = maxLives;
        }
        PlayerPrefs.SetInt("PlayerLives", maxLives);
        UpdateLivesText();
    }

    // Add experience to the player.
    public void AddXP(float value) {
        // If the player has an exp boost active, grant bonus exp.
        if (PlayerPrefs.GetFloat("ExpDuration") > 0){
            int expBoost = PlayerPrefs.GetInt("ExpBoost");
            xp += value + (value * expBoost * .01f);
        }
        else {
            xp += value;
        }
        if(xp >= nextLevelUpAmount) {
            level++;
            maxHealth += 5;
            PlayerPrefs.SetInt("MaxHealth", (int)maxHealth);
            xp -= nextLevelUpAmount;
            float newLevelUpAmount = (nextLevelUpAmount * 1.5f) + 10;
            nextLevelUpAmount = newLevelUpAmount;
            PlayerPrefs.SetFloat("NextLevel", nextLevelUpAmount);
        }
        PlayerPrefs.SetFloat("XP", xp);
        PlayerPrefs.SetInt("PlayerLevel", level);
        UpdateLevelText();
        UpdateXPText();
        UpdateXPBar();
        //print("added " + value + " exp");
    }
    
    // Add % of max health to the player.
    public void AddPercentHealth(int value) {
        float percentOfMaxHealth = maxHealth * (value * .01f);
        health += percentOfMaxHealth;
        if (health > maxHealth){
            health = maxHealth;
        }
    }
}