using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class Wardrobe : MonoBehaviour {
	public MainMenu mainMenu;
	public PlayerCanvas playerStats;
	// Wardrobe
	public GameObject objectWindow;
	public Shop shopScreen;

	// Opens the window for the Wardrobe.
	public void ShowObjectWindow(){
		// TODO: Slide window on to screen over 1 second.
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
		shopScreen.GetComponent<Shop>().SetDefaultShopState();
	}

	// Closes the window for the Wardrobe.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}
}