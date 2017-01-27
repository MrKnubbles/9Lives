using UnityEngine;
using System;

public class MovingTraps : MonoBehaviour {

	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private MoveObject moveObject;
	private Player player;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}
	
	void Update () {
		// If object reaches it's destination...
		if (moveObject.isDoneMoving()){
			if (moveHorizontal != 0){
				HorizontalMovement();
			}
			if (moveVertical != 0){
				VerticalMovement();
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