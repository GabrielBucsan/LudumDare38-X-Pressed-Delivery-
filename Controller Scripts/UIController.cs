using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	//Singleton for easy access
	public static UIController instance;

	//Panel variables
	[Header("Panel variables")]
	public GameObject gamePanel;
	public GameObject initialPanel;
	public GameObject instructionsPanel;
	public GameObject gameOverPanel;
	public GameObject exPanel;
	public GameObject pausePanel;

	//Game panel variables
	[Header("Game Panel variables")]
	public Text interactionText;
	public Text messageText;
	public Animator messageTxtAnimator;
	public Text scoreLabel;
	public Text nameText;

	//Initial panel variables
	[Header("Initial Panel variables")]
	public Toggle genderToggle;
	public Toggle orientationToggle;

	//Gameover panel variables
	[Header("Gameover Panel variables")]
	public Text scoreText;
	public Text highscoreText;

	//Ex panel variables
	[Header("Ex Panel variables")]
	public Text phraseText;

	//Photo variables
	[Header("Photo variables")]
	public RawImage photo;
	public RenderTexture renderTexture;
	public Rect photoRect;
	private Animator photoAnimator;

	//Awake method
	void Awake(){

		instance = this;

		//Set initial variables
		Initialize();

	}

	//Update method
	void Update(){

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gamePanel.activeInHierarchy) {
				PauseGame ();
			}
		}

	}

	//Initializes UI components
	void Initialize(){

		initialPanel.SetActive (true);
		gameOverPanel.SetActive (false);
		gamePanel.SetActive (false);
		exPanel.SetActive (false);
		pausePanel.SetActive (false);

		photoAnimator = photo.GetComponent<Animator> ();

	}

	//UI called when user starts playing
	public void PlayUI(){

		initialPanel.SetActive (false);
		gamePanel.SetActive (true);

	}

	//Photo effect
	public void TakePhoto(){
	
		photoAnimator.Play ("TakePhotoAnim");

	}

	//Resets photo
	public void ResetPhoto(){

		photoAnimator.Play ("Empty");

	}

	//Shows message
	public void ShowMessage(string _message){

		messageText.text = _message;

		messageTxtAnimator.Play ("FadeAnim");

	}

	//Method called when players loses
	public void GameOver(bool _isHighscore){

		gameOverPanel.SetActive (true);
		gamePanel.SetActive (false);
		initialPanel.SetActive (false);
		exPanel.SetActive (false);

		scoreText.text = "You delivered " + GameController.instance.giftsDelivered + " gifts";
		if (_isHighscore) {
			highscoreText.text = "This is your new highscore!!!";
		} else {
			highscoreText.text = "Your highscore is " + GameController.instance.highscore;
		}

	}

	public void ExUI(){

		gamePanel.SetActive (false);
		exPanel.SetActive (true);

		phraseText.text = "What a small world! " + PhraseManager.instance.GetPhrase();

	}

	public void UpdateName(){

		nameText.text = GameController.instance.actualName;

	}

	void PauseGame(){

		Time.timeScale = 0;
		pausePanel.SetActive (true);
		gamePanel.SetActive (false);

		GameController.instance.charController.enabled = false;
		GameController.instance.fpc.enabled = false;

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

	}

	public void UnpauseGame(){

		pausePanel.SetActive (false);
		gamePanel.SetActive (true);
		Time.timeScale = 1;

		GameController.instance.charController.enabled = true;
		GameController.instance.fpc.enabled = true;

	}
}
