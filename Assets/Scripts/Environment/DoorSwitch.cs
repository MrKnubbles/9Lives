using UnityEngine;

public class DoorSwitch : MonoBehaviour {

	private Player player;
	public GameObject onSwitch;
	public GameObject offSwitch;
	public bool isTimedSwitch;
	public bool isActive = false;
	public float maxDeactivateTimer;
	private float deactivateTimer;
	public AudioClip sfxTicking;
	private AudioManager audioManager;
	public bool isTicking = false;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		onSwitch = this.transform.GetChild(0).gameObject;
		offSwitch = this.transform.GetChild(1).gameObject;
		if (maxDeactivateTimer == 0){
			isTimedSwitch = false;
		}
		else{
			isTimedSwitch = true;
			deactivateTimer = maxDeactivateTimer;
			audioManager = AudioManager.Instance;
		}
	}

	void Update() {
		HandleSwitch();

		if(isActive) {
			onSwitch.SetActive(true);
			offSwitch.SetActive(false);
			if(isTimedSwitch) {
				if (!isTicking){
					// Sets the ticking sfx to the last location in the array.
					audioManager.PlayLoop(sfxTicking);
					isTicking = true;
				}
				deactivateTimer -= Time.deltaTime;
				if(deactivateTimer <= 0) {
					DeactivateSwitch();
				}
			}
		}
		if (player.isDead){
			DeactivateSwitch();
		}
	}

	void HandleSwitch() {
		if(player.isNearSwitch) {
			if(Input.GetKeyDown(KeyCode.E) || player.isActivatingSwitch) {
				isActive = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			player.isNearSwitch = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player")
			player.isNearSwitch = true;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player") {
			player.isNearSwitch = false;
			player.isActivatingSwitch = false;
		}
	}

	void DeactivateSwitch(){
		offSwitch.SetActive(true);
		onSwitch.SetActive(false);
		deactivateTimer = maxDeactivateTimer;
		isActive = false;
		if (isTimedSwitch){
			// Ticking sfx is always set to the last array location so you need to stop that one specifically.
			audioManager.sfx[audioManager.sfx.Length-1].Stop();
			isTicking = false;
		}
	}
}