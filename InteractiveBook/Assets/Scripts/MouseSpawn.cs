using UnityEngine;
using System.Collections;

public class MouseSpawn : MonoBehaviour {

	public GameObject mouse;

	private Vector3 spawnPosition;

	void Start(){

		spawnPosition = GameObject.Find("MouseSpawn").transform.position;
		spawnPosition.z += 1;
	}

	public void SpawnFood(){

		Instantiate (mouse, spawnPosition, Quaternion.identity);
	}
}