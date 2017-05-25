using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class Player : MonoBehaviour {

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
	//
	private GameObject masksContainer;
	[SerializeField]
	private List<GameObject> masks;
	public GameObject bloodSplatPrefab;
	private Quaternion bloodRot = new Quaternion();
	public GameObject respawnPos;
	public GameObject bloodParticle;
	public Door exitDoor;
	public bool isDead = false;
	public bool isJumping = false;
	public bool isFalling = false;
	public bool isSliding = false;
	public bool isFacingRight = true;
	public bool hasDoubleJumped = false;
    public float jumpSpeed = 15.0f;
	public float moveSpeed = 1.5f;
	//public float lives = 9;
	
	[SerializeField] PlayerCanvas playerCanvas;
	[SerializeField] GameObject playerCanvasGO;
	[SerializeField] GameObject playerCanvasPrefab;
	float invulnerableTimer = 2.0f;
	float maxInvulnerableTimer = 2.0f;
	bool isInvulnerable = false;
	public float lives = 9;
	[SerializeField] HealthBarCanvas healthBarCanvas;
	[SerializeField] GameObject healthBarCanvasGO;
	[SerializeField] GameObject healthBarCanvasPrefab;
	private float invulnerableTimer;
	private float maxInvulnerableTimer = .5f;
	public bool isInvulnerable = false;
	public bool isStunned = false;
	public GameManager gameManager;
	public bool isGrounded = true;
	public AudioClip sfxDie;
	public AudioClip sfxDoubleJump;
	public AudioClip sfxHit;
	public AudioClip sfxJump;
	public AudioClip sfxSlam;
	public AudioManager audioManager;
	public Rigidbody2D rb2d;
	public PlayerMovement action;
	public Vector3 pos;
	public Vector3 facing;
	private float slideTimer = 0;
	// Needed for activating a switch.
	public bool isNearSwitch = false;
	public bool isActivatingSwitch = false;
	public GameObject HUD;

	// Getters and Setters
	public PlayerCanvas GetPlayerCanvas() { return playerCanvas; }
	
    void Start(){
		InitPlayerCanvas();
		animator = GetComponent<Animator>();
		GetSpriteRenderers();
		SetCharacter();
		SetHeads();	
		InitLayerMasks();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb2d = GetComponent<Rigidbody2D>();
		HUD = GameObject.Find("HUD");
		audioManager = AudioManager.Instance;
		gameManager.isGameStarted = true;
		gameManager.isLevelComplete = false;
		gameManager.isGameOver = false;
		animator.SetBool("isGameStarted", true);
		gameManager.isLevelComplete = false;
		exitDoor = GameObject.Find("ExitDoor/Door").GetComponent<Door>();
		respawnPos = GameObject.Find("Respawn");
		invulnerableTimer = maxInvulnerableTimer;
	} 
	void Update() {
		if (!gameManager.isLevelComplete && !gameManager.isPaused){
			HandleHeads();
			//UpdateHealthRegeneration();
			if (!isDead){
				UpdateInvulnerability();
				if (isSliding){
					slideTimer += Time.deltaTime;
					if (slideTimer >= .75f){
						ResetSlide();
					}
				}
				if (!isStunned){
					if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
						action.MoveRight();
					}
					if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
						action.MoveLeft();
					}
					if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))){
						action.Jump();
					}
					if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding){
						action.Special();
					}
					if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
						action.StopRunning();
					}
				}
				else{
					action.StopRunning();
				}
				if (rb2d.velocity.y < -0.1f && !isFalling){
					SetFalling();
				}
			}
		}
		if ((Input.GetKeyUp(KeyCode.L) && gameManager.isLevelComplete)){
			HUD.GetComponent<LoadLevel>().ReplayLevel();
		}
		if ((Input.GetKeyUp(KeyCode.R) && gameManager.isLevelComplete)){
			HUD.GetComponent<LoadLevel>().LoadNextLevel();
		}

		// TODO: Remove. Testing only
		if(Input.GetKeyDown(KeyCode.M)) {
			AttachHeadAccessory(testAcc);
		}

		
	}

	// void UpdateHealthRegeneration() {
	// 	if(currentHealthRegenTime > 0) {
	// 		currentHealthRegenTime -= Time.deltaTime;
	// 	} else {
	// 		health += 1;
	// 		UpdateHealthBar();
	// 		currentHealthRegenTime = healthRegenInterval;
	// 		lastHealthRegenTime = System.DateTime.Now;
	// 	}
	// }

	public void ResetSlide(){
		animator.SetBool("isSliding", false);
		GetComponent<CircleCollider2D>().enabled = true;
		slideTimer = 0;
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		isSliding = false;
	}

	void TakeDamage(float damage) {
		// call this to activate invulnerability for a duration
		// to prevent spamming of damage and FX
		isInvulnerable = true;

		// This is to check if the player is already dead
		// If they are then return because we don't 
		// care about the rest
		float health = playerCanvas.GetHealth();
		if(health <= 0) {
			return;
		} else {	
			playerCanvas.TakeDamage(damage);
			health = playerCanvas.GetHealth();
			// check if that last hit killed the player and if 
			// so then call Die
			if(health <= 0) {
				Die();
			// if you're not dead from the last hit then play some FX	
			} else {	
				SpawnBlood();
				audioManager.PlayOnce(sfxDie);
			}
		}
	}

	void UpdateInvulnerability() {
		if(isInvulnerable) {
			if(invulnerableTimer >= 0) {
				invulnerableTimer -= Time.deltaTime;
				DamageFlash();
			} else {
				isInvulnerable = false;
				isStunned = false;
				invulnerableTimer = maxInvulnerableTimer;
			}
		}
	}

	void DamageFlash(){
		if (invulnerableTimer > maxInvulnerableTimer * .8f){
			MakeInvisible(true);
		}
		else if(invulnerableTimer > maxInvulnerableTimer * .6f){
			MakeInvisible(false);
		}
		else if(invulnerableTimer > maxInvulnerableTimer * .4f){
			MakeInvisible(true);
		}
		else if (invulnerableTimer > maxInvulnerableTimer * .2f){
			MakeInvisible(false);
		}
		else if (invulnerableTimer > 0){
			MakeInvisible(true);
		}
		else{
			MakeInvisible(false);
		}
	}

	void MakeInvisible(bool isInvisible){
		if (isInvisible){
			headSprites[0].color = new Color(1f, 0f, 0f, .25f);
			headSprites[1].color = new Color(1f, 0f, 0f, .25f);
			headSprites[2].color = new Color(1f, 0f, 0f, .25f);
			earSprites[0].color = new Color(1f, 0f, 0f, .25f);
			earSprites[1].color = new Color(1f, 0f, 0f, .25f);
			armSprites[0].color = new Color(1f, 0f, 0f, .25f);
			armSprites[1].color = new Color(1f, 0f, 0f, .25f);
			legSprites[0].color = new Color(1f, 0f, 0f, .25f);
			legSprites[1].color = new Color(1f, 0f, 0f, .25f);
			bodySprite.color = new Color(1f, 0f, 0f, .25f);
			tailSprite.color = new Color(1f, 0f, 0f, .25f);
		}
		else{
			headSprites[0].color = new Color(1f, 1f, 1f, 1f);
			headSprites[1].color = new Color(1f, 1f, 1f, 1f);
			headSprites[2].color = new Color(1f, 1f, 1f, 1f);
			earSprites[0].color = new Color(1f, 1f, 1f, 1f);
			earSprites[1].color = new Color(1f, 1f, 1f, 1f);
			armSprites[0].color = new Color(1f, 1f, 1f, 1f);
			armSprites[1].color = new Color(1f, 1f, 1f, 1f);
			legSprites[0].color = new Color(1f, 1f, 1f, 1f);
			legSprites[1].color = new Color(1f, 1f, 1f, 1f);
			bodySprite.color = new Color(1f, 1f, 1f, 1f);
			tailSprite.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	public void Die(){
		playerCanvas.Die();
		SpawnBlood();
		isDead = true;
		audioManager.PlayOnce(sfxDie);
		animator.SetBool("isDead", true);
		isActivatingSwitch = false;
		int lives = playerCanvas.GetLives();
		if (lives <= 0) {
			gameManager.isGameOver = true;
		}
	}

	void InitPlayerCanvas() {
		playerCanvasGO = GameObject.Find("PlayerCanvas");
		if(playerCanvasGO == null) {
			GameObject tmp = GameObject.Instantiate(playerCanvasPrefab);
			tmp.name = "PlayerCanvas";
			playerCanvas = tmp.GetComponent<PlayerCanvas>();
		} else {
			playerCanvas = playerCanvasGO.GetComponent<PlayerCanvas>();
		}
	}

	void InitLayerMasks() {
		masksContainer = GameObject.Find("Masks");
		foreach(RectTransform g in masksContainer.transform) {
			masks.Add(g.gameObject);
		}
	}

	public void AttachHeadAccessory(GameObject accessory) {
		GameObject acc1 = Instantiate(accessory, headIdle.transform);
		GameObject acc2 = Instantiate(accessory, headDie.transform);
		GameObject acc3 = Instantiate(accessory, headJump.transform);
		acc1.transform.localPosition = new Vector3(0,0,0);
		acc2.transform.localPosition = new Vector3(0,0,0);
		acc3.transform.localPosition = new Vector3(0,0,0);
	}

	void Respawn(){
		float lives = playerCanvas.GetLives();
		if (lives != 0){
			playerCanvas.Respawn();
			isDead = false;
			isJumping = false;
			hasDoubleJumped = false;
			isFalling = false;
			isSliding = false;
			isGrounded = true;
			animator.SetBool("isJumping", false);
			animator.SetBool("isFalling", false);
			animator.SetBool("isSliding", false);
			animator.SetBool("isRunning", false);
			animator.SetBool("isDead", false);
			rb2d.velocity = new Vector2(0, 0);
			GetComponent<CircleCollider2D>().enabled = true;
			transform.position = respawnPos.transform.position;
			transform.localScale = respawnPos.transform.localScale;
			isInvulnerable = false;
			isStunned = false;
			invulnerableTimer = maxInvulnerableTimer;
		}
	}

	void HidePlayer(){
		headSprites[0].sortingLayerName = "Hidden";
		headSprites[1].sortingLayerName = "Hidden";
		headSprites[2].sortingLayerName = "Hidden";
		earSprites[0].sortingLayerName = "Hidden";
		earSprites[1].sortingLayerName = "Hidden";
		armSprites[0].sortingLayerName = "Hidden";
		armSprites[1].sortingLayerName = "Hidden";
		legSprites[0].sortingLayerName = "Hidden";
		legSprites[1].sortingLayerName = "Hidden";
		bodySprite.sortingLayerName = "Hidden";
		tailSprite.sortingLayerName = "Hidden";
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

	void OnCollisionEnter2D(Collision2D other){
		if ((other.gameObject.tag == "Trap" || other.gameObject.tag == "TriggerTrap") && !isDead){
			if(!isInvulnerable) {
				TrapStats stats = other.gameObject.GetComponent<TrapStats>();
				if(stats != null) {
					float damage = stats.damage;
					if(damage > 0) {
						TakeDamage(damage);
					} else {
						Die();
					}
				}
			}
		}
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = other.transform;
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform"){
			transform.parent = null;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "TriggerTrap") {
			if(!isInvulnerable) {
				TrapStats stats = other.gameObject.GetComponent<TrapStats>();
				if(stats != null) {
					float damage = stats.damage;
					if(damage > 0) {
						TakeDamage(damage);
					} else {
						Die();
					}
				}
			}
		}
		if (other.gameObject.tag == "TriggerKill" && !isDead){
			Die();
		}
		if (other.gameObject.tag == "Exit" && !isDead && exitDoor.isActive){
			// Hides player behind the exit door and stops time.
			HidePlayer();
			Time.timeScale = 0;
			gameManager.isLevelComplete = true;
		}
	}

	void SpawnBlood() {
		//Vector3 offsetY = new Vector3(0, -0.5f, 0);
		Vector3 spawnPosition = this.transform.position;
		GameObject tmpBlood;
		tmpBlood = GameObject.Instantiate(bloodParticle, spawnPosition, Quaternion.identity);
		foreach(GameObject g in masks) {
			//offsetY = new Vector3(0, -1f, 0);
			GameObject tmpBloodSplat;
			tmpBloodSplat = Instantiate(bloodSplatPrefab, g.transform, false);
			tmpBloodSplat.transform.position = this.transform.position;
		}

		int random = Random.Range(0, 360);
		bloodRot = Quaternion.Euler(0, 0, random);
		bloodSplatPrefab.transform.rotation = bloodRot;
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

	public void SetGrounded(){
		animator.SetBool("isJumping", false);
		animator.SetBool("isFalling", false);
		isGrounded = true;
		isJumping = false;
		isFalling = false;
		hasDoubleJumped = false;
	}

	public void SetFalling(){
		animator.SetBool("isJumping", false);
		animator.SetBool("isSliding", false);
		animator.SetBool("isFalling", true);
		isGrounded = false;
		isJumping = false;
		isFalling = true;
	}

	private void HandleHeads() {
		if(isJumping && !isDead || isFalling && !isDead || isSliding && !isDead || hasDoubleJumped && !isDead) {
			headJump.SetActive(true);			
			headDie.SetActive(false);
			headIdle.SetActive(false);	
		} else if(isDead) {		
			headDie.SetActive(true);
			headJump.SetActive(false);	
			headIdle.SetActive(false);	
		} else {
			headIdle.SetActive(true);
			headJump.SetActive(false);	
			headDie.SetActive(false);			
		}
	}
}