using UnityEngine;

public class Spikes : MonoBehaviour {

	public Player player;
	private Rigidbody2D m_rb2d;

	// Movement Stuff
	public float maxRetractTimer;
	private float retractTimer;

	// my Stuff
	private MoveObject moveObject;
	public bool isHidden;
	private bool isActive;
	private bool isActivated;
	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private Vector3 finalPosition;

	void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();
		retractTimer = maxRetractTimer;
		player = GameObject.Find("Player").GetComponent<Player>();
		if (isHidden){
			moveObject = GetComponent<MoveObject>();
			moveObject.SetSpeed(moveSpeed);
			moveObject.SetDistanceX(moveHorizontal);
			moveObject.SetDistanceY(moveVertical);
		}
	}

	void FixedUpdate() {
		HandleMovement();
	}

	void HandleMovement() {
		if (isActivated){
			retractTimer -= Time.fixedDeltaTime;
			if (isActive) {
				GetComponent<Collider2D>().enabled = true;
				if (isHidden){
					moveObject.Move();
					isActive = false;
				}
			}
			if (retractTimer <= 0) {
				isActivated = false;
				if (isHidden){
					moveObject.SetSpeed(moveSpeed/3);
					moveObject.SetDistanceX(-moveHorizontal);
					moveObject.SetDistanceY(-moveVertical);
					moveObject.Move();
				}
				retractTimer = maxRetractTimer;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player" && !player.isDead) {
			if (!isActivated){
				if (isHidden){
					moveObject.SetSpeed(moveSpeed);
					moveObject.SetDistanceX(moveHorizontal);
					moveObject.SetDistanceY(moveVertical);
				}
				GetComponent<Collider2D>().enabled = true;
				isActive = true;
				isActivated = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.collider.tag == "Player") {
			GetComponent<Collider2D>().enabled = false;
		}
	}
}