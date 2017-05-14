using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

	//Time settings
	public float timeRate;
	private float nextTime;

	public float distanceOfInteraction;

	private InteractableObject interactObj;

	//Start method
	void Start(){
	
		nextTime = Time.time + timeRate;

	}

	//Update method
	void Update(){

		if (nextTime < Time.time) {
			
			CheckInteractable ();

		}

		if (Input.GetKeyDown(KeyCode.E)) {
			InteractE ();
		}

		if (Input.GetMouseButtonDown(0)) {
			InteractClick ();
		}

	}

	//Checks raycast
	GameObject CheckRaycast(){

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform.tag == "Interactable" && Vector3.Distance(transform.position, hit.transform.position) < distanceOfInteraction) {
				return hit.transform.gameObject;
			}
		}

		return null;

	}
		
	void CheckInteractable(){

		nextTime = Time.time + timeRate;

		GameObject obj = CheckRaycast ();

		if (obj != null) {
			//Gets interactable script from object
			InteractableObject intObj = obj.GetComponent<InteractableObject> ();
			interactObj = intObj;

			if (GameController.instance.hasGift) {
				UIController.instance.interactionText.text = intObj.messageGift;
			} else {
				UIController.instance.interactionText.text = intObj.messageNoGift;
			}

		} else {
			//Changes interaction text to nothing
			UIController.instance.interactionText.text = "";

			interactObj = null;
		}

	}

	//Method called when the key E is pressed
	void InteractE(){

		if (interactObj != null && interactObj.interactE) {
			interactObj.InteractE ();
		}

	}

	//Method called when input is left mouse click
	void InteractClick(){

		if (interactObj != null && interactObj.interactClick) {
			interactObj.InteractClick ();
		}

	}

}
