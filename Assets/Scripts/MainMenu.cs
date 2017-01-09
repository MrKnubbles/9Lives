using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject levelSelectScreen;
	public GameObject controlsScreen;
	public GameObject mainMenuScreen;
	public LoadLevel loadLevel;
	public void ShowLevelSelect(){
		levelSelectScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void ShowControls(){
		controlsScreen.SetActive(true);
		mainMenuScreen.SetActive(false);
	}

	public void BackButton(){
		levelSelectScreen.SetActive(false);
		controlsScreen.SetActive(false);
		mainMenuScreen.SetActive(true);
	}
}