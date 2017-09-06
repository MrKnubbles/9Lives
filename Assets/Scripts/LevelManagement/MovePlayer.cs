using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private float moveSpeed     	= 1;
	private float moveAmountX   	= 0;
	private float moveAmountY   	= 0;
	private bool isMoving 			= false; // is this element currently moving? 
	public bool doneMoving			= false;
	private bool startMoving 		= false;
	

	public void SetSpeed(float newSpeed) { moveSpeed = newSpeed; } 
	public void SetDistanceX(float newXAmount) { moveAmountX = newXAmount; }
	public void SetDistanceY(float newYAmount) { moveAmountY = newYAmount; }
	public void SetDistanceXY(float newXAmount, float newYAmount) { moveAmountX = newXAmount; moveAmountY = newYAmount; }
	public void Move() { startMoving = true; }
	public bool isObjectMoving() { return isMoving; }
	public bool isDoneMoving() { return doneMoving; }
		
	void Update () {	
		if ((!isMoving && startMoving) && (this)) {
			if (moveAmountY != 0){
				StartCoroutine(MovePlayerXY(moveAmountX, moveAmountY));
			}
			else if (moveAmountX != 0) {
				StartCoroutine(MovePlayerX(moveAmountX));
			}
		}
	}

	// Co-Routines to do the work
	public IEnumerator MovePlayerX(float moveAmount){
		startMoving = false;
		doneMoving = false;
		isMoving = true;
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = currentPosition;
		targetPosition.x = currentPosition.x + moveAmountX;
		
		while(currentPosition.x != targetPosition.x){
			currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetPosition.x, Time.deltaTime * moveSpeed);
			transform.position = currentPosition;
		
			yield return null;
		}
		doneMoving = true;
		isMoving = false;
		yield return 0;
	}

	public IEnumerator MovePlayerY(float moveAmount){
		startMoving = false;
		doneMoving = false;
		isMoving = true;
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = currentPosition;
		targetPosition.y = currentPosition.y + moveAmountY;
		
		while(currentPosition.y != targetPosition.y){
			currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetPosition.y, Time.deltaTime * moveSpeed);
			transform.position = currentPosition;
		
			yield return null;
		}
		doneMoving = true;
		isMoving = false;
		yield return 0;
	}

	// Moves the Player diagonally (up and down stairs)
	public IEnumerator MovePlayerXY(float moveXAmount, float moveYAmount){
		startMoving = false;
		doneMoving = false;
		isMoving = true;
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = currentPosition;
		targetPosition.x = currentPosition.x + moveAmountX;
		targetPosition.y = currentPosition.y + moveAmountY;
		
		while((currentPosition.x != targetPosition.x) && (currentPosition.y != targetPosition.y)){
			currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetPosition.x, Time.deltaTime * moveSpeed);
			currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetPosition.y, Time.deltaTime * moveSpeed);
			transform.position = currentPosition;
		
			yield return null;
		}
		moveAmountX = 0;
		moveAmountY = 0;
		doneMoving = true;
		isMoving = false;
		yield return 0;
	}
}