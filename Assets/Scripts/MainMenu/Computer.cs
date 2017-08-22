using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class Computer : MonoBehaviour {
	public MainMenu mainMenu;
	// Computer
	public GameObject objectWindow;

	// Opens the window for the Computer.
	public void ShowObjectWindow(){
		// TODO: Slide window on to screen over 1 second.
		mainMenu.CloseWindows();
		objectWindow.SetActive(true);
	}

	// Closes the window for the Cat Bed.
	public void CloseObjectWindow(){
		// TODO: Slide window off of screen.
		objectWindow.SetActive(false);
	}
}