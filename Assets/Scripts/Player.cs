﻿using UnityEngine;
using System;

public class Player : MonoBehaviour {
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
	public AudioSource audio;
	public Rigidbody2D rb2d;
	public PlayerMovement action;
	public Vector3 pos;
	public Vector3 facing;
	private float slideTimer = 0;
	
    void Start(){
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb2d = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
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
				if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isSliding){
					action.Jump();
				}
				if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding){
					action.Special();
				}
				if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
					GetComponent<Animator>().SetBool("isRunning", false);
				}
			}
		}
	}

	public void ResetSlide(){
		GetComponent<Animator>().SetBool("isSliding", false);
		GetComponent<CircleCollider2D>().enabled = true;
		slideTimer = 0;
		rb2d.velocity = new Vector2(0, 0);
		isSliding = false;
	}

	public void Die(){
		bloodParticle.SetActive(true);
		isDead = true;
		audio.clip = sfxDie;
		audio.Play();
		GetComponent<Animator>().SetBool("isDead", true);
		if (lives == 1){
			lives = 0;
			bloodParticle.SetActive(false);
			//GetComponent<SpriteRenderer>().sortingLayerName = "Hidden";
			gameManager.isGameOver = true;
		}
		else{
			lives--;
		}
	}

	void Respawn(){
		if (lives != 0){
			bloodParticle.SetActive(false);
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
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Trap" && !isDead){
			Die();
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "FallingSpikes" && !isDead){
			Die();
		}
		if (other.gameObject.tag == "Exit" && !isDead && exitDoor.m_activate){
			// Hides player behind the exit door and stops time.
			GetComponent<SpriteRenderer>().sortingLayerName = "Hidden";
			Time.timeScale = 0;
			gameManager.isLevelComplete = true;
		}
	}
}