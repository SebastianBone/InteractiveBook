using UnityEngine;
using System.Collections;


public class MouseController : MonoBehaviour {

	public GameObject [] mouseExplorepoints;
	private int randomPosition;
	private NavMeshAgent agent;
	private GameObject owl;
	private float waitTime;
	bool isExplorePosition;
	bool isCatched = false;


	void Start () {
		waitTime = Random.Range (5, 10);
		agent = GetComponent<NavMeshAgent>();
		mouseExplorepoints = GameObject.FindGameObjectsWithTag ("mouseExplore");
		isExplorePosition = false;
		randomPosition = Random.Range(0, mouseExplorepoints.Length);
		owl = GameObject.FindGameObjectWithTag ("owl");
		GotoNextPoint();
	}

	void Update () {
		
		GotoNextPoint ();
	}

	void GotoNextPoint() {

		if (mouseExplorepoints.Length == 0) {
			return;
		}
			
		if (!isCatched) {
			agent.SetDestination (mouseExplorepoints [randomPosition].transform.position);

			if (isExplorePosition) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0) {
					randomPosition = Random.Range (0, mouseExplorepoints.Length);
					waitTime = Random.Range (5, 10);
					isExplorePosition = false;
				}
			}
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider == mouseExplorepoints [randomPosition].GetComponent<SphereCollider> ()) {
			isExplorePosition = true;
		}
		if (collider == owl.GetComponent<BoxCollider> ()) {
			agent.SetDestination (transform.position);
			isCatched = true;
		}
	}
}