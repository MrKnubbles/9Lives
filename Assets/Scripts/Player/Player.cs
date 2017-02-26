using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject headIdle;
	public GameObject headJump;
	public GameObject headDie;	
	private GameObject masksContainer;
	[SerializeField]
	private List<GameObject> masks;
	public GameObject bloodSplatPrefab;
	private Quaternion bloodRot = new Quaternion();
	public GameObject respawnPos;
	public GameObject bloodParticle;
	public Door exitDoor;
	public bool isDead = false;
	public bool isJumping = false;
	public bool isFalling = false;
	public bool isSliding = false;
	public bool isFacingRight = true;
	public bool hasDoubleJumped = false;
    public float jumpSpeed = 15.0f;
	public float moveSpeed = 1.5f;
	public float lives = 9;
	public GameManager gameManager;
	public bool isGrounded = true;
	public AudioClip sfxDie;
	public AudioClip sfxDoubleJump;
	public AudioClip sfxHit;
	public AudioClip sfxJump;
	public AudioClip sfxSlam;
	// public AudioSource audio;
	public AudioManager audioManager;
	public Rigidbody2D rb2d;
	public PlayerMovement action;
	public Vector3 pos;
	public Vector3 facing;
	private float slideTimer = 0;
	// Needed for activating a switch.
	public bool isNearSwitch = false;
	public bool isActivatingSwitch = false;
	public GameObject HUD;
	
    void Start(){
		headIdle.SetActive(true);
		headDie.SetActive(false);
		headJump.SetActive(false);		
		masksContainer = GameObject.Find("Masks");
		foreach(RectTransform g in masksContainer.transform) {
			masks.Add(g.gameObject);
		}
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb2d = GetComponent<Rigidbody2D>();
		HUD = GameObject.Find("HUD");
		audioManager = AudioManager.Instance;
		gameManager.isGameStarted = true;
		gameManager.isLevelComplete = false;
		gameManager.isGameOver = false;
		GetComponent<Animator>().SetBool("isGameStarted", true);
		gameManager.isLevelComplete = false;
		exitDoor = GameObject.Find("ExitDoor/Door").GetComponent<Door>();
		respawnPos = GameObject.Find("Respawn");
	} 
	void Update(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			HandleHeads();
			if (!isDead){
				if (isSliding){
					slideTimer += Time.deltaTime;
					if (slideTimer >= .75f){
						ResetSlide();
					}
				}
				if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
					action.MoveRight();
				}
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
					action.MoveLeft();
				}
				if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))){
					action.Jump();
				}
				if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding){
					action.Special();
				}
				if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
					GetComponent<Animator>().SetBool("isRunning", false);
				}
				if (rb2d.velocity.y < 0 && !isFalling){
					SetFalling();
				}
			}
		}
		if ((Input.GetKeyUp(KeyCode.L) && gameManager.isLevelComplete)){
			HUD.GetComponent<LoadLevel>().ReplayLevel();
		}
		if ((Input.GetKeyUp(KeyCode.R) && gameManager.isLevelComplete)){
			HUD.GetComponent<LoadLevel>().LoadNextLevel();
		}
	}

	public void ResetSlide(){
		GetComponent<Animator>().SetBool("isSliding", false);
		GetComponent<CircleCollider2D>().enabled = true;
		slideTimer = 0;
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		isSliding = false;
	}

	public void Die(){
		SpawnBlood();
		isDead = true;
		audioManager.PlayOnce(sfxDie);
		GetComponent<Animator>().SetBool("isDead", true);
		isActivatingSwitch = false;
		if (lives == 1){
			lives = 0;
			gameManager.isGameOver = true;
		}
		else{
			lives--;
		}
	}

	void Respawn(){
		if (lives != 0){
			isDead = false;
			isJumping = false;
			hasDoubleJumped = false;
			isFalling = false;
			isSliding = false;
			isGrounded = true;
			GetComponent<Animator>().SetBool("isJumping", false);
			GetComponent<Animator>().SetBool("isFalling", false);
			GetComponent<Animator>().SetBool("isSliding", false);
			GetComponent<Animator>().SetBool("isRunning", false);
			GetComponent<Animator>().SetBool("isDead", false);
			transform.position = respawnPos.transform.position;
			transform.localScale = respawnPos.transform.localScale;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Trap" && !isDead){
			Die();
		}
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = other.transform;
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = null;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if ((other.gameObject.tag == "FallingSpikes" || other.gameObject.tag == "DrippingPipe") && !isDead){
			Die();
		}
		if (other.gameObject.tag == "Exit" && !isDead && exitDoor.isActive){
			// Hides player behind the exit door and stops time.
			GetComponent<SpriteRenderer>().sortingLayerName = "Hidden";
			Time.timeScale = 0;
			gameManager.isLevelComplete = true;
		}
	}

	void SpawnBlood() {
		//Vector3 offsetY = new Vector3(0, -0.5f, 0);
		Vector3 spawnPosition = this.transform.position;
		GameObject tmpBlood;
		tmpBlood = GameObject.Instantiate(bloodParticle, spawnPosition, Quaternion.identity);
		foreach(GameObject g in masks) {
			//offsetY = new Vector3(0, -1f, 0);
			GameObject tmpBloodSplat;
			tmpBloodSplat = Instantiate(bloodSplatPrefab, g.transform, false);
			tmpBloodSplat.transform.position = this.transform.position;
		}

		int random = Random.Range(0, 360);
		bloodRot = Quaternion.Euler(0, 0, random);
		bloodSplatPrefab.transform.rotation = bloodRot;
	}

	public void SetGrounded(){
		GetComponent<Animator>().SetBool("isJumping", false);
		GetComponent<Animator>().SetBool("isFalling", false);
		isGrounded = true;
		isJumping = false;
		isFalling = false;
		hasDoubleJumped = false;
	}

	public void SetFalling(){
		GetComponent<Animator>().SetBool("isJumping", false);
		GetComponent<Animator>().SetBool("isSliding", false);
		GetComponent<Animator>().SetBool("isFalling", true);
		isGrounded = false;
		isJumping = false;
		isFalling = true;
	}

	private void HandleHeads() {
		if(isJumping || isFalling || isSliding) {
			headJump.SetActive(true);			
			headDie.SetActive(false);
			headIdle.SetActive(false);	
		} else if(isDead) {		
			headDie.SetActive(true);
			headJump.SetActive(false);	
			headIdle.SetActive(false);	
		} else {
			headIdle.SetActive(true);
			headJump.SetActive(false);	
			headDie.SetActive(false);			
		}
	}
}