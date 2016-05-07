using UnityEngine;
using System.Collections;

public class FoodSpawn : MonoBehaviour {

	public GameObject food_Apple;

	private int index;
	private Vector3 newPosition;
	private float positionZ;

	void Start(){

		newPosition = transform.position;
	}

	public void SpawnFood(){
		positionZ = Random.Range (16, 21);
		newPosition.z = positionZ;
		Instantiate (food_Apple, newPosition, Quaternion.identity);
	}
}