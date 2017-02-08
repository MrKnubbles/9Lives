using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
	// GameObjects that contains all the items for each category.
	public GameObject charactersSet;
	public GameObject accessoriesSet;
	public GameObject otherSet;
	// Buttons to show each category.
	public GameObject charactersButton;
	public GameObject accessoriesButton;
	public GameObject otherButton;
	// Images to display when a category button is selected.
	public GameObject charactersPressed;
	public GameObject accessoriesPressed;
	public GameObject otherPressed;
	// Window that contains the buttons for different themes.
	public GameObject themesWindow;

	public void CharactersButton(){
		DisableAllCategories();
		charactersSet.SetActive(true);
		EnableAllButtons();
		charactersButton.SetActive(false);
		DisableAllPressedButtons();
		charactersPressed.SetActive(true);
	}

	public void AccessoriesButton(){
		DisableAllCategories();
		accessoriesSet.SetActive(true);
		EnableAllButtons();
		accessoriesButton.SetActive(false);
		DisableAllPressedButtons();
		accessoriesPressed.SetActive(true);
	}

	public void OtherButton(){
		DisableAllCategories();
		otherSet.SetActive(true);
		EnableAllButtons();
		otherButton.SetActive(false);
		DisableAllPressedButtons();
		otherPressed.SetActive(true);
	}

	public void SelectTheme(){
		// TODO: Pass in a string to select theme type.
		themesWindow.SetActive(false);
	}

	public void ThemeButton(){
		themesWindow.SetActive(true);
	}

	public void CloseThemesWindow(){
		themesWindow.SetActive(false);
	}

	void DisableAllCategories(){
		charactersSet.SetActive(false);
		accessoriesSet.SetActive(false);
		otherSet.SetActive(false);
	}

	void EnableAllButtons(){
		charactersButton.SetActive(true);
		accessoriesButton.SetActive(true);
		otherButton.SetActive(true);
	}

	void DisableAllPressedButtons(){
		charactersPressed.SetActive(false);
		accessoriesPressed.SetActive(false);
		otherPressed.SetActive(false);
	}
}
