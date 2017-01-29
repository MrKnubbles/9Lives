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
	private Vector3 startPosition;
	private Player player;
	private MoveObject moveObject;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		resetTimer = maxResetTimer;
		if (isHidden){
			startPosition = transform.localPosition;
			moveObject = GetComponent<MoveObject>();
			moveObject.SetSpeed(moveSpeed);
			moveObject.SetDistanceX(moveHorizontal);
			moveObject.SetDistanceY(moveVertical);
		}
	}

	void Update() {
		HandleMovement();
	}

	void HandleMovement() {
		if (isActive){
			resetTimer -= Time.deltaTime;
			if (isActivated) {
				if (isHidden){
					moveObject.Move();
				}
				else{
					GetComponent<Collider2D>().enabled = true;
					isActive = false;
				}
				isActivated = false;
			}
			if (isHidden && resetTimer <= 0){
				if(!moveObject.isObjectMoving() && (transform.localPosition != startPosition)) {
					moveObject.SetSpeed(moveSpeed/3);
					moveObject.SetDistanceX(-moveHorizontal);
					moveObject.SetDistanceY(-moveVertical);
					moveObject.Move();
				}
				else if (isHidden && resetTimer <= 0 && transform.localPosition == startPosition){
					resetTimer = maxResetTimer;
					isActive = false;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && !player.isDead && !isActivated && !isActive){
			if (isHidden && !moveObject.isObjectMoving()){
				moveObject.SetSpeed(moveSpeed);
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				GetComponent<Collider2D>().enabled = true;
				isActive = true;
				isActivated = true;
			}
			else if (!isHidden){
				GetComponent<Collider2D>().enabled = true;
				isActive = true;
				isActivated = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead && !isActivated && !isActive){
			if (isHidden && !moveObject.isObjectMoving() && moveObject.isDoneMoving()){
				moveObject.SetSpeed(moveSpeed);
				moveObject.SetDistanceX(moveHorizontal);
				moveObject.SetDistanceY(moveVertical);
				GetComponent<Collider2D>().enabled = true;
				isActive = true;
				isActivated = true;
			}
			else if (!isHidden){
				GetComponent<Collider2D>().enabled = true;
				isActive = true;
				isActivated = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Player") {
			GetComponent<Collider2D>().enabled = false;
		}
	}
}