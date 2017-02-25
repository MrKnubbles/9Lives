using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {
	private GameManager gameManager;
	private AudioManager audioManager;
	public AudioClip sfxCoin;
	public string coinType;
	private Player player;
	public GameObject floatingText;
	public MoveObject floatingTextMovement;
	private float alpha;

	void Start(){
		gameManager = GameManager.Instance;
		audioManager = AudioManager.Instance;
		player = GameObject.Find("Player").GetComponent<Player>();
		floatingText = GameObject.Find("HUD/FloatingText");
		floatingTextMovement = floatingText.GetComponent<MoveObject>();
	}

	void Update(){
		// Fades text over time.
		if (floatingTextMovement.isObjectMoving()){
			alpha -= Time.deltaTime / 1.25f;
			floatingText.GetComponent<Text>().color = new Color(1, 1, 1, alpha);
		}
		// Resets alpha to default state and hides text.
		else {
			alpha = 1;
			floatingText.GetComponent<Text>().color = new Color(1, 1, 1, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && !player.isDead && transform.GetChild(0).gameObject.activeSelf){
			switch(coinType) {
				case "bronze":
					gameManager.tempCoinCounter += 1;
					floatingText.GetComponent<Text>().text = "+1";
				break;

				case "silver":
					gameManager.tempCoinCounter += 2;
					floatingText.GetComponent<Text>().text = "+2";
				break;

				case "gold":
					gameManager.tempCoinCounter += 3;
					floatingText.GetComponent<Text>().text = "+3";
				break;

				default:
					print("Invalid coin type");
				break;
			}
			audioManager.PlayOnce(sfxCoin);
			ShowFloatingText();
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	void ShowFloatingText(){
		floatingText.transform.position = transform.position;
		floatingTextMovement.SetSpeed(100);
		floatingTextMovement.SetDistanceY(125);
		floatingTextMovement.Move();
	}
}
