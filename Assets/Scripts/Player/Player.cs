﻿using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class Player : MonoBehaviour {
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
	
    void Start(){
		masksContainer = GameObject.Find("Masks");
		foreach(RectTransform g in masksContainer.transform) {
			masks.Add(g.gameObject);
		}
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb2d = GetComponent<Rigidbody2D>();
		// audio = GetComponent<AudioSource>();
		audioManager = AudioManager.Instance;
		gameManager.isGameStarted = true;
		GetComponent<Animator>().SetBool("isGameStarted", true);
		gameManager.isLevelComplete = false;
		exitDoor = GameObject.Find("ExitDoor/Door").GetComponent<Door>();
		respawnPos = GameObject.Find("Respawn");
	} 
	void Update(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
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

		int random = Random.Range(0, 4);
		switch(random) {
			case 0:
				bloodRot = Quaternion.Euler(0, 0, 0);
				bloodSplatPrefab.transform.rotation = bloodRot;
			break;

			case 1:
				bloodRot = Quaternion.Euler(0, 0, 90);
				bloodSplatPrefab.transform.rotation = bloodRot;
			break;

			case 2:
				bloodRot = Quaternion.Euler(0, 0, 180);
				bloodSplatPrefab.transform.rotation = bloodRot;
			break;

			case 3:
				bloodRot = Quaternion.Euler(0, 0, 270);
				bloodSplatPrefab.transform.rotation = bloodRot;
			break;

			default:
			break;
		}
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
}