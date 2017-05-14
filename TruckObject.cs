using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckObject : InteractableObject {

	//Method called when the key E is pressed
	public override void InteractE(){
	
		if (!GameController.instance.hasGift) {
			GameController.instance.TakeGiftFromTruck ();
		}

	}

	//Method called when input is left mouse click
	public override void InteractClick(){

		if (!GameController.instance.hasGift) {
			GameController.instance.TakeGiftFromTruck ();
		}

	}

}
