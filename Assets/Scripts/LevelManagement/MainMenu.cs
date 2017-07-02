using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public LoadLevel loadLevel;
	public GameObject catBedWindow;

	public void ShowWorldSelect(){
		loadLevel.playerMain.transform.position = loadLevel.startLocation.transform.position;
		loadLevel.playerMain.transform.localScale = loadLevel.startLocation.transform.localScale;
		loadLevel.DisableAllScreens();
		loadLevel.worldSelectScreen.SetActive(true);
		loadLevel.playerLevel.Start();
	}

	public void ShowCatBedWindow(){
		catBedWindow.SetActive(true);
	}

	// Closes all windows.
	public void CloseWindows(){
		// Slide window onto screen over 1 second.
		catBedWindow.SetActive(false);
	}
}