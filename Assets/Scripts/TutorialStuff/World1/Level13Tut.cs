using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13Tut : MonoBehaviour {

	Vector3 m_messagePosition = Vector3.zero;

	void Start () {
		if(!PlayerPrefs.HasKey("Level13TutStartTriggered")) {
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Looks like some of those spikes on the roof are loose. Watch out for falling traps!", m_messagePosition);
			PlayerPrefs.SetString("Level13TutStartTriggered", "");
		}
	}
}
