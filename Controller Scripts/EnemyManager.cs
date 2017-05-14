using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	//Enemy manager variables
	[Header("Enemy manager variables")]
	public int initialEnemyCount;
	public int maxEnemyCount;

	public Gender targetGender;

	//Singleton for easy acess
	public static EnemyManager instance;

	//Lists of Transform and GameObjects
	[HideInInspector] public List<Transform> isVisibleFrom = new List<Transform>();
	[HideInInspector] public List<GameObject> malePeople = new List<GameObject>();
	[HideInInspector]public List<GameObject> femalePeople = new List<GameObject> ();
	[HideInInspector] public List<GameObject> enemies = new List<GameObject>();

	//Awake method
	void Awake(){

		instance = this;

	}

	//Method that initializates the game
	public void StartGame(){

		targetGender = GameController.instance.ReturnTargetGender ();

		//Makes sure there is client to deliver gift
		if (maxEnemyCount > GameController.instance.peopleInstances.Length - 1) {
			maxEnemyCount = GameController.instance.peopleInstances.Length - 1;
		}

		//Initiate lists
		foreach (var item in GameController.instance.peopleInstances) {

			if (item.GetComponent<OrdinaryPerson> ().gender == Gender.Male) {
				malePeople.Add (item);
			} else {
				femalePeople.Add (item);
			}

		}

	}

	//Method that creates an enemy and manages the lists
	public void MakeEnemy(List<GameObject> _list){

		bool a = true;

		while (a) {
			GameObject enemy = _list [Random.Range (0, _list.Count)];

			if (!enemies.Contains(enemy)) {
				enemies.Add (enemy);
				_list.Remove (enemy);

				OrdinaryPerson op = enemy.GetComponent<OrdinaryPerson> ();

				op.isEx = true;
				enemy.GetComponentInChildren<FieldOfView> ().enabled = true;

				op.mapIcon.GetComponent<Renderer> ().material = GameController.instance.enemyMat;

				a = false;
			}
		}

	}

}
