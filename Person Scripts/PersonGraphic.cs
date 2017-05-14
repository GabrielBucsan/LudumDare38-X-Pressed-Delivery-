using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonGraphic : MonoBehaviour {

	[HideInInspector] public SkinnedMeshRenderer rend;

	//Color variables
	public Color hairColor;
	public Color skinColor;

	//Animator
	Animator animator;

	//Awake method
	void Awake(){

		//Sets references and variables
		animator = GetComponent<Animator>();

	}

	//Start method
	void Start(){

		hairColor = LookController.instance.PickHairColor ();
		skinColor = LookController.instance.PickSkinColor ();

		rend = GetComponent<SkinnedMeshRenderer> ();
		rend.material.color = new Color (Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

	}

	//Changes boolean state of animator
	public void ChangeBooleanState(string _name, bool _state){

		animator.SetBool (_name, _state);

	}

	//Plays specific animation from animator
	public void PlayAnimation(string _name){
	
		animator.Play (_name);

	}

}
