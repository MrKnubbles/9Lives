using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level16Tut : MonoBehaviour {

	Vector3 m_messagePosition = Vector3.zero;

	void Start () {
		if(!PlayerPrefs.HasKey("Level16TutStartTriggered")) {
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Don't fall into that green goo! It doesn't look safe.", m_messagePosition);
			PlayerPrefs.SetString("Level16TutStartTriggered", "");
		}
	}
}
