using UnityEngine;

public class DoorSwitch : MonoBehaviour {

	private Player player;
	public GameObject onSwitch;
	public GameObject offSwitch;
	public bool isTimedSwitch;
	public bool isActive = false;
	public float maxDeactivateTimer;
	private float deactivateTimer;
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
					audioManager.PlayLoop(audioManager.sfxTicking);
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
		}
	}

	void DeactivateSwitch(){
		offSwitch.SetActive(true);
		onSwitch.SetActive(false);
		deactivateTimer = maxDeactivateTimer;
		isActive = false;
		if (isTimedSwitch){
			// TODO: Find out how to target the ticking sfx specifically and stop it.
			audioManager.sfx[0].Stop();
			isTicking = false;
		}
	}
}