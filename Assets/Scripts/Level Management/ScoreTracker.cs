using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreTracker : MonoBehaviour {
	public int levelScore;
	public float time;
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
		if (levelNumber >= 1 && levelNumber <= 45){
			worldNumber = 1;
		}
		else if (levelNumber >= 46 && levelNumber <= 90){
			worldNumber = 2;
		}
		levelNumberText.text = worldNumber + "-" + levelNumber;
		audioManager = AudioManager.Instance;
	}

	// Update is called once per frame
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
			}
		}
		else if (gameManager.isLevelComplete){
			timeText.text = "" + GetTime();
			livesText.text = "" + player.lives;
			score = GetTime() * player.lives;
			LevelComplete();
		}
		else if (gameManager.isGameOver){
			loseTimeText.text = "" + GetTime();
			loseLivesText.text = "" + player.lives;
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
		if (score >= 350){
			star2.SetActive(true);
		}
		if (score >= 650){
			star3.SetActive(true);
		}
		CalculateScore();
		levelManager.UnlockLevel(levelNumber);
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}

	void GameOver(){
		Time.timeScale = 0;
		audioManager.StopSFX();
		gameOverScreen.SetActive(true);
		pauseButton.SetActive(false);
		CalculateScore();
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}

	void CalculateScore(){
		levelLivesText.text = "" + livesText.text;
		levelTimeText.text = "" + timeText.text;
		levelScoreText.text = "" + score;
	}
}