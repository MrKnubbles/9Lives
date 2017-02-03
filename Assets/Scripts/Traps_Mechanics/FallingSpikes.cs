using UnityEngine;

public class FallingSpikes : MonoBehaviour {

	public float moveSpeed;
	private float moveVertical = -1;
	public bool isActivated;
	private bool isActive;
	private Vector3 startPosition;
	private Vector3 resetPosition;
	private Player player;
	private Rigidbody2D rb2d;
	private MoveObject moveObject;
	private AudioManager audioManager;
	public AudioClip sfxSpikesFalling;
	public AudioClip sfxSpikesMoving;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		audioManager = AudioManager.Instance;
		rb2d = GetComponent<Rigidbody2D>();
		startPosition = transform.localPosition;
		resetPosition = startPosition;
		resetPosition.y += 1;
		moveObject = GetComponent<MoveObject>();
		moveObject.SetSpeed(moveSpeed);
		moveObject.SetDistanceY(moveVertical);
	}

	void Update() {
		if (isActive){
			Fall();
		}
		if (isActivated){
			if (transform.localPosition == startPosition){
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
		rb2d.velocity = new Vector3(0, rb2d.velocity.y - 2f, 0);
		rb2d.gravityScale = 2f;
		isActive = false;
		audioManager.PlayOnce(sfxSpikesFalling);
	}

	public void Reset(){
		transform.localPosition = resetPosition;
		rb2d.velocity = new Vector3(0, 0, 0);
		rb2d.gravityScale = 0;
		moveObject.Move();
		audioManager.PlayOnce(sfxSpikesMoving);
	}
}