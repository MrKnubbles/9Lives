using UnityEngine;

public class Door : MonoBehaviour {

	private GameObject openDoor;
	private GameObject closedDoor;
	public DoorSwitch doorSwitch;
	private Player player;
	public bool isActive = true;

	void Start() {
		player = GameObject.Find("Player").GetComponent<Player>();
		openDoor = this.transform.GetChild(0).gameObject;
		closedDoor = this.transform.GetChild(1).gameObject;
		if (GameObject.Find("DoorSwitch") != null){
			doorSwitch = GameObject.Find("DoorSwitch/Switches").GetComponent<DoorSwitch>();
			CloseDoor();
			isActive = false;
		}
		else{
			isActive = true;
		}
	}

	void Update() {
		if (GameObject.Find("DoorSwitch") != null){
			HandleDoor();
		}
	}

	void OpenDoor(){
		openDoor.SetActive(true);
		closedDoor.SetActive(false);
	}

	void CloseDoor(){
		closedDoor.SetActive(true);
		openDoor.SetActive(false);
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