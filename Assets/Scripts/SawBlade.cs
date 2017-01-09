using UnityEngine;

public class SawBlade : MonoBehaviour {

	// Rigidbody
	private Rigidbody2D m_rb2d;

	// Movement Stuff
	private Vector2 m_velocity;
	private Vector3 m_startPosition;
	private Vector3 m_targetPositionHorizontal;
	private Vector3 m_targetPositionVertical;
	public bool m_moveHorizontal;
	public bool m_moveVertical;
	public bool m_invert;
	public float m_moveSpeed;
	public float m_moveDistance;

	void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();
		m_startPosition = transform.position;
		m_targetPositionHorizontal = new Vector3(m_startPosition.x + m_moveDistance, m_startPosition.y, m_startPosition.z);
		m_targetPositionVertical = new Vector3(m_startPosition.x, m_startPosition.y + m_moveDistance, m_startPosition.z);
	}
	
	void FixedUpdate() {
		if(m_moveHorizontal){
			m_velocity = new Vector2(1, 0);
			MoveHorizontal();
		} else if(m_moveVertical) {
			m_velocity = new Vector2(0, 1);
			MoveVertical();
		} else {
			m_rb2d.position = m_startPosition;
			m_rb2d.velocity = new Vector3(0, 0, 0);
		}
	}

	void MoveHorizontal() {
		if (m_invert){
			if(transform.position.x >= m_startPosition.x) {
			m_rb2d.velocity += (m_velocity * -m_moveSpeed) * Time.fixedDeltaTime;
			} else if(transform.position.x <= m_targetPositionHorizontal.x) {
				m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
			}
		}
		else{
			if(transform.position.x <= m_startPosition.x) {
				m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
			} else if(transform.position.x >= m_targetPositionHorizontal.x) {
				m_rb2d.velocity += (m_velocity * -m_moveSpeed) * Time.fixedDeltaTime;
			}
		}
	}

	void MoveVertical() {
		if (m_invert){
			if(transform.position.y >= m_startPosition.y) {
			m_rb2d.velocity += (m_velocity * -m_moveSpeed) * Time.fixedDeltaTime;
			} else if(transform.position.y <= m_targetPositionVertical.y) {
				m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
			}
		}
		else{
			if(transform.position.y <= m_startPosition.y) {
				m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
			} else if(transform.position.y >= m_targetPositionVertical.y) {
				m_rb2d.velocity += (m_velocity * -m_moveSpeed) * Time.fixedDeltaTime;
			}
		}
	}
}