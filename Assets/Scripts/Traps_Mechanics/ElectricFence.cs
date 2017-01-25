using UnityEngine;
using System.Collections;

public class ElectricFence : MonoBehaviour {

	public ScoreTracker time;
	public GameObject electricity;

	void Start(){
		time = GameObject.Find("HUD").GetComponent<ScoreTracker>();
		electricity = transform.GetChild(0).gameObject;
	}

	void Update(){
		if (time.GetTime() % 3 == 0){
			electricity.SetActive(true);
		}
		else{
			electricity.SetActive(false);
		}
	}
}
