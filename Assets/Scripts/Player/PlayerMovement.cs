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
		if (!player.isDead && !player.isStunned){
			if (!player.hasDoubleJumped){
				player.GetComponent<Animator>().SetBool("isJumping", true);
				player.GetComponent<Animator>().SetBool("isFalling", false);
				if (player.isSliding){
					player.ResetSlide();
				}
				if (!player.isJumping && !player.isFalling && player.isGrounded){
					player.audioManager.PlayOnce(player.sfxJump);
					player.rb2d.AddForce(transform.up * player.jumpSpeed);
					player.isFalling = false;
					player.isJumping = true;
					player.isGrounded = false;
				}
				// Double jump
				else if ((player.isJumping || player.isFalling) && !player.isGrounded){
					player.audioManager.PlayOnce(player.sfxDoubleJump);
					player.rb2d.velocity = new Vector2(0, 0);
					player.rb2d.AddForce(transform.up * player.jumpSpeed / 1.15f);
					player.hasDoubleJumped = true;
					player.isFalling = false;
				}
			}
		}
	}

	public void Special(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			if (!player.isDead && !player.isStunned){
				if (player.isNearSwitch && !player.isActivatingSwitch){
					ActivateSwitch();
				}
				else if (player.isGrounded){
					Slide();
				}
				else{
					Slam();
				}
			}
		}
	}

	private void ActivateSwitch(){
		if (!player.isStunned){
			player.isActivatingSwitch = true;
		}
	}

	private void Slam(){
		player.audioManager.PlayOnce(player.sfxSlam);
		// player.audio.clip = player.sfxSlam;
		// player.audio.Play();
		player.rb2d.AddForce(transform.up * -player.jumpSpeed * 2);
		player.GetComponent<Animator>().SetBool("isFalling", true);
		player.isFalling = true;
	}

	private void Slide(){
		if (!player.isSliding && !player.isFalling){
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
		if(!player.isHit) {
			player.GetComponent<Animator>().SetBool("isRunning", false);
		}
	}

	public void MoveLeft(){
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			if (!player.isDead && !player.isStunned){
				if (player.isSliding && player.isFacingRight){
					player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, 0);
					player.rb2d.velocity *= -1;
				}
				else if (!player.isSliding){
					if(!player.isHit) {
						player.GetComponent<Animator>().SetBool("isRunning", true);
					}
					player.pos = player.transform.localPosition;
					player.facing = player.transform.localScale;
					player.pos.x -= player.moveSpeed * Time.deltaTime;
					player.transform.localPosition = player.pos;
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
			if (!player.isDead && !player.isStunned){
				if (player.isSliding && !player.isFacingRight){
					player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, 0);
					player.rb2d.velocity *= -1;
				}
				else if (!player.isSliding){
					if(!player.isHit) {
						player.GetComponent<Animator>().SetBool("isRunning", true);
					}
					player.pos = player.transform.localPosition;
					player.facing = player.transform.localScale;
					player.pos.x += player.moveSpeed * Time.deltaTime;
					player.transform.localPosition = player.pos;
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
