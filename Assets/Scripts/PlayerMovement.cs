using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public GameManager gameManager;
	public Player player;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	public void Jump(){
		if (!player.isDead && !player.isSliding){
			if (!player.hasDoubleJumped){
				if (!player.isJumping && player.isGrounded){
					player.audio.clip = player.sfxJump;
					player.audio.Play();
					player.rb2d.AddForce(transform.up * player.jumpSpeed);
					player.GetComponent<Animator>().SetBool("isJumping", true);
					player.isJumping = true;
					player.isGrounded = false;
				}
				else if (player.isJumping && !player.isGrounded){
					player.audio.clip = player.sfxDoubleJump;
					player.audio.Play();
					player.rb2d.velocity = new Vector2(0, 0);
					player.rb2d.AddForce(transform.up * player.jumpSpeed / 1.25f);
					player.hasDoubleJumped = true;
				}
			}
		}
	}

	// public void DoubleJump(){
	// 	player.audio.clip = player.sfxDoubleJump;
	// 	player.audio.Play();
	// 	player.rb2d.velocity = new Vector2(0, 0);
	// 	player.rb2d.AddForce(transform.up * player.jumpSpeed / 1.25f);
	// 	player.hasDoubleJumped = true;
	// }

	public void Special(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			if (!player.isDead){
				if (player.isGrounded){
					Slide();
				}
				else{
					Slam();
				}
			}
		}
	}

	private void Slam(){
		player.audio.clip = player.sfxSlam;
		player.audio.Play();
		player.rb2d.AddForce(transform.up * -player.jumpSpeed * 2);
		player.GetComponent<Animator>().SetBool("isFalling", true);
		player.isFalling = true;
	}

	private void Slide(){
		if (!player.isSliding){
			player.GetComponent<Animator>().SetBool("isSliding", true);
			player.GetComponent<CircleCollider2D>().enabled = false;
			if (player.isFacingRight){
				player.rb2d.AddForce(transform.right * player.jumpSpeed);
			}
			else{
				player.rb2d.AddForce(transform.right * -player.jumpSpeed);
			}
			player.isSliding = true;
		}
	}

	public void StopRunning(){
		player.GetComponent<Animator>().SetBool("isRunning", false);
	}

	public void MoveLeft(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			if (!player.isDead){
				if (player.isSliding && player.isFacingRight){
					player.rb2d.velocity *= -1;
				}
				else if (!player.isSliding){
					player.GetComponent<Animator>().SetBool("isRunning", true);
					player.pos = player.transform.position;
					player.facing = player.transform.localScale;
					player.pos.x -= player.moveSpeed * Time.deltaTime;
					player.transform.position = player.pos;
				}
				if (player.facing.x > 0){
					player.facing.x *= -1;
					player.transform.localScale = player.facing;
				}
				player.isFacingRight = false;
			}
		}
	}

	public void MoveRight(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			if (!player.isDead){
				if (player.isSliding && !player.isFacingRight){
					player.rb2d.velocity *= -1;
				}
				else if (!player.isSliding){
					player.GetComponent<Animator>().SetBool("isRunning", true);
					player.pos = player.transform.position;
					player.facing = player.transform.localScale;
					player.pos.x += player.moveSpeed * Time.deltaTime;
					player.transform.position = player.pos;
				}
				if (player.facing.x < 0){
					player.facing.x *= -1;
					player.transform.localScale = player.facing;
				}
				player.isFacingRight = true;
			}
		}
	}

	public void Use(){

	}
}
