using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	//Singleton for easy access
	public static AudioController instance;

	//Audioclip variables
	[Header("AudioClips")]
	public AudioClip[] male;
	public AudioClip[] female;

	//AudioSource variable
	AudioSource audioSource;

	//Awake method
	void Awake(){

		//Initiating variables
		instance = this;
		audioSource = GetComponent<AudioSource> ();

	}

	//Method that executes a especific AudioClip
	public void PlaySound(bool _isMale){

		if (_isMale) {
			audioSource.clip = male [Random.Range (0, male.Length)];
		} else {
			audioSource.clip = female [Random.Range(0, female.Length)];
		}

		audioSource.Play ();

	}

}
