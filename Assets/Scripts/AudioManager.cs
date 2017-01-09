using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour {
	private static AudioManager m_instance = null;
	public static AudioManager Instance { get { return m_instance; } }

	public AudioClip musicMain;
	public AudioClip musicLevels;
	public AudioSource audio;
	private bool playOnce = false;
	static bool AudioBegin = false;
	//public GameObject musicPlayer;

	void Awake(){
		//TODO: Make audio sources for music and sounds separate.
		if (m_instance != null && m_instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			m_instance = this;
			m_instance.name = "AudioManager";

			audio = GetComponent<AudioSource>();
			playOnce = false;

			if (!AudioBegin){
				DontDestroyOnLoad(gameObject);
				AudioBegin = true;
			}
		}

		// if (m_instance == null){
		// 	//m_instance = this.gameObject;
		// 	m_instance.name = "AudioManager";

		// 	audio = GetComponent<AudioSource>();
		// 	playOnce = false;

		// 	if (!AudioBegin){
		// 		DontDestroyOnLoad(gameObject);
		// 		AudioBegin = true;
		// 	}
		// }
		// else{
		// 	if (this.gameObject.name != "AudioManager"){
		// 		Destroy(this.gameObject);
		// 	}
		// }
	}
	void Update () {
		if (!AudioBegin){
			audio.Play();
			AudioBegin = true;
		}
		if (playOnce && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
				audio.Stop();
				AudioBegin = false;
				audio.clip = musicLevels;
				playOnce = false;
		}
		else if (!playOnce && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
				audio.Stop();
				AudioBegin = false;
				audio.clip = musicMain;
				playOnce = true;
		}
	}
}