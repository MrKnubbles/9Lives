using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ScreenPosition {
	NONE,
	TOP_LEFT,
	TOP_RIGHT,
	BOTTOM_RIGHT,
	BOTTOM_LEFT,
	MIDDLE
}
public class HintSystemCanvas : MonoBehaviour {	

	public static HintSystemCanvas m_singleton = null;
	[SerializeField] GameObject m_defaultHintWindow;
	[SerializeField] GameObject m_highlightArrow;
	GameObject m_highlightedObject = null;
	GameObject m_owl = null;
	[SerializeField] GameObject m_owlPrefab;
	
	void Awake() {        
        DontDestroyOnLoad(this);
    }

	void Start() {
		if(m_singleton == null) {
			m_singleton = this;
		} else if(m_singleton != this) {
			Destroy(gameObject);
		}
		// TODO: Debug Only!!
		// string tmp = "Derp";
		// DisplayDefaultHint(tmp, ScreenPosition.BOTTOM_LEFT);
		// DisplayDefaultHint(tmp, ScreenPosition.TOP_LEFT);
		// DisplayDefaultHint(tmp, ScreenPosition.BOTTOM_RIGHT);
		// DisplayDefaultHint(tmp, ScreenPosition.TOP_RIGHT);
		// DisplayDefaultHint(tmp);
	}

	public void DisplayDefaultHint(string hintText, Vector3 messagePos = default(Vector3), GameObject objectToHighlight = null) {

		m_highlightedObject = objectToHighlight;
		RectTransform rectTransform = m_defaultHintWindow.GetComponent<RectTransform>();
		m_defaultHintWindow.GetComponentInChildren<Text>().text = hintText;
		m_defaultHintWindow.SetActive(true);
		Vector3 owlPos = new Vector3(messagePos.x - 1.2f, messagePos.y - 0.8f, messagePos.z);
		m_owl = Instantiate(m_owlPrefab, owlPos, Quaternion.identity);
		rectTransform.position = messagePos;
		GameManager.Instance.isPaused = true;
	}

	public void ResumePlay() {
		m_defaultHintWindow.SetActive(false);
		if(m_owl != null) {
			//m_owl = null;
			Destroy(m_owl.gameObject);
		}
		if(m_highlightedObject != null) {
			m_highlightedObject.SetActive(false);
		}
		GameManager.Instance.isPaused = false;
	}
}
