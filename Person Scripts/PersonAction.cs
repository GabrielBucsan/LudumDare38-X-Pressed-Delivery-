using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonAction : MonoBehaviour {

	//Person action variables
	[Header("Person action variables")]
	public Vector2 timeIntervalToAction;
	public float walkChance;
	public float sitChance;
	public float idleChance;

	public bool willSit;
	private bool isSeated;
	public Seat seat;

	//Enemy Variables
	[Header("Enemy variables")]
	public bool foundPlayer;
	public float timeToForget;
	[HideInInspector] public float timeSeen;
	public bool isHuntingPlayer;

	GameObject target;

	//Navigation variables
	[HideInInspector] public NavMeshAgent agent;
	bool hasPath;

	private float timeToNextAction;

	[HideInInspector] public PersonGraphic graphic;

	//Awake method
	void Awake(){

		//Initiate references and variables
		agent = GetComponent<NavMeshAgent> ();
		graphic = GetComponent<PersonGraphic> ();
		timeToNextAction = Time.time;

		target = GameObject.FindGameObjectWithTag ("Player").gameObject;

	}

	//Update method
	void Update(){

		//Do next action
		if (!GameController.instance.isDead) {
			if (!foundPlayer) {

				if (timeToNextAction < Time.time) {

					SetNextAction ();

				}

				if (!agent.pathPending) {
					if (agent.remainingDistance <= agent.stoppingDistance) {
						if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
							if (hasPath && !willSit) {
								hasPath = false;

								//Chance animation
								graphic.ChangeBooleanState ("isWalking", false);
								graphic.ChangeBooleanState ("isRunning", false);

							} else if (hasPath && willSit) {
								
								FaceDirection ();

								//Change animation
								graphic.ChangeBooleanState ("isWalking", false);
								graphic.ChangeBooleanState ("Sit", true);
								graphic.ChangeBooleanState ("isRunning", false);

								hasPath = false;
								willSit = false;
								isSeated = true;

							}
						}
					}
				}
			} else {
				if (!isHuntingPlayer) {
					isHuntingPlayer = true;

					agent.ResetPath ();
					agent.speed = 3.5f;

					graphic.ChangeBooleanState ("isWalking", false);
					graphic.ChangeBooleanState ("Sit", false);
					graphic.ChangeBooleanState ("isRunning", true);

					if (seat != null) {
						willSit = false;
						seat.isTaken = false;
						seat = null;
					}
				} else {
					if ((timeSeen + timeToForget) < Time.time) {
						foundPlayer = false;
						isHuntingPlayer = false;
					} else if (Vector3.Distance (transform.position, target.transform.position) < 1.5f) {

						GameController.instance.FoundByEx (GetComponent<OrdinaryPerson>());

					} else {
						HuntPlayer ();
					}
				}			
			}
		}
	}

	//Method that moves the person to player position
	void HuntPlayer(){

		MovePerson (target.transform.position);

	}

	//Kind of Finite State Machine with some behaviours
	void SetNextAction(){

		float chance = Random.Range (0f, 1f);

		//Walk
		if (chance < walkChance) {

			//Sets destination point
			Vector3 waypoint = Random.insideUnitSphere * 20;
			waypoint.y = 0;

			MovePerson (waypoint);

		}else if (chance < (walkChance + sitChance)) {//Sit
			if (!isSeated && !willSit) {
				Sit ();
			}
		}else if (chance < (walkChance + sitChance + idleChance)) {//Idle
			Idle ();
		}

		timeToNextAction = Time.time + Random.Range(timeIntervalToAction.x, timeIntervalToAction.y);

	}

	//Stay idle
	void Idle(){

		if (seat != null && seat.isTaken && !isSeated) {
			seat.isTaken = false;
			willSit = false;
			seat = null;
		}

	}

	//Move person
	void MovePerson(Vector3 _waypoint){

		agent.SetDestination (_waypoint);
		hasPath = true;
		isSeated = false;
		willSit = false;

		if (seat != null && seat.isTaken) {
			seat.isTaken = false;
			seat = null;
		}

		//Change Animation
		if (!isHuntingPlayer) {
			graphic.ChangeBooleanState ("isWalking", true);
			graphic.ChangeBooleanState ("Sit", false);
			graphic.ChangeBooleanState ("isRunning", false);

			if (agent.speed == 3.5f) {
				agent.speed = 2f;
			}
		}

	}

	//Sits person
	void Sit(){

		seat = SeatManager.instance.GetSeat ();

		if (seat != null) {

			agent.SetDestination (seat.seatPosition.position);

			graphic.ChangeBooleanState ("isWalking", true);
			graphic.ChangeBooleanState ("Sit", false);
			graphic.ChangeBooleanState ("isRunning", false);

			willSit = true;
			seat.isTaken = true;
			hasPath = true;

		}

	}

	//Face model to transform
	void FaceDirection(){

		Vector3 pos = new Vector3 (seat.lookAt.position.x, transform.position.y, seat.lookAt.position.z);

		transform.position = seat.seatPosition.position;

		transform.LookAt (pos);

	}

}
