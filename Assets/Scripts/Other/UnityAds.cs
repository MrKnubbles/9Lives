using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour
{
	private LoadLevel loadLevel;
	[SerializeField] private TV tv;

	void Start(){
		loadLevel = GameObject.Find("HUD").GetComponent<LoadLevel>();
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			tv = GameObject.Find("HUD").transform.GetChild(0).Find("TVObject").GetComponent<TV>();
		}
		Advertisement.Initialize("1201017", true);
	}

	public void ShowAd(string condition){
		ShowOptions options = new ShowOptions();

		// if (condition == "restart"){
		// 	options.resultCallback = AdCallbackHandlerForRestart;
		// }
		// else if (condition == "next"){
		// 	options.resultCallback = AdCallbackHandlerForNext;
		// }
		// else if (condition == "reward"){
		// 	options.resultCallback = AdCallbackHandlerForReward;
		// }

		switch (condition){
			case "restart":
				options.resultCallback = AdCallbackHandlerForRestart;
				break;
			case "next":
				options.resultCallback = AdCallbackHandlerForNext;
				break;
			case "reward":
				print("Reward ad selected.");
				options.resultCallback = AdCallbackHandlerForReward;
				break;
		}

		if (Advertisement.IsReady()){
			Advertisement.Show(options);
		}
	}

	// Used to restart current level after Ad is finished.
	void AdCallbackHandlerForRestart(ShowResult result){
		switch (result){
			case ShowResult.Finished:
				loadLevel.RestartLevelAfterAd();
				break;
			case ShowResult.Skipped:
				loadLevel.LoadMainMenu();
				break;
			case ShowResult.Failed:
				loadLevel.LoadMainMenu();
				break;
		}
	}

	// Used to load next level after Ad is finished.
	void AdCallbackHandlerForNext(ShowResult result){
		switch (result){
			case ShowResult.Finished:
				loadLevel.LoadLevelAfterAd();
				break;
			case ShowResult.Skipped:
				loadLevel.LoadMainMenu();
				break;
			case ShowResult.Failed:
				loadLevel.LoadMainMenu();
				break;
		}
	}

	// Used to grant reward after Ad is finished.
	void AdCallbackHandlerForReward(ShowResult result){
		switch (result){
			case ShowResult.Finished:
				tv.RewardForAd();
				break;
			case ShowResult.Skipped:
				tv.NoRewardForAd();
				break;
			case ShowResult.Failed:
				tv.NoRewardForAd();
				break;
		}
	}
}