using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;
	public GameObject fruit;

	void Start () {
		agent = GetComponent<NavMeshAgent>();

	}

	void Update() {
		
		fruit = GameObject.FindGameObjectWithTag ("food");	
		agent.SetDestination (fruit.transform.position);			
	}

	void OnCollisionEnter(Collision c) {
		if (c.collider.tag == "food") {
			Vector3 newPosition = target.position;
			newPosition.x *= -1;
			newPosition.z *= -1;
			target.position = newPosition;
			Debug.Log("collide");
		}
		
	}
		
}
