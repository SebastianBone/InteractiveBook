using UnityEngine;
using System.Collections;

public class MouseSpawn : MonoBehaviour {

	public GameObject mouse;
	private GameObject owlScene;
	private GameObject mouseClone;

	private Vector3 spawnPosition;

	void Start(){
		owlScene = GameObject.Find ("Owl_scene");
		spawnPosition = GameObject.Find("MouseSpawn").transform.position;
		spawnPosition.z += 1;
	}

	public void SpawnFood(){

		mouseClone = Instantiate (mouse, spawnPosition, Quaternion.identity) as GameObject;
		mouseClone.transform.SetParent (owlScene.transform);
	}
}