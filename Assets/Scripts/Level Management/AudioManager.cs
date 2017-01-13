using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour {
	private static AudioManager m_instance = null;
	public static AudioManager Instance { get { return m_instance; } }

	public AudioClip musicMain;
	public AudioClip musicLevels;
	public AudioClip sfxTicking;
	public AudioSource music;
	public AudioSource[] sfx;
	private bool playOnce = false;
	static bool AudioBegin = false;
	private DoorSwitch doorSwitch;
	private LoadLevel mainMenu;
	private const int MAX_FXSOURCES = 10;

	void Awake(){
		//TODO: Make audio sources for music and sounds separate.
		if (m_instance != null && m_instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			m_instance = this;
			m_instance.name = "AudioManager";

			// audio = gameObject.AddComponent<AudioSource>();
			// sfx = gameObject.AddComponent<AudioSource>();
			// sfx.playOnAwake = false;
			sfx = new AudioSource[MAX_FXSOURCES];
			InitAudioSources();
			playOnce = false;

			if (!AudioBegin){
				DontDestroyOnLoad(gameObject);
				AudioBegin = true;
			}
		}
	}
	void Update () {
		if (!AudioBegin){
			music.Play();
			AudioBegin = true;
		}
		if (playOnce && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main")){
			mainMenu = GameObject.Find("HUD").GetComponent<LoadLevel>();
			music.Stop();
			AudioBegin = false;
			music.clip = musicLevels;
			playOnce = false;
			UpdateMuteButtons();
			if (GameObject.Find("DoorSwitch") != null){
				doorSwitch = GameObject.Find("DoorSwitch").transform.GetChild(0).GetComponent<DoorSwitch>();
			}
		}
		else if (!playOnce && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			mainMenu = GameObject.Find("HUD").GetComponent<LoadLevel>();
			music.Stop();
			AudioBegin = false;
			music.clip = musicMain;
			playOnce = true;

			UpdateMuteButtons();
		}
	}

	// Creates all the needed AudioSource Components
	private void InitAudioSources() {
		music = gameObject.AddComponent<AudioSource>();
		music.playOnAwake = false;

		for (int i = 0; i < sfx.Length; i++) {
			sfx[i] = gameObject.AddComponent<AudioSource>();
			sfx[i].playOnAwake = false;
		}
	}

	// Find available space in soundfx AudioSources
	// returns : AudioSource that isn't being used
	private AudioSource GetEmptyAudioSource() {
		bool found = false;
		int i = 0;
		AudioSource emptySource = sfx[0];
		do {
			if (!sfx[i].isPlaying && !found) {
				found = true;
				emptySource = sfx[i];
			}
			i++;
		} while (i < sfx.Length && !found);
		return emptySource;
	}

	public void PlayOnce(AudioClip clip) { 
		AudioSource soundFX = GetEmptyAudioSource();
		soundFX.clip = clip;
		soundFX.Play();
	}

	public void PlayLoop(AudioClip clip){
		AudioSource soundFX = GetEmptyAudioSource();
		soundFX.clip = clip;
		soundFX.loop = true;
		soundFX.Play();
	}

	public void MuteMusic(){
		music.mute = !music.mute;
	}

	public void MuteSFX(){
		for (int i = 0; i < sfx.Length; i++) {
			sfx[i].mute = !sfx[i].mute;
		}
	}

	public void PauseSFX(){
		for (int i = 0; i < sfx.Length; i++) {
			sfx[i].Pause();
		}
	}
	
	public void UnpauseSFX(){
		for (int i = 0; i < sfx.Length; i++) {
			sfx[i].UnPause();
		}
	}

	private void UpdateMuteButtons(){
		if (music.mute){
			mainMenu.musicMuted.SetActive(true);
		}
		else{
			mainMenu.musicMuted.SetActive(false);
		}
	}
}