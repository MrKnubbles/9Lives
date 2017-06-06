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
	private SpriteRenderer[] headSprites = new SpriteRenderer[3];
	private SpriteRenderer[] earSprites = new SpriteRenderer[2];
	private SpriteRenderer[] armSprites = new SpriteRenderer[2];
	private SpriteRenderer[] legSprites = new SpriteRenderer[2];
	private SpriteRenderer bodySprite;
	private SpriteRenderer tailSprite;
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
		headSprites[0] = headIdle.GetComponent<SpriteRenderer>();
		headSprites[1] = headDie.GetComponent<SpriteRenderer>();
		headSprites[2] = headJump.GetComponent<SpriteRenderer>();
		earSprites[0] = earLeft.GetComponent<SpriteRenderer>();
		earSprites[1] = earRight.GetComponent<SpriteRenderer>();
		armSprites[0] =	armLeft.GetComponent<SpriteRenderer>();
		armSprites[1] = armRight.GetComponent<SpriteRenderer>();
		legSprites[0] = legLeft.GetComponent<SpriteRenderer>();
		legSprites[1] = legRight.GetComponent<SpriteRenderer>();
		bodySprite = body.GetComponent<SpriteRenderer>();
		tailSprite = tail.GetComponent<SpriteRenderer>();
	}

	// Sets the Player's character to the active character selected by swapping the sprites of each body part.
	void SetCharacter(){
		string activeChar = PlayerPrefs.GetString("ActiveChar");
		if (PlayerPrefs.GetString("ActiveChar") == activeChar){
			GameObject activeCat = GameObject.Find("Player/Skins/"+activeChar);
			if(activeCat != null) {
				activeCat.SetActive(true);
				headSprites[0].sprite = activeCat.transform.Find("Head").GetComponent<SpriteRenderer>().sprite;
				headSprites[1].sprite = activeCat.transform.Find("HeadDead").GetComponent<SpriteRenderer>().sprite;
				headSprites[2].sprite = activeCat.transform.Find("HeadJump").GetComponent<SpriteRenderer>().sprite;
				earSprites[0].sprite = activeCat.transform.Find("LeftEar").GetComponent<SpriteRenderer>().sprite;
				earSprites[1].sprite = activeCat.transform.Find("RightEar").GetComponent<SpriteRenderer>().sprite;
				armSprites[0].sprite = activeCat.transform.Find("LeftArm").GetComponent<SpriteRenderer>().sprite;
				armSprites[1].sprite = activeCat.transform.Find("RightArm").GetComponent<SpriteRenderer>().sprite;
				legSprites[0].sprite = activeCat.transform.Find("LeftLeg").GetComponent<SpriteRenderer>().sprite;
				legSprites[1].sprite = activeCat.transform.Find("RightLeg").GetComponent<SpriteRenderer>().sprite;
				bodySprite.sprite = activeCat.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
				tailSprite.sprite = activeCat.transform.Find("Tail").GetComponent<SpriteRenderer>().sprite;
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