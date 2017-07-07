using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public LoadLevel loadLevel;
	public CatBed catBed;
	
	// Closes the Main Menu and brings the player to the World / Level Select Screen.
	public void ShowWorldSelect(){
		loadLevel.playerMain.transform.position = loadLevel.startLocation.transform.position;
		loadLevel.playerMain.transform.localScale = loadLevel.startLocation.transform.localScale;
		loadLevel.DisableAllScreens();
		loadLevel.worldSelectScreen.SetActive(true);
		loadLevel.playerLevel.Start();
	}

	// Closes all windows.
	public void CloseWindows(){
		// TODO: Slide window off of screen over 1 second.
		catBed.objectWindow.SetActive(false);
	}

	// Gets all upgrade ranks by retrieving PlayerPrefs.
	public void GetUpgradeRanks(){
		catBed.GetPrefs();
	}

	// Initializes the upgrade ranks for each object.
	// This should only be done once upon logging in for the first time.
	public void InitializeUpgradeRanks(){
		catBed.SetPrefs();
	}
}