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
	
	void Awake() {        
        DontDestroyOnLoad(this);
    }

	void Start() {
		if(m_singleton == null) {
			m_singleton = this;
		} else if(m_singleton != this) {
			Destroy(gameObject);
		}
		string tmp = "Derp";
		DisplayDefaultHint(tmp, ScreenPosition.BOTTOM_LEFT);
		DisplayDefaultHint(tmp, ScreenPosition.TOP_LEFT);
		DisplayDefaultHint(tmp, ScreenPosition.BOTTOM_RIGHT);
		DisplayDefaultHint(tmp, ScreenPosition.TOP_RIGHT);
		DisplayDefaultHint(tmp);
	}

	public void DisplayDefaultHint(string hintText, ScreenPosition screenPosition = ScreenPosition.MIDDLE, GameObject objectToHighlight = null) {
		GameObject tmp = Instantiate(m_defaultHintWindow, this.transform);
		RectTransform rectTransform = tmp.GetComponent<RectTransform>();

		tmp.GetComponentInChildren<Text>().text = hintText;

		switch(screenPosition) {
			case ScreenPosition.TOP_LEFT:
				rectTransform.localPosition = new Vector2((rectTransform.rect.width / 2), -(rectTransform.rect.height / 2));
				rectTransform.anchorMin = new Vector2(0,1);
				rectTransform.anchorMax = new Vector2(0,1);
				break;

			case ScreenPosition.TOP_RIGHT:
				rectTransform.localPosition = new Vector2(-(rectTransform.rect.width / 2), -(rectTransform.rect.height / 2));
				rectTransform.anchorMin = new Vector2(1, 1);
				rectTransform.anchorMax = new Vector2(1, 1);
				break;

			case ScreenPosition.BOTTOM_RIGHT:
				rectTransform.localPosition = new Vector2(-(rectTransform.rect.width / 2), (rectTransform.rect.height / 2));
				rectTransform.anchorMin = new Vector2(1,0);
				rectTransform.anchorMax = new Vector2(1,0);
				break;

			case ScreenPosition.BOTTOM_LEFT:
				rectTransform.localPosition = new Vector2((rectTransform.rect.width / 2), (rectTransform.rect.height / 2));
				rectTransform.anchorMin = new Vector2(0,0);
				rectTransform.anchorMax = new Vector2(0,0);

				break;

			case ScreenPosition.MIDDLE:
				rectTransform.anchorMin = new Vector2(0.5f,0.5f);
				rectTransform.anchorMax = new Vector2(0.5f,0.5f);
				break;

			default:
				Debug.Log("Something went wrong. The position specified for the hint window is not valid in method DisplayDefaultHint in HintSystemCanvas.cs");
				break;
		}
		Destroy(tmp, 5);
	}
}
