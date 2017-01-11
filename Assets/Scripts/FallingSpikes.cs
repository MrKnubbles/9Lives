using UnityEngine;

public class FallingSpikes : MonoBehaviour {
	private Player player;
	private Rigidbody2D m_rb2d;

	// Movement
	public float maxResetTimer;
	private float resetTimer;
	private MoveObject moveObject;
	private bool isActive;
	public bool isActivated;
	public float moveSpeed;
	public float moveVertical;
	private Vector3 resetPosition;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		m_rb2d = GetComponent<Rigidbody2D>();
		resetPosition = transform.position;
		resetPosition.y += 1;
		resetTimer = maxResetTimer;
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceY(moveVertical);
	}

	void FixedUpdate() {
		if (isActive){
			Fall();
		}
		if (isActivated){
			if (resetTimer >= 0){
				resetTimer -= Time.fixedDeltaTime;
			}
			else if (resetTimer <= 0){
				resetTimer = maxResetTimer;
				isActivated = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && !player.isDead && !isActivated) {
			isActive = true;
			isActivated = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead && !isActivated) {
			isActive = true;
			isActivated = true;
		}
	}

	void Fall() {
		m_rb2d.velocity = new Vector3(0, m_rb2d.velocity.y - 3.75f, 0);
		m_rb2d.gravityScale = 2f;
		isActive = false;
	}

	public void Reset(){
		transform.position = resetPosition;
		m_rb2d.velocity = new Vector3(0, 0, 0);
		m_rb2d.gravityScale = 0;
		moveObject.Move();
	}
}