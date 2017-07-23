using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {
    
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

    void Awake() {        
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
    public void Save() {
        string dateTimeString = System.DateTime.UtcNow.ToString (System.Globalization.CultureInfo.InvariantCulture);
        PlayerPrefs.SetString ("DateTime", dateTimeString);
		PlayerPrefs.SetString("LastExitTime", new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc).ToString());
        PlayerPrefs.SetInt("isFirstStartup", 1);
        PlayerPrefs.SetFloat("PlayerHealth", health);
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
            PlayerPrefs.SetFloat("PlayerHealth", maxHealth);
        } else {
            health = PlayerPrefs.GetFloat("health");
            // timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
            // timeSinceLastOpenedGame = System.DateTime.Now.Second - timeGameWasLastOpened;
            // if(timeSinceLastOpenedGame > (healthRegenInterval * maxHealth)) {
            //     health = maxHealth;
            // } else {
            //     // TODO: Calculate health
            //     health = maxHealth;
            // }
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
            } else {
                // Calculate how many life regen intervals have passed as a single integer
                int index = (int)(timeSinceLastOpenedGame / lifeRegenInterval);
                int extraLives = 0;
                switch(index) {
                    case 0:
                        extraLives = 0;
                        break;

                    case 1:
                        extraLives = 1;
                        break;

                    case 2:
                        extraLives = 2;
                        break;

                    case 3:
                        extraLives = 3;
                        break;

                    case 4:
                        extraLives = 4;
                        break;

                    case 5:
                        extraLives = 5;
                        break;

                    case 6:
                        extraLives = 6;
                        break;

                    case 7:
                        extraLives = 7;
                        break;

                    case 8:
                        extraLives = 8;
                        break;

                    case 9:
                        extraLives = 9;
                        break;

                    default:
                        extraLives = 0;
                        break;
                    
                }
                lives += extraLives;
                if(lives > maxLives) {
                    lives = maxLives;
                }
                health = maxHealth;
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

    public void AddLives(int value){
        lives += value;
        health = maxHealth;
        if (lives > maxLives){
            lives = maxLives;
        }
        PlayerPrefs.SetInt("PlayerLives", maxLives);
        UpdateLivesText();
    }

    public void AddXP(float value) {
        xp += value;
        if(xp >= nextLevelUpAmount) {
            level++;
            xp -= nextLevelUpAmount;
            float newLevelUpAmount = (nextLevelUpAmount * 1.25f) + 20;
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
    
    public void AddLife(int value) {
        if(lives < maxLives) {
            lives += value;
            health = maxHealth;
        } else {
            Debug.Log("Warning: Lives are already at maximum!!");
        }
    }
}