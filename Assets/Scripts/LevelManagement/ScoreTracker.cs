using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreTracker : MonoBehaviour {
	//TODO: Use less variables to display score information.
	public int levelScore;
	public float time;
	public float maxTime = 120f;
	public float score;
	private int worldNumber;
	private int levelNumber;
	public Text livesText;
	public Text timeText;
	public Text levelNumberText;
	public Text levelLivesText;
	public Text levelTimeText;
	public Text levelScoreText;
	public Text loseLivesText;
	public Text loseTimeText;
	public Text loseScoreText;
	public GameObject star1;
	public GameObject star2;
	public GameObject star3;
	public GameObject levelCompleteScreen;
	public GameObject gameOverScreen;
	public GameObject pauseButton;
	private GameManager gameManager;
	private Player player;
	private LevelManager levelManager;
	private Scene scene;
	private AudioManager audioManager;
	private bool triggerOnce = false;
	private int coinScoreValue = 25;
	
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
		levelCompleteScreen.SetActive(true);
		pauseButton.SetActive(false);
		Time.timeScale = 0;
		audioManager.StopSFX();

		if (score > 0){
			star1.SetActive(true);
		}
		if (score >= 400){
			star2.SetActive(true);
		}
		if (score >= 800){
			star3.SetActive(true);
		}
		CalculateScore();
		levelManager.UnlockLevel(levelNumber);
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		triggerOnce = true;
	}

	void GameOver(){
		Time.timeScale = 0;
		audioManager.StopSFX();
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
		loseLivesText.text = "" + livesText.text;
		loseTimeText.text = "" + timeText.text;
		loseScoreText.text = "" + score;
	}
}