using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Item : MonoBehaviour {
	private int goldCost;
	private int playerGold;
	private string activeChar;


	void Start () {
		goldCost = Int32.Parse(this.transform.GetChild(3).GetComponent<Text>().text);
		UpdateAccessibility();
		playerGold = PlayerPrefs.GetInt("Coins");
		activeChar = PlayerPrefs.GetString("ActiveChar");
	}

	void Update(){
		CheckForUpdate();
	}

	private void UpdateAccessibility(){
		// If the item is selected.
		if (PlayerPrefs.GetString("ActiveChar") == "" + this.transform.GetChild(6).GetComponent<Text>().text){
			SetAsSelected();
		}
		// If the item has been purchased.
		else if (PlayerPrefs.GetInt("" + this.transform.GetChild(6).GetComponent<Text>().text) == 1){
			SetAsSelectable();
		}
		// If the item has not been purchased but player has enough gold to purchase it.
		else if (PlayerPrefs.GetInt("Coins") >= goldCost){
			SetAsPurchasable();
		}
		// If the item has not been purchased and player does not have enough gold to purchase it.
		else {
			SetAsLocked();
		}
	}

	private void CheckForUpdate(){
		if (PlayerPrefs.GetInt("Coins") != playerGold || PlayerPrefs.GetString("ActiveChar") != activeChar){
			UpdateAccessibility();
			playerGold = PlayerPrefs.GetInt("Coins");
			activeChar = PlayerPrefs.GetString("ActiveChar");
		}
	}

	private void SetAsSelected(){
		this.transform.GetChild(1).gameObject.SetActive(false);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(false);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(false);		// Item cost
		this.transform.GetChild(4).gameObject.SetActive(false);		// Select Char Button
		this.transform.GetChild(5).gameObject.SetActive(true);		// Selected Char image
		//print("active char is = " + PlayerPrefs.GetString("ActiveChar"));
	}

	private void SetAsSelectable(){
		this.transform.GetChild(1).gameObject.SetActive(false);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(false);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(false);		// Item cost
		this.transform.GetChild(4).gameObject.SetActive(true);		// Select Char Button
		this.transform.GetChild(5).gameObject.SetActive(false);		// Selected Char image
	}

	private void SetAsPurchasable(){
		this.transform.GetChild(1).gameObject.SetActive(true);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(false);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(true);		// Item cost
		this.transform.GetChild(4).gameObject.SetActive(false);		// Select Char Button
		this.transform.GetChild(5).gameObject.SetActive(false);		// Selected Char image
	}

	private void SetAsLocked(){
		this.transform.GetChild(1).gameObject.SetActive(false);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(true);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(true);		// Item cost
		this.transform.GetChild(4).gameObject.SetActive(false);		// Select Char Button
		this.transform.GetChild(5).gameObject.SetActive(false);		// Selected Char image
	}
}
