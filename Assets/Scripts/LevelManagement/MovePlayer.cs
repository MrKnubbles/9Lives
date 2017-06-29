using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private float moveSpeed     	= 1;
	private float moveAmountX   	= 0;	
	private bool isMoving 			= false; // is this element currently moving? 
	public bool doneMoving			= false;
	private bool startMoving 		= false;
	

	public void SetSpeed(float newSpeed) { moveSpeed = newSpeed; } 
	public void SetDistanceX(float newXAmount) { moveAmountX = newXAmount; }
	public void Move() { startMoving = true; }
	public bool isObjectMoving() { return isMoving; }
	public bool isDoneMoving() { return doneMoving; }
		
	void Update () {	
		if ((!isMoving && startMoving) && (this)) {
			if (moveAmountX != 0) {
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
}