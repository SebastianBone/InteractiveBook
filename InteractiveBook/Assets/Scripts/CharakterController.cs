using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharakterController : MonoBehaviour {

	public float moveTime = 4f;
	public float turnTime = 4f;
	public float movespeed = 1f;
	public float turnspeed = 10f;

	private float movingTime;
	private float turningTime;

	private GameObject food;

	int random;

	void Start() {
		movingTime = moveTime;
		turningTime = turnTime;
		random = Random.Range (-1, 2);
		food = GameObject.Find("Food");
	}

	void Update() {

		if (food.GetComponent<FoodController> ().getAmountOfFood () > 0) {

			transform.LookAt (food.transform);
			transform.Translate (Vector3.forward * movespeed * Time.deltaTime);
		}

		else {
			moveTime -= Time.deltaTime;

			if (moveTime <= 0) {

				turnTime -= Time.deltaTime;

				RotateObject ();

				if (turnTime <= 0) {
					ResetValues();
				}

			} 
			else {
				transform.Translate (Vector3.forward * movespeed * Time.deltaTime);	
			}
		}
	}

	void OnTriggerStay(){
		if (food.GetComponent<FoodController> ().getAmountOfFood () > 0) {
			food.GetComponent<FoodController> ().DecreaseFood ();
		}
	}

	private void ResetValues(){
		moveTime = movingTime;
		turnTime = turningTime;
		random = Random.Range(-1, 2);
	}

	private void RotateObject(){
		
		transform.Rotate (Vector3.up, turnspeed * random * Time.deltaTime);
	}		
}