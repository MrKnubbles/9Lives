using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

	public bool m_onConveyor;
	private GameObject m_player;

	void Start() {
		m_onConveyor = false;
		m_player = GameObject.Find("Player");
	}

	void Update() {
		if(m_onConveyor) {
			m_player.transform.position += new Vector3(2f, 0, 0) * Time.deltaTime;
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
