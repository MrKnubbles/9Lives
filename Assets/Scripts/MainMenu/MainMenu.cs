using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public LoadLevel loadLevel;
	[SerializeField] GameObject confirmationWindow;
	[SerializeField] GameObject adConfirmationWindow;
	public CatBed catBed;
	public Fridge fridge;
	public Wardrobe wardrobe;
	public Bank bank;
	public TV tv;
	public Computer computer;
	[SerializeField] private Shop shop;
	
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
		bank.CloseObjectWindow();
		tv.CloseObjectWindow();
		computer.CloseObjectWindow();
	}

	// Gets all upgrade ranks by retrieving PlayerPrefs.
	public void GetUpgradeRanks(){
		catBed.GetPrefs();
		fridge.GetPrefs();
		bank.GetPrefs();
		tv.GetPrefs();
	}

	// Initializes the upgrade ranks for each object.
	// This should only be done once upon logging in for the first time.
	public void InitializeUpgradeRanks(){
		catBed.SetPrefs();
		fridge.SetPrefs();
		bank.SetPrefs();
		tv.SetPrefs();
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

	public void PromptAdConfirmation(){
		adConfirmationWindow.SetActive(true);
	}

	public void CloseWatchAdWindow(){
		adConfirmationWindow.SetActive(false);
	}

	public void UpdateCurrencies(){
		shop.UpdateCurrencies();
	}
}