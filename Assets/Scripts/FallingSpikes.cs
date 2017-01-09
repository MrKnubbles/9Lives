using UnityEngine;

public class FallingSpikes : MonoBehaviour {

	// Rigidbody
	private Rigidbody2D m_rb2d;

	// Movement Stuff
	public bool m_activate;
	public bool m_isFake = false;
	public float m_moveSpeed;
	public float m_moveDistance;
	public float m_maxResetTimer;
	private float m_resetTimer;
	public bool m_hasTriggered;
	private Vector3 m_startPosition;
	private Vector2 m_velocity = new Vector2(1, 0);

	// Wiggle Stuff
	private Vector3 m_maxWiggle;
	public float m_wiggleTimer;
	public float m_maxWiggleTimer;
	public bool m_wiggle;

	private Player player;

	void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();
		m_startPosition = transform.position;
		m_maxWiggle = new Vector3(m_startPosition.x + m_moveDistance, m_startPosition.y, m_startPosition.z);
		m_resetTimer = m_maxResetTimer;
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void FixedUpdate() {
		if(m_activate) {
			if(m_wiggleTimer > 0){
				m_wiggleTimer -= Time.fixedDeltaTime;
				Wiggle();
			} else {
				if (!m_isFake){
					Fall();
				} else{
					Reset();
				}
			}
		}
		if (m_hasTriggered && m_resetTimer >= 0){
			m_resetTimer -= Time.fixedDeltaTime;
		}
		if (m_resetTimer <= 0 && m_hasTriggered){
			m_resetTimer = m_maxResetTimer;
			m_hasTriggered = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player" && !player.isDead && m_resetTimer == m_maxResetTimer) {
			m_hasTriggered = true;
			m_activate = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		// if (other.gameObject.tag == "Player"){
		// 	transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
		// }
		if(other.gameObject.tag == "TrapCatcher") {
			m_hasTriggered = true;
			Reset();
		}
	}

	void Wiggle() {
		if(transform.position.x <= m_startPosition.x) {
			m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
		} else if(transform.position.x >= m_maxWiggle.x) {
			m_rb2d.velocity += (m_velocity * -m_moveSpeed) * Time.fixedDeltaTime;
		}
	}

	void Fall() {
		m_rb2d.velocity = new Vector3(0, m_rb2d.velocity.y - 1.5f, 0);
		m_rb2d.gravityScale = 1f;
	}

	public void Reset(){
		//transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
		transform.position = m_startPosition;
		m_rb2d.velocity = new Vector3(0, 0, 0);
		m_rb2d.gravityScale = 0;
		m_wiggleTimer = m_maxWiggleTimer;
		m_activate = false;
	}
}
