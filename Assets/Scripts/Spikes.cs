using UnityEngine;

public class Spikes : MonoBehaviour {

	public bool isHidden;
	public float moveSpeed;
	public float moveHorizontal;
	public float moveVertical;
	private float maxResetTimer = 1.25f;
	private float resetTimer;
	private bool isActive;
	private bool isActivated;
	private Player player;
	private MoveObject moveObject;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		resetTimer = maxResetTimer;
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
			resetTimer -= Time.fixedDeltaTime;
			if (isActive) {
				GetComponent<Collider2D>().enabled = true;
				if (isHidden){
					moveObject.Move();
					isActive = false;
				}
			}
			if (resetTimer <= 0) {
				isActivated = false;
				if (isHidden){
					moveObject.SetSpeed(moveSpeed/3);
					moveObject.SetDistanceX(-moveHorizontal);
					moveObject.SetDistanceY(-moveVertical);
					moveObject.Move();
				}
				resetTimer = maxResetTimer;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && !player.isDead && !isActivated){
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

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Player") {
			GetComponent<Collider2D>().enabled = false;
		}
	}
}