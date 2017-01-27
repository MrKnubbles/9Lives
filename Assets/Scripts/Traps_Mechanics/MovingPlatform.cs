using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private MoveObject moveObject;

	void Start() {
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}
	
	void Update () {
		// If object reaches it's destination, reverse direction and move.
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
		moveHorizontal = -moveHorizontal;
		moveObject.SetDistanceX(moveHorizontal);
		moveObject.Move();
	}
	void VerticalMovement(){
		moveVertical = -moveVertical;
		moveObject.SetDistanceY(moveVertical);
		moveObject.Move();
	}
}
