using UnityEngine;

public class Door : MonoBehaviour {

	public DoorSwitch doorSwitch;
	private Player player;
	public bool isActive = true;
	private Animator animator;
	public AnimationState state;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		animator = GetComponent<Animator>();
		animator.SetBool("isDoorOpening", false);
		animator.SetBool("isDoorClosing", false);
		if (GameObject.Find("DoorSwitch") != null){
			doorSwitch = GameObject.Find("DoorSwitch/Switches").GetComponent<DoorSwitch>();
			StartDoorClosed();
		}
		else{
			StartDoorOpen();
		}
	}

	void Update() {
		if (GameObject.Find("DoorSwitch") != null){
			HandleDoor();
		}
	}

	void OpenDoor(){
		animator.SetBool("isDoorOpening", true);
		animator.SetBool("isDoorClosing", false);
	}

	void CloseDoor(){
		animator.SetBool("isDoorOpening", false);
		animator.SetBool("isDoorClosing", true);
	}

	void StartDoorClosed(){
		isActive = false;
		animator.Play("door_close", 0, 1f);
	}

	void StartDoorOpen(){
		isActive = true;
		animator.Play("door_open", 0, 1f);
	}

	void HandleDoor() {
		if (doorSwitch.isActive){
			isActive = true;
		}
		if(isActive) {
			OpenDoor();
		}
		if (player.isDead || !doorSwitch.isActive){
			isActive = false;
			CloseDoor();
		}
	}
}