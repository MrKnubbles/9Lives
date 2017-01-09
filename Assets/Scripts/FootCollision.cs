using UnityEngine;
using System.Collections;

public class FootCollision : MonoBehaviour {

	public Player player;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Platform"){
			player.GetComponent<Animator>().SetBool("isJumping", false);
			player.GetComponent<Animator>().SetBool("isFalling", false);
			player.isGrounded = true;
			player.isJumping = false;
			player.hasDoubleJumped = false;
		}
	}
}
