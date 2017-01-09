using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	public GameManager gameManager;
	public Door door;
	public LoadLevel loadlevel;
	public string levelName;

	// void OnTriggerEnter2D(Collider2D other){
	// 	if (other.gameObject.tag == "Player" && door.m_activate){
	// 		gameManager.isLevelOver = true;
	// 		// TODO: Make "level complete" screen which shows score (stars), and 3 buttons
	// 		//	-Replay	- Loads current level
	// 		//	-Home	- Loads main menu scene
	// 		//	-Next	- Loads next level
	// 		//loadlevel.LoadNextLevel(levelName);
	// 	}
	// }


}
