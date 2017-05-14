using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour {

	//Singleton for easy access
	public static NameGenerator instance;

	//Arrary of first names
	string[] maleFirstNames = {"James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Crhistopher", "Daniel", "Paul", "Mark", "Gabriel", "Ygor"};
	string[] femaleFirstNames = {"Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen"};

	//Array of last names
	string[] familyNames = {"Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White"};

	//Awake method
	void Awake(){

		instance = this;

	}

	//Random selects first and last names
	public string GenerateName(bool _isMale){

		string name = "";

		if (_isMale) {
			name = maleFirstNames [Random.Range (0, maleFirstNames.Length)];
		} else {
			name = femaleFirstNames [Random.Range(0, femaleFirstNames.Length)];
		}

		name = name + " " + familyNames [Random.Range (0, familyNames.Length)];

		return name;

	}

}
