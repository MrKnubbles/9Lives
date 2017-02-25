using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour {

	Rigidbody2D m_rb2d;
	private float m_swingForce;

	void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();
		m_swingForce = 1f;
	}

	void Update() {
		UpdateSwing();
	}

	void UpdateSwing() {
		if(m_rb2d.velocity.x <= .01f) {
			if(m_rb2d.rotation <= 180f) {
				m_rb2d.AddForce(new Vector2(-m_swingForce, 0));
			} else if(m_rb2d.rotation >= 0f) {
				m_rb2d.AddForce(new Vector2(m_swingForce, 0));
			}
		}
	}
}
