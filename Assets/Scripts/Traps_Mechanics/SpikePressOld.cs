using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePress : MonoBehaviour {

	public GameObject m_leftSpike;
	public GameObject m_rightSpike;
	private Vector2 m_leftSpikeStartLocation;
	private Vector2 m_rightSpikeStartLocation;
	private Vector2 m_middlePosition;
	public bool m_insideTrap;
	private float m_closeSpeed = 0.075f;
	private float m_openSpeed = 0.025f;

	void Start() {
		m_leftSpikeStartLocation = m_leftSpike.transform.position;
		m_rightSpikeStartLocation = m_rightSpike.transform.position;
	}

	void Update() {
		if(m_insideTrap) {
			CloseSpikes();
		} else {
			OpenSpikes();
		}
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			m_insideTrap = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			m_insideTrap = false;
		}
	}

	void CloseSpikes() {
		m_middlePosition = (m_rightSpike.transform.position / 2) + (m_leftSpike.transform.position / 2);
		m_leftSpike.transform.position = Vector2.MoveTowards(m_leftSpike.transform.position, m_middlePosition, m_closeSpeed);
		m_rightSpike.transform.position = Vector2.MoveTowards(m_rightSpike.transform.position, m_middlePosition, m_closeSpeed);
	}

	void OpenSpikes() {
		m_leftSpike.transform.position = Vector2.MoveTowards(m_leftSpike.transform.position, m_leftSpikeStartLocation, m_openSpeed);
		m_rightSpike.transform.position = Vector2.MoveTowards(m_rightSpike.transform.position, m_rightSpikeStartLocation, m_openSpeed);
	}
}
