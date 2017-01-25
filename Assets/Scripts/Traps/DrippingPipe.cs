using UnityEngine;

public class DrippingPipe : MonoBehaviour {
	public bool isActivated;
	public float maxDelayTimer = 2f;
	private float resetTimer;
	private bool isActive;
	private Vector3 resetPosition;
	private Player player;
	private Rigidbody2D rb2d;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		rb2d = GetComponent<Rigidbody2D>();
		resetPosition = transform.localPosition;
		resetTimer = maxDelayTimer;
		isActive = true;
		Fall();
	}

	void Update() {
		if (isActive && !isActivated){
			Fall();
		}
		if (!isActive && isActivated){
			if (resetTimer >= 0){
				resetTimer -= Time.fixedDeltaTime;
			}
			else if (resetTimer <= 0){
				resetTimer = maxDelayTimer;
				isActive = true;
				isActivated = false;
			}
		}
	}

	void Fall() {
		rb2d.gravityScale = 1f;
		rb2d.velocity = new Vector3(0, -.25f, 0);
		isActivated = true;
	}

	public void Reset(){
		transform.localPosition = resetPosition;
		rb2d.gravityScale = 0;
		rb2d.velocity = new Vector3(0, 0, 0);
		isActive = false;
	}
}