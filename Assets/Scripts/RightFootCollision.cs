using UnityEngine;
using System.Collections;

public class RightFootCollision : MonoBehaviour {

	public Player player;
	public LeftFootCollision leftFoot;
	public bool isGrounded;
	private int index;

	void Start(){
		player = GameObject.Find("Player").GetComponent<Player>();
		index = transform.GetSiblingIndex();
		leftFoot = transform.parent.GetChild(index - 1).gameObject.GetComponent<LeftFootCollision>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Platform"){
			isGrounded = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Platform"){
			if (leftFoot.isGrounded){
				SetGrounded();
			}
			else if (player.rb2d.velocity.y == 0){
				SetGrounded();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Platform"){
			isGrounded = false;
		}
	}

	void SetGrounded(){
		player.GetComponent<Animator>().SetBool("isJumping", false);
		player.GetComponent<Animator>().SetBool("isFalling", false);
		player.isGrounded = true;
		player.isJumping = false;
		player.hasDoubleJumped = false;
	}
}