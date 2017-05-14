using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseManager : MonoBehaviour {

	//Singleton for easy access
	public static PhraseManager instance;

	//General Phrases
	[Header("General Phrases")]
	public string[] generalPhrases;

	//Awake method
	void Awake(){

		instance = this;

	}

	//Get random phrase
	public string GetPhrase(){

		return generalPhrases [Random.Range (0, generalPhrases.Length)];

	}

}
