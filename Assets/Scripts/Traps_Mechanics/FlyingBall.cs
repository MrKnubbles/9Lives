using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBall : MonoBehaviour {

	public Vector2 force;
	[SerializeField] float resetTime;
	float currentResetTime;
	Vector3 startPosition;
	Rigidbody2D rb2d;
	[SerializeField] bool isFlying = false;

	void Start() {
		startPosition = this.transform.position;
		rb2d = GetComponent<Rigidbody2D>();
		currentResetTime = resetTime;
	}

	void Update() {
		if(isFlying) {
			UpdateResetTime();
		}
		UpdateUpwardFlyingBall();
	}

	void UpdateResetTime() {
		if(currentResetTime > 0) {
			currentResetTime -= Time.deltaTime;
		} else {
			// this will reset the ball to the start position and start over
			// TODO: Play some sort of FX here as if the ball was destroyed in case it hasn't fallen off screen yet.
			isFlying = false;
			rb2d.velocity = Vector2.zero;
			currentResetTime = resetTime;
		}
	}

	void UpdateUpwardFlyingBall() {
		if(!isFlying) {
			isFlying = true;
			this.transform.position = startPosition;
			rb2d.AddForce(force);
		}
	}
}
