using UnityEngine;

public class Door : MonoBehaviour {

	public GameObject m_openDoor;
	public GameObject m_closedDoor;
	public DoorSwitch doorSwitch1;
	public DoorSwitch doorSwitch2;
	public Player player;
	public bool m_timedDoor;
	public bool m_hasTwoSwitches;
	public bool m_activate;
	public float m_maxDeactivateTimer;
	private float m_deactivateTimer;

	void Start() {
		m_deactivateTimer = m_maxDeactivateTimer;
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void Update() {
		HandleDoor();
		if (m_timedDoor && doorSwitch1.m_activate){
			if (m_hasTwoSwitches && doorSwitch2.m_activate){
				m_activate = true;
				m_maxDeactivateTimer = doorSwitch2.m_maxDeactivateTimer;
			}
			else if (!m_hasTwoSwitches){
				m_activate = true;
				m_maxDeactivateTimer = doorSwitch1.m_maxDeactivateTimer;
			}
		}
		else if ((m_timedDoor && !doorSwitch1.m_activate) || (m_hasTwoSwitches && !doorSwitch2.m_activate)){
			m_activate = false;
		}
		if (m_timedDoor && player.isDead){
			m_activate = false;
		}
	}

	void HandleDoor() {
		if(m_activate) {
			m_openDoor.SetActive(true);
			m_closedDoor.SetActive(false);
			if(m_timedDoor) {
				m_deactivateTimer -= Time.deltaTime;
				if(m_deactivateTimer <= 0 || m_activate == false) {
					m_closedDoor.SetActive(true);
					m_openDoor.SetActive(false);
					m_deactivateTimer = m_maxDeactivateTimer;
					m_activate = false;
				}
			}
		} else {
			m_closedDoor.SetActive(true);
			m_openDoor.SetActive(false);
		}
	}
}
