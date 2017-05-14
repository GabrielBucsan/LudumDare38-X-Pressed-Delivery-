using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable {

	//String variables
	public string messageNoGift;
	public string messageGift;

	//Boolean variables
	public bool interactE;
	public bool interactClick;

	//Method called when the key E is pressed
	public virtual void InteractE(){



	}

	//Method called when input is left mouse click
	public virtual void InteractClick(){



	}

}
