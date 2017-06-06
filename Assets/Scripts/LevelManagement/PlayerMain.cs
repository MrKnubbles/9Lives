using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class PlayerMain : MonoBehaviour {

	public GameObject testAcc;
	public Animator animator;
	// Player Body Parts
	public GameObject headIdle;
	public GameObject headJump;
	public GameObject headDie;
	public GameObject earLeft;
	public GameObject earRight;
	public GameObject armLeft;
	public GameObject armRight;
	public GameObject legLeft;
	public GameObject legRight;
	public GameObject body;
	public GameObject tail;
	// Sprite of each body part
	private Image[] headSprites = new Image[3];
	private Image[] earSprites = new Image[2];
	private Image[] armSprites = new Image[2];
	private Image[] legSprites = new Image[2];
	private Image bodySprite;
	private Image tailSprite;
	public Vector3 pos;
	public Vector3 facing;
	
    public void Start(){
		animator = GetComponent<Animator>();
		GetSpriteRenderers();
		SetCharacter();
		SetHeads();
		animator.SetBool("isGameStarted", true);
	} 

	public void AttachHeadAccessory(GameObject accessory) {
		GameObject acc1 = Instantiate(accessory, headIdle.transform);
		GameObject acc2 = Instantiate(accessory, headDie.transform);
		GameObject acc3 = Instantiate(accessory, headJump.transform);
		acc1.transform.localPosition = new Vector3(0,0,0);
		acc2.transform.localPosition = new Vector3(0,0,0);
		acc3.transform.localPosition = new Vector3(0,0,0);
	}

	void GetSpriteRenderers(){
		headSprites[0] = headIdle.GetComponent<Image>();
		headSprites[1] = headDie.GetComponent<Image>();
		headSprites[2] = headJump.GetComponent<Image>();
		earSprites[0] = earLeft.GetComponent<Image>();
		earSprites[1] = earRight.GetComponent<Image>();
		armSprites[0] =	armLeft.GetComponent<Image>();
		armSprites[1] = armRight.GetComponent<Image>();
		legSprites[0] = legLeft.GetComponent<Image>();
		legSprites[1] = legRight.GetComponent<Image>();
		bodySprite = body.GetComponent<Image>();
		tailSprite = tail.GetComponent<Image>();
	}

	// Sets the Player's character to the active character selected by swapping the sprites of each body part.
	void SetCharacter(){
		string activeChar = PlayerPrefs.GetString("ActiveChar");
		if (PlayerPrefs.GetString("ActiveChar") == activeChar){
			GameObject activeCat = GameObject.Find("Player/Skins/"+activeChar);
			if(activeCat != null) {
				activeCat.SetActive(true);
				headSprites[0].sprite = activeCat.transform.Find("Head").GetComponent<Image>().sprite;
				headSprites[1].sprite = activeCat.transform.Find("HeadDead").GetComponent<Image>().sprite;
				headSprites[2].sprite = activeCat.transform.Find("HeadJump").GetComponent<Image>().sprite;
				earSprites[0].sprite = activeCat.transform.Find("LeftEar").GetComponent<Image>().sprite;
				earSprites[1].sprite = activeCat.transform.Find("RightEar").GetComponent<Image>().sprite;
				armSprites[0].sprite = activeCat.transform.Find("LeftArm").GetComponent<Image>().sprite;
				armSprites[1].sprite = activeCat.transform.Find("RightArm").GetComponent<Image>().sprite;
				legSprites[0].sprite = activeCat.transform.Find("LeftLeg").GetComponent<Image>().sprite;
				legSprites[1].sprite = activeCat.transform.Find("RightLeg").GetComponent<Image>().sprite;
				bodySprite.sprite = activeCat.transform.Find("Body").GetComponent<Image>().sprite;
				tailSprite.sprite = activeCat.transform.Find("Tail").GetComponent<Image>().sprite;
				activeCat.SetActive(false);
			}
		}
	}

	void SetHeads(){
		headIdle.SetActive(true);
		headDie.SetActive(false);
		headJump.SetActive(false);	
	}
}