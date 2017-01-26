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
	void Update () {
		if (moveHorizontal != 0){
			HorizontalMovement();
		}
		if (moveVertical != 0){
			VerticalMovement();
		}
	}

	void HorizontalMovement(){
		// If object reaches it's destination, reverse direction and move.
		if (moveObject.isDoneMoving()){
			moveHorizontal = -moveHorizontal;
			moveObject.SetDistanceX(moveHorizontal);
			moveObject.Move();
		}
	}
	void VerticalMovement(){
		// If object reaches it's destination, reverse direction and move.
		if (moveObject.isDoneMoving()){
			moveVertical = -moveVertical;
			moveObject.SetDistanceY(moveVertical);
			moveObject.Move();
		}
	}
}
