using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Item : MonoBehaviour {
	private int goldCost;

	void Start () {
		UpdateAccessibility();
	}

	public void UpdateAccessibility(){
		goldCost = Int32.Parse(this.transform.GetChild(3).GetComponent<Text>().text);
		if (PlayerPrefs.GetInt("Coins") >= goldCost){
			transform.GetChild(2).gameObject.SetActive(false);
		}
		else {
			transform.GetChild(2).gameObject.SetActive(true);
		}
	}
}
