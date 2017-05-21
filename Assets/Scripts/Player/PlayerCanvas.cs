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
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

    // Getters and Setters
    public float GetHealth() { return health; }
    public int GetLives() { return lives; }

    void Awake() {        
        DontDestroyOnLoad(this);
        CheckFirstStartup(); 

        InitHealth();   
        InitLives();   
        InitLevel();
        InitXP();      
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
        UpdateHealthRegeneration();
        UpdateLivesRegeneration();
        // float derp = TimeSinceLastHealthRegen(System.DateTime.Now.Second);
		// Debug.Log(derp);
        //float ddd = System.DateTime.Now.Second;
		//Debug.Log(ddd);
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetFloat("LastExitTime", (float)System.DateTime.Now.Second);
        PlayerPrefs.SetInt("isFirstStartup", 1);
        PlayerPrefs.SetFloat("health", health);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("lives", lives);
        PlayerPrefs.SetFloat("xp", xp);
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
        } else {
            timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
            timeSinceLastOpenedGame = System.DateTime.Now.Second - timeGameWasLastOpened;
            if(timeSinceLastOpenedGame > (healthRegenInterval * maxHealth)) {
                health = maxHealth;
            } else {
                // TODO: Calculate health
                health = maxHealth;
            }
        }
        UpdateHealthBar();
    }

    void InitLives() {
        if(isFirstStartup) {
            lives = maxLives;
        } else {
            lives = PlayerPrefs.GetInt("lives");
            timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
            timeSinceLastOpenedGame = System.DateTime.Now.Second - timeGameWasLastOpened;
            if(timeSinceLastOpenedGame > (lifeRegenInterval * maxLives)) {
                lives = maxLives;
            } else {
                // Calculate how many life regen intervals have passed as a single integer
                int index = (int)(timeGameWasLastOpened / lifeRegenInterval);
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
            }
        }
        UpdateLivesText();
    }

    void InitXP() {
        if(isFirstStartup) {
            xp = 0;
        } else {
            xp = PlayerPrefs.GetFloat("xp");
        }
        UpdateXPText();
    }

    void InitLevel() {
        if(isFirstStartup) {
            level = 1;
        } else {
            level = PlayerPrefs.GetInt("level");
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
                lives += 1;
                UpdateLivesText();
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
        livesText.text = "Lives: " + lives.ToString();
	}

    void UpdateLevelText() {
        levelText.text = "Level: " + level.ToString();
    }

    void UpdateXPText() {
        xpText.text = "XP: " + xp.ToString();
    }

    public void AddXP(float value) {
        xp += value;
        UpdateXPText();
        if(xp >= nextLevelUpAmount) {
            level++;
            float newLevelUpAmount = nextLevelUpAmount + (lastLevelUpAmount * 0.2f);
            lastLevelUpAmount = nextLevelUpAmount;
            nextLevelUpAmount = newLevelUpAmount;
            UpdateLevelText();
        }
    }
}
