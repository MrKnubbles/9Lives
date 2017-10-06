using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Tut : MonoBehaviour {

	[SerializeField] GameObject m_arrowMove;
	[SerializeField] GameObject m_arrowJumpSlide;
	[SerializeField] RectTransform m_messagePositionMove;
	[SerializeField] RectTransform m_messagePositionJumpSlide;

	void Start () {
		PlayerPrefs.DeleteKey("Level1TutStartTriggered");
		PlayerPrefs.DeleteKey("Level1TutJumpSlideTriggered");
		if(!PlayerPrefs.HasKey("Level1TutStartTriggered")) {
			m_arrowMove.SetActive(true);
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Press these buttons to move left and right.", m_messagePositionMove.position, m_arrowMove);
			PlayerPrefs.SetString("Level1TutStartTriggered", "");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(!PlayerPrefs.HasKey("Level1TutJumpSlideTriggered")) {
			m_arrowJumpSlide.SetActive(true);
			HintSystemCanvas.m_singleton.DisplayDefaultHint("Press the left button to slide and the right button to jump!", m_messagePositionJumpSlide.position, m_arrowJumpSlide);
			PlayerPrefs.SetString("Level1TutJumpSlideTriggered", "");
		}
	}
}
