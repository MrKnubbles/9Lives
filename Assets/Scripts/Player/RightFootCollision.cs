﻿using UnityEngine;
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
		if (other.gameObject.tag == "Platform" || other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "Ramp"){			
			if(!player.isHit) {
				isGrounded = true;
			} else {
				player.isHit = false;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Platform" || other.gameObject.tag == "MovingPlatform"){
			if (leftFoot.isGrounded){		
				if(!player.isHit) {
					player.SetGrounded();	
				}
			}
			else if (player.rb2d.velocity.y == 0){		
				if(!player.isHit) {
					player.SetGrounded();	
				}
			}
		}
		if (other.gameObject.tag == "Ramp"){		
			if(!player.isHit) {
				player.SetGrounded();	
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Platform" || other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "Ramp"){
			isGrounded = false;
		}
	}
}