using UnityEngine;
using System.Collections;

public class ElectricFence : MonoBehaviour {
	public ScoreTracker time;
	public GameObject electricity;
	public AudioClip sfxElectricity;
	private AudioManager audioManager;
	public bool playOnce = false;

	void Start(){
		audioManager = AudioManager.Instance;
		time = GameObject.Find("HUD").GetComponent<ScoreTracker>();
		electricity = transform.GetChild(0).gameObject;
	}

	void Update(){
		if (time.GetTime() % 3 == 0){
			if (time.GetTime() != time.maxTime && !playOnce){
				electricity.SetActive(true);
				audioManager.PlayOnce(sfxElectricity);
				playOnce = true;
			}
		}
		else{
			electricity.SetActive(false);
			playOnce = false;
		}
	}
}
