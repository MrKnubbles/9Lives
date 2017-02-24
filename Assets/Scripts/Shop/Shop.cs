using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
	// Player's money
	public Text goldText;
	public Text gemsText;
	// Confirmation window.
	public GameObject confirmationWindow;
	// Info of item being purchased.
	private string selectedItem;
	private int selectedCost;
	private int selectedValue;
	private Purchaser purchaser;

	void Start(){
		goldText.text = PlayerPrefs.GetInt("Coins").ToString();
		gemsText.text = PlayerPrefs.GetInt("Gems").ToString();
		purchaser = GetComponent<Purchaser>();
	}

	public void PurchaseItem(string itemName){
		selectedItem = itemName;
		// Cash purchases
		if (selectedItem == "Gems" || selectedItem == "RemoveAds"){
			string selectedCashCost = GameObject.Find("" + selectedItem).transform.GetChild(3).GetComponent<Text>().text;
			selectedValue = Int32.Parse(GameObject.Find("" + selectedItem).transform.GetChild(6).GetComponent<Text>().text);
			confirmationWindow.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Are you sure you want to purchase " + selectedValue + " " + selectedItem + " for " + selectedCashCost + "?";
		}
		// Gem purchases
		else if (selectedItem == "Gold"){
			selectedCost = Int32.Parse(GameObject.Find("" + selectedItem).transform.GetChild(3).GetComponent<Text>().text);
			selectedValue = Int32.Parse(GameObject.Find("" + selectedItem).transform.GetChild(6).GetComponent<Text>().text);
			confirmationWindow.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Are you sure you want to purchase " + selectedValue + " " + selectedItem + " for " + selectedCost + " Gems?";
		}
		// Gold purchases
		else{
			selectedCost = Int32.Parse(GameObject.Find("" + selectedItem).transform.GetChild(3).GetComponent<Text>().text);
			confirmationWindow.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Are you sure you want to purchase " + selectedItem + " for " + selectedCost + " Gold?";
		}
		confirmationWindow.SetActive(true);
	}

	public void AcceptPurchase(){
		if (selectedItem == "Gems"){
			purchaser.Buy5Gems();
		}
		if (selectedItem == "RemoveAds"){
			purchaser.BuyRemoveAds();
		}
		else if (selectedItem == "Gold"){
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + selectedValue);
			PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - selectedCost);
		}
		else {
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - selectedCost);
			PlayerPrefs.SetInt("" + selectedItem, PlayerPrefs.GetInt("" + selectedItem) + 1);
			SelectChar(selectedItem);
		}
		confirmationWindow.SetActive(false);
		UpdateItemAvailability();
	}

	public void JumpToGemPurchase(){
		OtherButton();
		PurchaseItem("Gems");
	}

	public void SelectChar(string itemName){
		PlayerPrefs.SetString("ActiveChar", "" + itemName);
	}

	public void CancelPurchase(){
		confirmationWindow.SetActive(false);
	}

	private void UpdateItemAvailability(){
		goldText.text = PlayerPrefs.GetInt("Coins").ToString();
		gemsText.text = PlayerPrefs.GetInt("Gems").ToString();
	}

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
