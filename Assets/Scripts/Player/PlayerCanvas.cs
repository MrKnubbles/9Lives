using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

	float health;
    public GameManager gameManager;
	[SerializeField] float maxHealth = 5;
	[SerializeField] Image healthBarForeground;
	float healthRegenInterval = 5f;
	float lastHealthRegenTime;
	float currentHealthRegenTime = 5f;
    float timeSinceLastOpenedGame;
    float timeGameWasLastOpened;

    public float GetHealth() { return health; }
    public void SetHealth(float value) { health = value; }

    void Awake() {
        DontDestroyOnLoad(this);


        timeGameWasLastOpened = PlayerPrefs.GetFloat("LastExitTime");
        timeSinceLastOpenedGame = System.DateTime.Now.Second - timeGameWasLastOpened;
        if(timeSinceLastOpenedGame > healthRegenInterval * maxHealth) {
            health = maxHealth;
            UpdateHealthBar();
        } else {
            // TODO: Calculate health
            health = maxHealth;
        }       
    }

	void Start () {	
		UpdateHealthBar();
	}
	
	void Update () {
        UpdateHealthRegeneration();
        float derp = TimeSinceLastHealthRegen(System.DateTime.Now.Second);
		Debug.Log(derp);
        //float ddd = System.DateTime.Now.Second;
		//Debug.Log(ddd);
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetFloat("LastExitTime", (float)System.DateTime.Now.Second);
	}

    public void Die() {
        health = 0;
        UpdateHealthBar();
    }

    public float TimeSinceLastHealthRegen(float dateTime) {
         return (dateTime - lastHealthRegenTime);
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

    public void Respawn() {        
        health = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage) {        			
        health -= damage;
        UpdateHealthBar();
	}

	void UpdateHealthBar() {
		float currentFillAmount =  health / maxHealth;
		healthBarForeground.GetComponent<Image>().fillAmount = currentFillAmount;
	}
}
