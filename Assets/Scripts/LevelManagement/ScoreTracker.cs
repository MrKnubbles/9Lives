﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreTracker : MonoBehaviour {
	// Managers
	private GameManager gameManager;
	private LevelManager levelManager;
	private AudioManager audioManager;
	// HUD info
	public float time;
	public float maxTime = 60f;
	public float score;
	private int coinExpValue = 1;
	private int worldNumber;
	private int levelNumber;
	public Text levelNumberText;
	public GameObject pauseButton;
	// Level Over info
	public Text livesText;
	public Text timeText;
	public Text levelLivesText;
	public Text levelTimeText;
	public Text levelScoreText;
	public GameObject levelOverScreen;
	public GameObject levelCompleteScreen;
	public GameObject gameOverScreen;
	public GameObject skipLevelButton;
	public GameObject skipLevelConfirmation;
	public GameObject skipLevelText;
	public GameObject nextLevelButton;
	public GameObject[] stars = new GameObject[3];
	// Other
	private Scene scene;
	private Player player;
	private bool triggerOnce = false;
	
	void Awake(){
		levelManager = LevelManager.Instance;
		scene = SceneManager.GetActiveScene();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void Start(){
		levelManager = LevelManager.Instance;
		scene = SceneManager.GetActiveScene();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		player = GameObject.Find("Player").GetComponent<Player>();
		levelNumber = Convert.ToInt32(SceneManager.GetActiveScene().name);
		//TODO: Create actual world numbers to track.
		if (levelNumber >= 1 && levelNumber <= 45){
			worldNumber = 1;
		}
		else if (levelNumber >= 46 && levelNumber <= 90){
			worldNumber = 2;
			levelNumber -= 45;
		}
		else if (levelNumber >= 91 && levelNumber <= 135){
			worldNumber = 3;
			levelNumber -= 90;
		}
		// Set coin exp value based on which world you're in.
		int tempCoinExpValue = coinExpValue * worldNumber;
		coinExpValue = tempCoinExpValue;
		
		levelNumberText.text = worldNumber + " - " + levelNumber;
		audioManager = AudioManager.Instance;
		time = 0;
		triggerOnce = false;
	}

	void Update () {
		// if player is alive and has not completed the level
		if (gameManager.isGameStarted && !gameManager.isLevelOver && !gameManager.isLevelComplete){
			if(time <= maxTime) {
				time += Time.deltaTime;
				timeText.text = "" + GetTime();
				//TODO: Update score in live time to display on the score bar.
				// scoreText.text = "" + GetScore();
			} else {
				timeText.text = "∞";
				time = maxTime + 1;
			}
		}
		// if player completes the level
		else if (gameManager.isLevelComplete && !triggerOnce){
			int coinScore = gameManager.tempCoinCounter * coinExpValue;
			gameManager.coinCounter += gameManager.tempCoinCounter;
			gameManager.tempCoinCounter = 0;
			int tempPlayerCoins = PlayerPrefs.GetInt("Coins");
			PlayerPrefs.SetInt("Coins", tempPlayerCoins += gameManager.coinCounter);
			if(time <= maxTime) {
				timeText.text = "" + GetTime();
				score = Mathf.Round((((maxTime - GetTime()) / maxTime) * 1500) - ((player.damageTaken / player.GetPlayerCanvas().GetMaxHealth()) * 100)) + (coinScore * 10);
			} else {
				timeText.text = "∞";
				score = Mathf.Round(((1 / maxTime) * 1500) - ((player.damageTaken / player.GetPlayerCanvas().GetMaxHealth()) * 100)) + (coinScore * 10);
			}
			levelLivesText.text = "" + player.GetPlayerCanvas().GetLives();
			player.GetPlayerCanvas().AddXP(coinScore);
			LevelComplete();
		}
		// if player dies
		else if (gameManager.isLevelOver && !triggerOnce){
			int coinScore = gameManager.tempCoinCounter * coinExpValue;
			gameManager.tempCoinCounter = 0;
			if(time <= maxTime) {
				timeText.text = "" + GetTime();
			} else {
				timeText.text = "∞";
			}
			levelLivesText.text = "" + player.GetPlayerCanvas().GetLives();
			score = coinScore;
			player.GetPlayerCanvas().AddXP(coinScore);
			LevelOver();
		}
	}

	public int GetScore(){
		return Mathf.FloorToInt(score);
	}

	public int GetTime(){
		return Mathf.FloorToInt(time);
	}

	void LevelComplete(){
		levelOverScreen.SetActive(true);
		levelCompleteScreen.SetActive(true);
		pauseButton.SetActive(false);
		Time.timeScale = 0;
		audioManager.StopSFX();
		// Add exp and gold based on player level.
		int tempPlayerCoins = PlayerPrefs.GetInt("Coins");
		player.GetPlayerCanvas().AddXP(PlayerPrefs.GetInt("PlayerLevel"));
		PlayerPrefs.SetInt("Coins", tempPlayerCoins += PlayerPrefs.GetInt("PlayerLevel"));

		if (score > 0){
			stars[0].SetActive(true);
			// If the player beats the level for the first time, award bonus exp and gold.
			if (!PlayerPrefs.HasKey("Level" + float.Parse(SceneManager.GetActiveScene().name) + "Score")){
				tempPlayerCoins = PlayerPrefs.GetInt("Coins");
				player.GetPlayerCanvas().AddXP(PlayerPrefs.GetInt("PlayerLevel"));
				PlayerPrefs.SetInt("Coins", tempPlayerCoins += PlayerPrefs.GetInt("PlayerLevel"));
			}
		}
		if (score >= 500){
			stars[1].SetActive(true);
			// If the player beats the level for the first time, award bonus exp and gold.
			if (!PlayerPrefs.HasKey("Level" + float.Parse(SceneManager.GetActiveScene().name) + "Score")){
				tempPlayerCoins = PlayerPrefs.GetInt("Coins");
				player.GetPlayerCanvas().AddXP((PlayerPrefs.GetInt("PlayerLevel")) * 2);
				PlayerPrefs.SetInt("Coins", tempPlayerCoins += (PlayerPrefs.GetInt("PlayerLevel") * 2));
			}
			// If the player previously beat the level with less than 2 stars, award bonus exp and gold for obtaining a second star.
			else if (PlayerPrefs.GetInt("Level" + float.Parse(SceneManager.GetActiveScene().name) + "Score") < 450){
				tempPlayerCoins = PlayerPrefs.GetInt("Coins");
				player.GetPlayerCanvas().AddXP((PlayerPrefs.GetInt("PlayerLevel")) * 2);
				PlayerPrefs.SetInt("Coins", tempPlayerCoins += (PlayerPrefs.GetInt("PlayerLevel") * 2));
			}
		}
		if (score >= 1000){
			stars[2].SetActive(true);
			// If the player beats the level for the first time, award bonus exp and gold.
			if (!PlayerPrefs.HasKey("Level" + float.Parse(SceneManager.GetActiveScene().name) + "Score")){
				tempPlayerCoins = PlayerPrefs.GetInt("Coins");
				player.GetPlayerCanvas().AddXP((PlayerPrefs.GetInt("PlayerLevel")) * 3);
				PlayerPrefs.SetInt("Coins", tempPlayerCoins += (PlayerPrefs.GetInt("PlayerLevel") * 3));
			}
			// If the player previously beat the level with less than 3 stars, award bonus exp and gold for obtaining a third star.
			else if (PlayerPrefs.GetInt("Level" + float.Parse(SceneManager.GetActiveScene().name) + "Score") < 900){
				tempPlayerCoins = PlayerPrefs.GetInt("Coins");
				player.GetPlayerCanvas().AddXP((PlayerPrefs.GetInt("PlayerLevel")) * 3);
				PlayerPrefs.SetInt("Coins", tempPlayerCoins += (PlayerPrefs.GetInt("PlayerLevel") * 3));
			}
		}

		nextLevelButton.SetActive(true);
		CalculateScore();
		levelManager.UnlockLevel(levelNumber);
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		triggerOnce = true;
	}

	void LevelOver(){
		CheckIfLevelCanBeSkipped();
		Time.timeScale = 0;
		audioManager.StopSFX();
		levelOverScreen.SetActive(true);
		gameOverScreen.SetActive(true);
		pauseButton.SetActive(false);
		CalculateScore();
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		triggerOnce = true;
	}

	void CalculateScore(){
		levelLivesText.text = "" + player.GetPlayerCanvas().GetLives();
		levelTimeText.text = "" + GetTime();
		levelScoreText.text = "" + score;
	}

	private void CheckIfLevelCanBeSkipped(){
		// If Player has Skips, current level is the latest unlocked level and current level isn't the last level in a world, then show SkipLevelButton.
		if (PlayerPrefs.GetInt("Skip") > 0){
			if (PlayerPrefs.GetInt("World1PlayerLevel") <= Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) != 45){
				skipLevelButton.SetActive(true);
			}
			else if (PlayerPrefs.GetInt("World2PlayerLevel") <= Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) > 45 && Int32.Parse(SceneManager.GetActiveScene().name) != 90){
				skipLevelButton.SetActive(true);
			}
			else if (PlayerPrefs.GetInt("World3PlayerLevel") <= Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) > 90 && Int32.Parse(SceneManager.GetActiveScene().name) != 135){
				skipLevelButton.SetActive(true);
			}
		}
		
		// If Player has previously beaten the current level, then show NextLevelButton.
		if (PlayerPrefs.GetInt("World1PlayerLevel") > Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) <= 45){
			nextLevelButton.SetActive(true);
		}
		else if (PlayerPrefs.GetInt("World2PlayerLevel") > Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) > 45 && Int32.Parse(SceneManager.GetActiveScene().name) <= 90){
			nextLevelButton.SetActive(true);
		}
		else if (PlayerPrefs.GetInt("World3PlayerLevel") > Int32.Parse(SceneManager.GetActiveScene().name) && Int32.Parse(SceneManager.GetActiveScene().name) > 90 && Int32.Parse(SceneManager.GetActiveScene().name) <= 135){
			nextLevelButton.SetActive(true);
		}
	}
}