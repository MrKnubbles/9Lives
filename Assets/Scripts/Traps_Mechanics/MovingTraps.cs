using UnityEngine;
using System;

public class MovingTraps : MonoBehaviour {

	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private MoveObject moveObject;
	private Player player;
	private bool spikePress;
	private Vector3 startPosition;
	private Vector3 endPosition;
	public float damage;
	private Vector3 direction;
	private float knockbackVelocity = 3f;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		if (gameObject.name == "SpikePress"){
			spikePress = true;
			startPosition = transform.localPosition;
			endPosition.y = startPosition.y + moveVertical;
			endPosition.x = startPosition.x + moveHorizontal;
		}
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}
	
	// LateUpdate keeps multiple SpikePresses in sync.
	void LateUpdate () {
		// If object reaches it's destination...
		if (moveObject.isDoneMoving()){
			if (moveHorizontal != 0){
				if (spikePress){
					SpikePressHorizontal();
				}
				else{
					HorizontalMovement();
				}
			}
			if (moveVertical != 0){
				if (spikePress){
					SpikePressVertical();
				}
				else{
					VerticalMovement();
				}
			}
		}
	}

	void HorizontalMovement(){
		// Reverse direction and move.
		moveHorizontal = -moveHorizontal;
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.Move();
	}
	void VerticalMovement(){
		// Reverse direction and move.
		moveVertical = -moveVertical;
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}

	void SpikePressVertical(){
		if (transform.localPosition == startPosition){
			moveObject.SetSpeed(moveSpeed);
		}
		moveVertical = -moveVertical;
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
		if (transform.localPosition.y == endPosition.y){
			moveObject.SetSpeed(moveSpeed / 5);
		}
	}

	void SpikePressHorizontal(){
		if (transform.localPosition == startPosition){
			moveObject.SetSpeed(moveSpeed);
		}
		moveHorizontal = -moveHorizontal;
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.Move();
		if (transform.localPosition.x == endPosition.x){
			moveObject.SetSpeed(moveSpeed / 5);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead){
			GetComponent<Collider2D>().enabled = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Player") {
			if (player.isDead){
				GetComponent<Collider2D>().enabled = false;
			}
			else{
				// When the player hits the left of trap...
				// get knocked away and face towards trap.
				if (other.transform.position.x <= transform.position.x){
					if (player.facing.x < 0){
						player.facing.x *= -1;
						player.transform.localScale = player.facing;
						player.isFacingRight = true;
					}
					player.rb2d.velocity = new Vector2(0, 0);
					player.rb2d.AddForce(transform.up * player.knockbackSpeed * 2f);
					player.rb2d.AddForce(transform.right * -player.knockbackSpeed);
					player.isStunned = true;
				}
				// When the player hits the right of trap...
				// get knocked away and face towards trap.
				else if (other.transform.position.x > transform.position.x){
					if (player.facing.x > 0){
						player.facing.x *= -1;
						player.transform.localScale = player.facing;
						player.isFacingRight = false;
					}
					player.rb2d.velocity = new Vector2(0, 0);
					player.rb2d.AddForce(transform.up * player.knockbackSpeed * 2f);
					player.rb2d.AddForce(transform.right * player.knockbackSpeed);
					player.isStunned = true;
				}
			}
		}
	}
}