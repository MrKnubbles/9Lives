using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	private Vector3 startPosition;
	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private Vector3 resetPosition;
	private MoveObject moveObject;

	void Start() {
		startPosition = transform.localPosition;
		resetPosition.x = startPosition.x + moveHorizontal;
		resetPosition.y = startPosition.y + moveVertical;
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (moveHorizontal != 0){
			HorizontalMovement();
		}
		if (moveVertical != 0){
			VerticalMovement();
		}
	}

	void HorizontalMovement(){
		// If initial horizontal move amount is positive.
		if (moveHorizontal > 0){
			// If moving left and reached final position, reverse direction.
			if (transform.localPosition.x <= startPosition.x){
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				moveObject.Move();
			}
			// If moving right and reached final position, reverse direction.
			else if (transform.localPosition.x >= resetPosition.x){
				moveObject.SetDistanceX(-moveHorizontal);
				moveObject.SetDistanceY(-moveVertical);
				moveObject.Move();
			}
		}
		// If initial horizontal move amount is negative.
		else if (moveHorizontal < 0){
			// If moving right and reached final position, reverse direction.
			if (transform.localPosition.x >= startPosition.x){
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				moveObject.Move();
			}
			// If moving left and reached final position, reverse direction.
			else if (transform.localPosition.x <= resetPosition.x){
				moveObject.SetDistanceX(-moveHorizontal);
				moveObject.SetDistanceY(-moveVertical);
				moveObject.Move();
			}
		}
	}
	void VerticalMovement(){
		// If initial vertical move amount is positive.
		if (moveVertical > 0){
			// If moving down and reached final position, reverse direction.
			if (transform.localPosition.y <= startPosition.y){
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				moveObject.Move();
			}
			// If moving up and reached final position, reverse direction.
			else if (transform.localPosition.y >= resetPosition.y){
				moveObject.SetDistanceX(-moveHorizontal);
				moveObject.SetDistanceY(-moveVertical);
				moveObject.Move();
			}
		}
		// If initial vertical move amount is negative.
		else if (moveVertical < 0){
			// If moving up and reached final position, reverse direction.
			if (transform.localPosition.y >= startPosition.y){
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				moveObject.Move();
			}
			// If moving down and reached final position, reverse direction.
			else if (transform.localPosition.y <= resetPosition.y){
				moveObject.SetDistanceX(-moveHorizontal);
				moveObject.SetDistanceY(-moveVertical);
				moveObject.Move();
			}
		}
	}
}
