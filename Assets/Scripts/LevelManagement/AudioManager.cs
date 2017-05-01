using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {
	private static AudioManager m_instance = null;
	public static AudioManager Instance { get { return m_instance; } }
	public AudioClip[] musicLevels;
	public AudioSource music;
	public AudioSource[] sfx;
	private bool playOnce = false;
	static bool AudioBegin = false;
	private DoorSwitch doorSwitch;
	private LoadLevel mainMenu;
	private const int MAX_FXSOURCES = 10;

	void Awake(){
		if (m_instance != null && m_instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			m_instance = this;
			m_instance.name = "AudioManager";
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
			// Choose which song to play based on which World the player is in.
			if (Int32.Parse(SceneManager.GetActiveScene().name) <= 45){
				music.clip = musicLevels[1];
			}
			else if (Int32.Parse(SceneManager.GetActiveScene().name) > 45 && Int32.Parse(SceneManager.GetActiveScene().name) <= 90){
				music.clip = musicLevels[2];
				music.volume = .5f;
			}
			music.loop = true;
			playOnce = false;
			if (GameObject.Find("DoorSwitch") != null){
				doorSwitch = GameObject.Find("DoorSwitch").transform.GetChild(0).GetComponent<DoorSwitch>();
			}
		}
		else if (!playOnce && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")){
			mainMenu = GameObject.Find("HUD").GetComponent<LoadLevel>();
			music.Stop();
			AudioBegin = false;
			music.clip = musicLevels[0];
			music.loop = true;
			playOnce = true;
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
		} while (i < (sfx.Length - 1) && !found);
		return emptySource;
	}

	public void PlayOnce(AudioClip clip) { 
		AudioSource soundFX = GetEmptyAudioSource();
		soundFX.clip = clip;
		soundFX.Play();
	}

	public void PlayLoop(AudioClip clip){
		// Used ONLY for the ticking sound on the TimedSwitches.
		// Sets it to the last array location so it can be targeted and stopped.
		sfx[sfx.Length - 1].clip = clip;
		sfx[sfx.Length - 1].loop = true;
		sfx[sfx.Length - 1].Play();
	}

	public void MuteMusic(){
		music.mute = !music.mute;
		int musicMuted = PlayerPrefs.GetInt("MusicMuted");
		PlayerPrefs.SetInt("MusicMuted", musicMuted * -1);
	}

	public void MuteSFX(){
		for (int i = 0; i < sfx.Length; i++) {
			sfx[i].mute = !sfx[i].mute;
		}
		int sfxMuted = PlayerPrefs.GetInt("SFXMuted");
		PlayerPrefs.SetInt("SFXMuted", sfxMuted * -1);
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
	public void StopSFX(){
		for (int i = 0; i < sfx.Length; i++) {
			sfx[i].Stop();
		}
	}
}