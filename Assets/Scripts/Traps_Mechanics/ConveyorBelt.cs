using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

	public bool m_onConveyor;
	private GameObject m_player;
	private float moveSpeed = 1.6f;

	void Start() {
		m_onConveyor = false;
		m_player = GameObject.Find("Player");
		// If you want the belt to move the opposite direction,
		// change the parent's localScale.x to -1.
		if (transform.parent.transform.localScale.x < 0){
			moveSpeed *= -1;
		}
	}

	void FixedUpdate() {
		if(m_onConveyor) {
			m_player.transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			m_onConveyor = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			m_onConveyor = false;
		}
	}
}
