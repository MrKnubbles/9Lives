using UnityEngine;

public class FallingSpikes : MonoBehaviour {

	public float moveSpeed;
	public float moveVertical;
	public bool isActivated;
	private float maxResetTimer = 2f;
	private float resetTimer;
	private bool isActive;
	private Vector3 resetPosition;
	private Player player;
	private Rigidbody2D rb2d;
	private MoveObject moveObject;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		rb2d = GetComponent<Rigidbody2D>();
		resetPosition = transform.localPosition;
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
		rb2d.velocity = new Vector3(0, rb2d.velocity.y - 3.75f, 0);
		rb2d.gravityScale = 2f;
		isActive = false;
	}

	public void Reset(){
		transform.localPosition = resetPosition;
		rb2d.velocity = new Vector3(0, 0, 0);
		rb2d.gravityScale = 0;
		moveObject.Move();
	}
}