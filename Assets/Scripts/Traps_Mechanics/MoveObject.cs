using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	private float moveSpeed     	= 1;
	private float moveAmountX   	= 0;						
	private float moveAmountY   	= 0;
	private float rotationAmount	= 0;		
	private bool isMoving 		= false; // is this element currently moving? 
	private bool isRotating		= false;
	// private bool doneMoving		= false;
	// private bool doneRotating		= false;
	private bool startMoving 		= false;
	private bool startRotating	= false;
	

	public void SetSpeed(float newSpeed) { moveSpeed = newSpeed; } 
	public void SetDistanceX(float newXAmount) { moveAmountX = newXAmount; }
	public void SetDistanceY(float newYAmount) { moveAmountY = newYAmount; }
	public void SetRotation(float newRotationAmount) { rotationAmount = newRotationAmount; }
	public void Move() { startMoving = true; }
	public void Rotate() {startRotating = true; }
	public bool isObjectMoving() { return isMoving; }
	public bool isObjectRotating() { return isRotating; }
	// public bool isDoneMoving() { return doneMoving; }
	// public bool isDoneRotating() { return doneRotating; }
		
	void Update () {	
		if ((!isMoving && startMoving) && (this)) {
			// which way? 
			if (moveAmountY != 0) {
				StartCoroutine(MoveVertical(moveAmountY));
			} 
			if (moveAmountX != 0) {
				StartCoroutine(MoveHorizontal(moveAmountX));
			}
		}
		if ((!isRotating && startRotating) && (this)){
			StartCoroutine(Rotate(rotationAmount));
		}
		// if (!isMoving && doneMoving){
		// 	doneMoving = false;
		// }
		// if (!isRotating && doneRotating){
		// 	doneRotating = false;
		// }
	}

	// Co-Routines to do the work
	public IEnumerator MoveHorizontal(float moveAmountX) { // Moves the object horizontally.
		startMoving = false;
		isMoving = true;
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = currentPosition;
		targetPosition.x = currentPosition.x + moveAmountX;
		
		while(currentPosition.x != targetPosition.x){
			currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetPosition.x, Time.deltaTime * moveSpeed);
			transform.localPosition = currentPosition;
		
			yield return null;
		}
		// doneMoving = true;
		isMoving = false;
		yield return 0;
	}

	public IEnumerator MoveVertical(float moveAmountY) { // Moves the object vertically.
		startMoving = false;
		isMoving = true;
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = currentPosition;
		targetPosition.y = currentPosition.y + moveAmountY;

		while(currentPosition.y != targetPosition.y){
			currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetPosition.y, Time.deltaTime * moveSpeed);
			transform.localPosition = currentPosition;
			
			yield return null;
		}
		// doneMoving = true;
		isMoving = false;
		yield return 0;
	}

	public IEnumerator Rotate(float rotationAmount){	// Rotates the object
		startRotating = false;
		isRotating = true;
		Vector3 currentRot = transform.rotation.eulerAngles;
		Vector3 targetRot = currentRot;
		targetRot.z = currentRot.z + rotationAmount;

		while(currentRot.z != targetRot.z){
			currentRot.z = Mathf.MoveTowards(currentRot.z, targetRot.z, Time.deltaTime * moveSpeed);
			transform.eulerAngles = currentRot;
			
			yield return null;
		}
		//doneRotating = true;
		isRotating = false;
		yield return 0;
	}
}
