using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//Singleton for easy access
	public static GameController instance;

	//Game variables
	[Header("Game variables")]
	public bool hasGift;
	public int giftsDelivered;
	public int highscore;
	public bool isDead;

	//Minimap variables
	[Header("Minimap variables")]
	public Material personMat;
	public Material enemyMat;

	//Audio clips
	[Header("Audio clips")]
	public AudioClip photoClick;

	private AudioSource audioSource;

	//Player variables
	[Header("Player variables")]
	public GameObject giftPosition;
	public Gender playerGender;
	public Orientation playerOrientation;
	[HideInInspector] public GameObject gift;

	//FPC variables
	[Header("First Person Controller Variables")]
	public CharacterController charController;
	public FirstPersonController fpc;

	//Gifts models
	[Header("Gift models")]
	public GameObject[] giftModels;

	//People variables
	[Header("People variables")]
	public GameObject[] peopleInstances;
	public Camera personCamera;

	private GameObject target;
	[HideInInspector] public string actualName;

	//Ex variables
	[Header("Ex variables")]
	public GameObject exCamera;

	Transform cameraTarget;

	//Boolean variables
	bool foundByExActivated;
	bool exCameraPositioned;

	//Reference to the IOMethods class
	IOMethods io;

	//Awake method
	void Awake(){

		//Initiates the class variables
		instance = this;
		Time.timeScale = 0;

		io = GetComponent<IOMethods> ();
		io.Load ();

		Initiate ();

	}

	//Update methos
	void Update(){

		if (foundByExActivated && !exCameraPositioned) {
			if (!exCameraPositioned) {
				MoveExCameraToPosition ();
			}

			if (Vector3.Distance(exCamera.transform.position, cameraTarget.position) < 0.1f) {
				exCameraPositioned = true;

				exCamera.transform.rotation = cameraTarget.rotation;

			}
		}

	}

	//Initiate game
	public void Play(){

		UIController.instance.PlayUI ();

		Time.timeScale = 1;

		if (UIController.instance.genderToggle.isOn) {
			playerGender = Gender.Male;
		} else {
			playerGender = Gender.Female;
		}

		if (UIController.instance.orientationToggle.isOn) {
			if (playerGender == Gender.Male) {
				playerOrientation = Orientation.Heterosexual;
			} else {
				playerOrientation = Orientation.Homosexual;
			}
		} else {
			if (playerGender == Gender.Male) {
				playerOrientation = Orientation.Homosexual;
			} else {
				playerOrientation = Orientation.Heterosexual;
			}
		}

		EnemyManager.instance.StartGame ();

		//Make enemy
		if (EnemyManager.instance.enemies.Count < EnemyManager.instance.maxEnemyCount) {
			for (int i = 0; i < EnemyManager.instance.initialEnemyCount; i++) {

				if (EnemyManager.instance.targetGender == Gender.Male) {
					EnemyManager.instance.MakeEnemy (EnemyManager.instance.malePeople);
				} else {
					EnemyManager.instance.MakeEnemy (EnemyManager.instance.femalePeople);
				}

			}
		}

		charController.enabled = true;
		fpc.enabled = true;

	}

	//Initiates class variables
	public void Initiate(){

		audioSource = GetComponent<AudioSource> ();

		charController.enabled = false;
		fpc.enabled = false;

		actualName = "";
		UIController.instance.UpdateName ();

	}

	//Random selects gift from array of possible gifts
	public GameObject RandomSelectGift(){

		if (giftModels.Length > 0) {
			return giftModels[Random.Range(0, giftModels.Length)];
		}

		return null;

	}

	//Method called when the player loses the game
	public void LostGame(){

		charController.enabled = false;
		fpc.enabled = false;

		isDead = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		bool isHighscore;
		if (giftsDelivered > highscore) {
			isHighscore = true;
			highscore = giftsDelivered;
			io.Save ();
		} else {
			isHighscore = false;
		}

		UIController.instance.GameOver (isHighscore);

		giftsDelivered = 0;

	}

	//Method called to quit the game
	public void QuitGame(){
	
		Application.Quit ();
	
	}

	//Called every time that the player clicks the truck
	public void PickTarget(){

		List<GameObject> peopleList = new List<GameObject> ();

		peopleList = (EnemyManager.instance.targetGender == Gender.Male) ? EnemyManager.instance.malePeople : EnemyManager.instance.femalePeople;

		//Randomly selects target from avaiable people
		target = peopleList[Random.Range(0, peopleList.Count)];
		//target = peopleInstances [Random.Range (0, peopleInstances.Length)];

		//Position camera
		OrdinaryPerson person = target.GetComponent<OrdinaryPerson> ();
		personCamera.transform.position = person.cameraPosition.position;
		personCamera.transform.rotation = person.cameraPosition.rotation;
		personCamera.transform.parent = person.cameraPosition;

		actualName = person.personName;
		UIController.instance.UpdateName ();

		//Swaps from ordinary to target
		person.isTarget = true;
		target.GetComponent<PersonGraphic>().rend.gameObject.layer = 8; //Target Layer

	}

	//Plays an AudioClip
	public void PlaySound(AudioClip _clip){

		audioSource.clip = _clip;
		audioSource.Play ();

	}

	//Method called when player interacts with the truck to take gifts
	public void TakeGiftFromTruck(){

		//Random gift
		GameObject obj = (GameObject) Instantiate (RandomSelectGift (), giftPosition.transform.position, Quaternion.identity);
		obj.transform.parent = giftPosition.transform;

		//Change variables
		hasGift = true;
		gift = obj;
		PlaySound (GameController.instance.photoClick);

		//Sets new target
		PickTarget ();

		//Set ui variables
		string message = "You took " + obj.GetComponent<Gift>().giftName;
		UIController.instance.ShowMessage (message);
		UIController.instance.TakePhoto ();

	}

	//Method called when player interacts with people to deliver gifts
	public void DeliverGift(){

		//Destroys instance of gift
		Destroy (gift);

		//Change gift variables
		hasGift = false;
		giftsDelivered++;

		//Resets target
		ResetTarget();

		//Make enemy
		if (EnemyManager.instance.enemies.Count < EnemyManager.instance.maxEnemyCount) {
			if (EnemyManager.instance.targetGender == Gender.Male) {
				EnemyManager.instance.MakeEnemy (EnemyManager.instance.malePeople);
			} else {
				EnemyManager.instance.MakeEnemy (EnemyManager.instance.femalePeople);
			}
		}

		//Sets UI variables
		string message = "Come back to the truck to take the next gift";
		UIController.instance.ShowMessage (message);
		UIController.instance.ResetPhoto ();
		string txt = "Gifts delivered: " + giftsDelivered;
		UIController.instance.scoreLabel.text = txt;

	}

	//Remove target
	public void ResetTarget(){

		//Resets camera
		personCamera.transform.parent = transform;

		//Swaps from target to ordinary
		target.GetComponent<OrdinaryPerson> ().isTarget = false;
		target.GetComponent<PersonGraphic>().rend.gameObject.layer = 0; //Default layer

		actualName = "";
		UIController.instance.UpdateName ();

	}

	//Resets game for replayability
	public void ResetGame(){

		SceneManager.LoadScene (0);

	}

	//Method called when an ex partner finds the player
	public void FoundByEx(OrdinaryPerson _person){

		if (!foundByExActivated) {

			foundByExActivated = true;
			isDead = true;

			PersonAction pa = _person.GetComponent<PersonAction> ();

			pa.graphic.PlayAnimation ("Angry");

			pa.agent.ResetPath ();
			pa.agent.Stop ();

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			UIController.instance.ExUI ();

			bool isMale = false;

			if (_person.gender == Gender.Male) {
				isMale = true;
			}

			AudioController.instance.PlaySound (isMale);

			if (hasGift) {
				Destroy (gift);
			}

			charController.enabled = false;
			fpc.enabled = false;

			cameraTarget = _person.cameraPosition;
			exCamera.SetActive (true);

		}

	}

	//Moves camera to position smoothly
	void MoveExCameraToPosition(){

		exCamera.transform.position = Vector3.Lerp (exCamera.transform.position, cameraTarget.position, Time.deltaTime);
		exCamera.transform.rotation = Quaternion.Lerp (exCamera.transform.rotation, cameraTarget.rotation, Time.deltaTime);


	}

	//Method that returns the target's gender
	public Gender ReturnTargetGender(){

		if (playerGender == Gender.Male) {
			if (playerOrientation == Orientation.Heterosexual) {
				return Gender.Female;
			} else {
				return Gender.Male;
			}
		} else {
			if (playerOrientation == Orientation.Heterosexual) {
				return Gender.Male;
			} else {
				return Gender.Female;
			}
		}

	}

}

//Enumarator that defines sexual orientation
public enum Orientation 
{
	Heterosexual,
	Homosexual
}