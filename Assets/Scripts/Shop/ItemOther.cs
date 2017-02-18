using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemOther : MonoBehaviour {
	private int goldCost;
	private int gemCost;
	private int playerGold;
	private int playerGems;
	public bool gemsOnly = false;
	public bool cashOnly = false;

	void Start () {
		if (gemsOnly){
			gemCost = Int32.Parse(this.transform.GetChild(3).GetComponent<Text>().text);
		}
		else if (!cashOnly){
			goldCost = Int32.Parse(this.transform.GetChild(3).GetComponent<Text>().text);
		}
		UpdateAccessibility();
	}

	void Update(){
		CheckForUpdate();
	}

	private void UpdateAccessibility(){
		playerGold = PlayerPrefs.GetInt("Coins");
		playerGems = PlayerPrefs.GetInt("Gems");
		// If the item has not been purchased but player has enough Gems to purchase it.
		if (gemsOnly && PlayerPrefs.GetInt("Gems") >= gemCost){
			SetAsPurchasable();
		}
		// If the item has not been purchased but player has enough Gold to purchase it.
		else if (!gemsOnly && PlayerPrefs.GetInt("Coins") >= goldCost){
			SetAsPurchasable();
		}
		// If the item has not been purchased and player does not have enough Gold or Gems to purchase it.
		else if (!cashOnly){
			SetAsLocked();
		}
	}

	private void CheckForUpdate(){
		if (PlayerPrefs.GetInt("Coins") != playerGold || PlayerPrefs.GetInt("Gems") != playerGems){
			UpdateAccessibility();
		}
	}

	private void SetAsPurchasable(){
		this.transform.GetChild(1).gameObject.SetActive(true);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(false);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(true);		// Item cost
	}

	private void SetAsLocked(){
		this.transform.GetChild(1).gameObject.SetActive(false);		// Purchase Button
		this.transform.GetChild(2).gameObject.SetActive(true);		// Locked item image
		this.transform.GetChild(3).gameObject.SetActive(true);		// Item cost
	}
}
