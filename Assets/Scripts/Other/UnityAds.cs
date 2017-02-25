using UnityEngine;
using UnityEngine.Advertisements;


public class UnityAds : MonoBehaviour
{
	private LoadLevel loadLevel;

	void Start(){
		loadLevel = GameObject.Find("HUD").GetComponent<LoadLevel>();
		Advertisement.Initialize("1201017", true);
	}

	public void ShowAd(string condition){
		ShowOptions options = new ShowOptions();

		if (condition == "restart"){
			options.resultCallback = AdCallbackHandlerForRestart;
		}
		else if (condition == "next"){
			options.resultCallback = AdCallbackHandlerForNext;
		}

		if (Advertisement.IsReady()){
			Advertisement.Show(options);
		}
	}

	// Used to restart current level after ad is finished.
	void AdCallbackHandlerForRestart(ShowResult result){
		switch (result){
			case ShowResult.Finished:
				loadLevel.RestartLevelAfterAd();
				break;
			case ShowResult.Skipped:
				loadLevel.RestartLevelAfterAd();
				break;
			case ShowResult.Failed:
				loadLevel.RestartLevelAfterAd();
				break;
		}
	}

	// Used to load next level after ad is finished.
	void AdCallbackHandlerForNext(ShowResult result){
		switch (result){
			case ShowResult.Finished:
				loadLevel.LoadLevelAfterAd();
				break;
			case ShowResult.Skipped:
				loadLevel.LoadLevelAfterAd();
				break;
			case ShowResult.Failed:
				loadLevel.LoadLevelAfterAd();
				break;
		}
	}
}