using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	private GameManager gameManager;
	private AudioManager audioManager;
	public AudioClip sfxCoin;
	public string coinType;
	private Player player;

	void Start(){
		gameManager = GameManager.Instance;
		audioManager = AudioManager.Instance;
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead){
			switch(coinType) {
				case "bronze":
					gameManager.tempCoinCounter += 1;
				break;

				case "silver":
					gameManager.tempCoinCounter += 2;
				break;

				case "gold":
					gameManager.tempCoinCounter += 3;
				break;

				default:
					print("Invalid coin type");
				break;
			}
			audioManager.PlayOnce(sfxCoin);
			gameObject.SetActive(false);
		}
	}
}
