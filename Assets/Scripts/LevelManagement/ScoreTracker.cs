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
	public float maxTime = 120f;
	public float score;
	private int coinScoreValue = 25;
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
		}
		else if (levelNumber >= 91 && levelNumber <= 135){
			worldNumber = 3;
		}
		levelNumberText.text = worldNumber + "-" + levelNumber;
		audioManager = AudioManager.Instance;
		time = maxTime;
		triggerOnce = false;
	}

	void Update () {
		if (gameManager.isGameStarted && !gameManager.isGameOver && !gameManager.isLevelComplete){
			if (time <= 0){
				time = 0;
				gameManager.isGameOver = true;
			}
			else {
				time -= Time.deltaTime;
			}
			livesText.text = "x " + player.lives;
			if (time >= 0){
				timeText.text = "" + GetTime();
				//TODO: Update score in live time to display on the score bar.
				// scoreText.text = "" + GetScore();
			}
		}
		else if (gameManager.isLevelComplete && !triggerOnce){
			int coinScore = gameManager.tempCoinCounter * coinScoreValue;
			//gameManager.coinCounter += (gameManager.tempCoinCounter * Int32.Parse(scene.name));
			gameManager.coinCounter += (gameManager.tempCoinCounter);
			gameManager.tempCoinCounter = 0;
			int tempPlayerCoins = PlayerPrefs.GetInt("Coins");
			PlayerPrefs.SetInt("Coins", tempPlayerCoins += gameManager.coinCounter);
			timeText.text = "" + GetTime();
			livesText.text = "" + player.lives;
			score = (GetTime() * player.lives) + coinScore;
			LevelComplete();
		}
		else if (gameManager.isGameOver && !triggerOnce){
			int coinScore = gameManager.tempCoinCounter * coinScoreValue;
			gameManager.tempCoinCounter = 0;
			timeText.text = "" + GetTime();
			livesText.text = "" + player.lives;
			score = coinScore;
			GameOver();
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

		if (score > 0){
			stars[0].SetActive(true);
		}
		if (score >= 450){
			stars[1].SetActive(true);
		}
		if (score >= 900){
			stars[2].SetActive(true);
		}
		nextLevelButton.SetActive(true);
		CalculateScore();
		levelManager.UnlockLevel(levelNumber);
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		triggerOnce = true;
	}

	void GameOver(){
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
		levelLivesText.text = "" + livesText.text;
		levelTimeText.text = "" + timeText.text;
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