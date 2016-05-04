using UnityEngine;
using System.Collections;

public class FoodSpawn : MonoBehaviour {

	public Transform food_Apple;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnFood(){

		Instantiate (food_Apple, transform.position, Quaternion.identity);
	
	}
}
