using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {

	public int amountOfFood;
	public int maxAmountOfFood = 100;
	bool buttonPressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (buttonPressed) {
			FillFood ();
		}
	}

	public void FillFood(){

		if (amountOfFood <= maxAmountOfFood) {
			
			amountOfFood += 1;
		} 
		if (amountOfFood == maxAmountOfFood) {
			buttonPressed = false;
		}
	}

	public void FillButtonPressed(){
		buttonPressed = true;
	}

	public int getAmountOfFood(){
		return amountOfFood;
	}

	public void DecreaseFood(){

		amountOfFood -= 1;
	}
}
