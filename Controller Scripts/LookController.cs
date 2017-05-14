using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour {

	//Singleton for easy access
	public static LookController instance;

	//Color variables
	public Color[] hairColors;
	public Color[] skinColors;

	//Awake method
	void Awake(){
	
		instance = this;

	}

	//Random selects hair color from array
	public Color PickHairColor(){

		return hairColors [Random.Range (0, hairColors.Length)];

	}

	//Random selects skin color from array
	public Color PickSkinColor(){

		return skinColors [Random.Range (0, skinColors.Length)];

	}

}
