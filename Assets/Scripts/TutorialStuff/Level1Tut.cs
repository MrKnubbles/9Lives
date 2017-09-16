using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Tut : MonoBehaviour {

	int hasTriggered;

	void Start () {
		if(PlayerPrefs.HasKey("Level1TutTriggered")) {
			hasTriggered = PlayerPrefs.GetInt("Level1TutTriggered");
		}
		if(hasTriggered == 0) {
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Jump over the spikes!!");
			PlayerPrefs.SetInt("Level1TutTriggered", 1);
		}
	}
}
