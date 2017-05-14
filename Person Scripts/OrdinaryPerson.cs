using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinaryPerson : InteractableObject {

	public Transform cameraPosition;
	public bool isTarget;
	public bool isEx;
	public GameObject mapIcon;

	public Gender gender;
	public string personName;

	//Start method
	void Start(){

		mapIcon.GetComponent<Renderer> ().material = GameController.instance.personMat;

		bool isMale;

		if (gender == Gender.Male) {
			isMale = true;
		} else {
			isMale = false;
		}

		personName = NameGenerator.instance.GenerateName (isMale);

	}

	//Method called when input is left mouse click
	public override void InteractClick(){
		if (GameController.instance.hasGift) {
			if (isTarget) {
				GameController.instance.DeliverGift ();
			} else if (isEx) {
				GameController.instance.FoundByEx (this);
			} else {
				GameController.instance.LostGame ();
			}
		}

	}

}

//Enumarator that defines gender
public enum Gender
{
	Male,
	Female
}
