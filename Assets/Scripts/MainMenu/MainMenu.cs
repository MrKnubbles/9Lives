using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public LoadLevel loadLevel;
	[SerializeField] GameObject confirmationWindow;
	public CatBed catBed;
	public Fridge fridge;
	public Wardrobe wardrobe;
	
	// Closes the Main Menu and brings the player to the World / Level Select Screen.
	public void ShowWorldSelect(){
		catBed.SavePrefs();
		fridge.SavePrefs();
		if (loadLevel.playerCanvas.GetLives() > 0){
			loadLevel.playerMain.transform.position = loadLevel.startLocation.transform.position;
			loadLevel.playerMain.transform.localScale = loadLevel.startLocation.transform.localScale;
			loadLevel.DisableAllScreens();
			loadLevel.worldSelectScreen.SetActive(true);
			loadLevel.playerLevel.Start();
		}
	}

	// Closes all windows.
	public void CloseWindows(){
		catBed.CloseObjectWindow();
		fridge.CloseObjectWindow();
		wardrobe.CloseObjectWindow();
	}

	// Gets all upgrade ranks by retrieving PlayerPrefs.
	public void GetUpgradeRanks(){
		catBed.GetPrefs();
		fridge.GetPrefs();
	}

	// Initializes the upgrade ranks for each object.
	// This should only be done once upon logging in for the first time.
	public void InitializeUpgradeRanks(){
		catBed.SetPrefs();
		fridge.SetPrefs();
	}

	// Opens to Upgrade Confirmation window for a specific object.
	// Set the int to the child (of confirmationWindow) you want to open.
	public void PromptUpgradeConfirmation(int childNumber){
		confirmationWindow.SetActive(true);
		confirmationWindow.transform.GetChild(childNumber).gameObject.SetActive(true);
	}

	public void CloseConfirmationWindow(){
		int max = confirmationWindow.transform.childCount;
		for (int i = 1; i < max; i++){
			confirmationWindow.transform.GetChild(i).gameObject.SetActive(false);
		}
		confirmationWindow.SetActive(false);
	}
}