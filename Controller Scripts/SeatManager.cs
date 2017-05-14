using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour {

	//Singleton for easy access
	public static SeatManager instance;

	//Array of all seats in scene
    public Seat[] seats;

	//List of avaiable seats
	private List<Seat> avaiableSeats;

	//Awake method
	void Awake(){

		//Initializes class variables
		instance = this;
		seats = FindObjectsOfType<Seat> ();
		avaiableSeats = new List<Seat> ();

	}

	//Get random avaiable seat
	public Seat GetSeat(){

		GetAvaiableSeats ();

		if (avaiableSeats.Count > 0) {
			return avaiableSeats [Random.Range (0, avaiableSeats.Count)];
		} else {
			return null;
		}
	}

	//Populates list of avaiable seats
	void GetAvaiableSeats(){

		avaiableSeats.Clear ();

		foreach (var item in seats) {
			if (!item.isTaken) {
				avaiableSeats.Add (item);
			}
		}

	}
}
