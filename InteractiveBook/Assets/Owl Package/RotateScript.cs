using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	public float rotation_speed = 5f;
	Transform owl_transform;

	// Use this for initialization
	void Start () 
	{
		owl_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		owl_transform.RotateAround (Vector3.up, rotation_speed * Time.deltaTime);
	}
}
