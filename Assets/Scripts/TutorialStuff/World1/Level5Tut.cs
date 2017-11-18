using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Tut : MonoBehaviour {

	Vector3 m_messagePosition = Vector3.zero;

	void Start () {
		if(!PlayerPrefs.HasKey("Level5TutStartTriggered")) {
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Looks like there is a new trap! Watch out for those saws, they look dangerous!", m_messagePosition);
			PlayerPrefs.SetString("Level5TutStartTriggered", "");
		}
	}
}
