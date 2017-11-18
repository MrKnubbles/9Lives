using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Tut : MonoBehaviour {

	Vector3 m_messagePosition = Vector3.zero;

	void Start () {
		if(!PlayerPrefs.HasKey("Level2TutStartTriggered")) {
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Press jump a second time while in the air to do a double jump!", m_messagePosition);
			PlayerPrefs.SetString("Level2TutStartTriggered", "");
		}
	}
}
