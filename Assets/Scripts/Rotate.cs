using UnityEngine;

public class Rotate : MonoBehaviour {

	// Rotation Stuff
	public float m_rotateSpeed;
	private Vector3 m_rotation;

	void Start() {
		m_rotation = new Vector3(0, 0, -m_rotateSpeed);
	}
	
	void Update() {
		RotateObject();
	}

	void RotateObject() {
		transform.Rotate(m_rotation);
	}
}
