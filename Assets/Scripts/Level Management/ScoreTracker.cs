using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreTracker : MonoBehaviour {
	public int levelScore;
	public float time;
	public float score;
	public int worldNumber;
	public int levelNumber;
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
	public GameManager gameManager;
	public Player player;
	//public LoadLevel level;
	//public LevelManager levelMan;
	public LevelManager levelManager;
	public Scene scene;
	private AudioManager audioManager;
	
	void Awake(){
		//GameObject levelMan = GameObject.Find("LevelManager");
		//levelManager = levelMan.GetComponent<LevelManager>();
		//GameObject LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>().gameObject;
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
			// print("gm is started " + gameManager.isGameStarted);
			// print("gm is game over " + gameManager.isGameOver);
			// print("gm is level over " + gameManager.isLevelOver);
		if (gameManager.isGameStarted && !gameManager.isGameOver && !gameManager.isLevelComplete){
			if (time <= 0){
				time = 0;
				//gameManager.isLevelOver = true;
				gameManager.isGameOver = true;
			}
			else {
				time -= Time.deltaTime;
			}
			timeText.text = "" + GetTime();
			livesText.text = "x " + player.lives;
		}
		else if (gameManager.isLevelComplete){
			timeText.text = "" + GetTime();
			livesText.text = "" + player.lives;
			score = GetTime() * player.lives;
			//SetLevelScore();
			LevelComplete();
		}
		else if (gameManager.isGameOver){
			loseTimeText.text = "" + GetTime();
			loseLivesText.text = "" + player.lives;
			//score = GetTime() * player.lives;
			GameOver();
		}
	}

	public int GetScore(){
		return Mathf.FloorToInt(score);
	}

	public int GetTime(){
		return Mathf.FloorToInt(time);
	}

	// public int SetLevelScore(){
	// 	// PlayerPrefs.SetInt("score", GetScore());
	// 	// return levelScore;
	// 	//return Mathf.FloorToInt()
	// }

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
	}

	void GameOver(){
		Time.timeScale = 0;
		audioManager.StopSFX();
		gameOverScreen.SetActive(true);
		pauseButton.SetActive(false);
		CalculateScore();
	}

	void CalculateScore(){
		levelLivesText.text = "" + livesText.text;
		levelTimeText.text = "" + timeText.text;
		levelScoreText.text = "" + score;
	}
}