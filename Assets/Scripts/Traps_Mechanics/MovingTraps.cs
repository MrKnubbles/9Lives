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

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			GetComponent<Collider2D>().enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead){
			GetComponent<Collider2D>().enabled = true;
		}
	}
}