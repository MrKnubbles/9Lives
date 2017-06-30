using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public PlayerMain player;
	private MovePlayer playerMovement;
	private string playerDirection;
	private bool isPlayerMoving = false;
	private Vector3 playerFacing;
	private Vector2 moveLocation;
	private float moveDistance;
	public GameObject worldSelectScreen;
	public GameObject levelSelectScreen;
	public GameObject controlsScreen;
	public GameObject mainMenuScreen;
	public LoadLevel loadLevel;

	// Sets the location for the player to move to on the WorldSelectScreen.
	public void SetPlayerMoveLocation(int objectNumber){
		// int roundedValue = RoundDown(worldNumber);
		// int firstDigit = roundedValue/10;
		// int secondDigit = worldNumber - roundedValue;
		// Move location is the building you selected.
		// moveLocation = GameObject.Find("HUD/WorldSelectScreen/WorldsScrollView/Viewport/WorldsSet").transform.GetChild((firstDigit * 3) - (4 - secondDigit)).transform.position;
		moveLocation = GameObject.Find("HUD/MainMenuScreen").transform.GetChild(objectNumber).transform.position;
		moveDistance = (moveLocation.x - player.transform.position.x);
		// worldLevelNumber = worldNumber;
		isPlayerMoving = true;
		// Makes player run to the right.
		if (player.transform.position.x < moveLocation.x){
			playerDirection = "right";
			player.animator.SetBool("isRunning", true);
			if (playerFacing.x < 0){
				playerFacing.x *= -1;
				player.transform.localScale = playerFacing;
			}
		}
		// Makes player run to the left.
		else if (player.transform.position.x > moveLocation.x){
			playerDirection = "left";
			player.animator.SetBool("isRunning", true);
			if (playerFacing.x > 0){
				playerFacing.x *= -1;
				player.transform.localScale = playerFacing;
			}
		}
		else {
			playerDirection = "none";
			player.animator.SetBool("isRunning", false);
		}
	}

	int RoundDown(int toRound)
	{
		return toRound - toRound % 10;
	}
}