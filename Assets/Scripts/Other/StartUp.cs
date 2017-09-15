using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour {

	float startUpTime = 2f;
	
	void Update () {
		if(startUpTime > 0) {
			startUpTime -=Time.deltaTime;
		} else {
			SceneManager.LoadScene("Main");
		}
	}
}
