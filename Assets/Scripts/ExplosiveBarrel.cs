using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour {

	public GameObject m_explosion;
	public GameObject m_barrel;

	void Start () {
		m_explosion.SetActive(false);
		m_barrel.SetActive(true);
	}
	
	void OnCollisionEnter2D(Collision2D other) {
		if(other.collider.tag == "Player") {
			m_explosion.SetActive(true);
			GetComponent<Rigidbody2D>().isKinematic = true;
			GetComponent<Collider2D>().enabled = false;
			m_barrel.SetActive(false);
			Destroy(this.gameObject, 2.0f);
		}
	}
}
