using UnityEngine;

public class FallingPlatform : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other) {
		if(other.collider.tag == "Player") {
			GetComponent<Rigidbody2D>().isKinematic = false;
		}
		// if (other.collider.tag == "Trap"){
		// 	Destroy(gameObject);
		// }
	}
}