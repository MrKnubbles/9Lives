using UnityEngine;

public class DoorSwitch : MonoBehaviour {

	public GameObject m_greenSwitch;
	public GameObject m_redSwitch;
	public Player player;
	public bool m_timedSwitch;
	public bool m_activate;
	public bool m_inRange = false;
	public float m_maxDeactivateTimer;
	private float m_deactivateTimer;

	void Start() {
		m_deactivateTimer = m_maxDeactivateTimer;
	}

	void Update() {
		
		HandleSwitch();

		if(m_activate) {
			m_greenSwitch.SetActive(true);
			m_redSwitch.SetActive(false);
			if(m_timedSwitch) {
				m_deactivateTimer -= Time.deltaTime;
				if(m_deactivateTimer <= 0) {
					DeactivateSwitch();
				}
			}
		}
		if (player.isDead){
			DeactivateSwitch();
		}
	}

	void HandleSwitch() {
		if(m_inRange) {
			if(Input.GetKeyDown(KeyCode.E)) {
				m_activate = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			m_inRange = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player") {
			m_inRange = false;;
		}
	}

	void DeactivateSwitch(){
		m_redSwitch.SetActive(true);
		m_greenSwitch.SetActive(false);
		m_deactivateTimer = m_maxDeactivateTimer;
		m_activate = false;
	}
}
