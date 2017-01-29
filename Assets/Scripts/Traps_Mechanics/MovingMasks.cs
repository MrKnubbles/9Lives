using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMasks : MonoBehaviour {
	public GameObject movingPlatform;
	
	void Update () {
		transform.localPosition = movingPlatform.transform.localPosition;
	}
}
