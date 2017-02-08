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
	// Default positions for each category set.
	private Vector3 charPos;
	private Vector3 accPos;
	private Vector3 othPos;

	public void SetDefaultShopState(){
		charPos = charactersSet.transform.GetChild(0).transform.GetChild(0).transform.position;
		accPos = accessoriesSet.transform.GetChild(0).transform.GetChild(0).transform.position;
		othPos = otherSet.transform.GetChild(0).transform.GetChild(0).transform.position;
		CharactersButton();
	}

	public void CharactersButton(){
		DisableAllCategories();
		charactersSet.SetActive(true);
		EnableAllButtons();
		charactersButton.SetActive(false);
		DisableAllPressedButtons();
		charactersPressed.SetActive(true);
		charactersSet.transform.GetChild(0).transform.GetChild(0).transform.position = charPos;
	}

	public void AccessoriesButton(){
		DisableAllCategories();
		accessoriesSet.SetActive(true);
		EnableAllButtons();
		accessoriesButton.SetActive(false);
		DisableAllPressedButtons();
		accessoriesPressed.SetActive(true);
		accessoriesSet.transform.GetChild(0).transform.GetChild(0).transform.position = accPos;
	}

	public void OtherButton(){
		DisableAllCategories();
		otherSet.SetActive(true);
		EnableAllButtons();
		otherButton.SetActive(false);
		DisableAllPressedButtons();
		otherPressed.SetActive(true);
		otherSet.transform.GetChild(0).transform.GetChild(0).transform.position = othPos;
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
