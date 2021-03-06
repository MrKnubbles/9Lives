﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCatcher : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "TriggerTrap") {
			if (other.gameObject.name == "FallingSpikes"){
				FallingSpikes script = other.transform.parent.GetComponent<FallingSpikes>();
				script.isActivated = true;
				script.Reset();
			}
			else if (other.gameObject.name == "Drip"){
				DrippingPipe script = other.transform.GetComponent<DrippingPipe>();
				script.Reset();
			}
		}
	}
}
