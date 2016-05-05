using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject fruit; 
	public GameObject otherAnimal;
	public GameObject animalChild;
	public int amountOfFood = 0;
	public int maxAmountOfFood = 10;

	private GameObject house;
	private GameObject feedingArea;
	private GameObject[] explorePositions;
	private int explorePositionsSize;
	private int randomPosition;
	private float waitTime;
	private float feedTime = 2f;
	private bool isExplorePosition = false;



	void Start () {
		agent = GetComponent<NavMeshAgent>();
		explorePositions = GameObject.FindGameObjectsWithTag ("explore");
		if (explorePositions != null) {
			explorePositionsSize = explorePositions.Length;
			randomPosition = Random.Range (0, explorePositionsSize);
		}
		waitTime = Random.Range(3, 8);
		feedingArea = GameObject.Find ("Feeder");
	}

	void Update() {
		fruit = GameObject.FindGameObjectWithTag ("food");
		if (fruit != null) {
			agent.SetDestination (fruit.transform.position);
		}

		if (amountOfFood >= maxAmountOfFood && otherAnimal != null) {

			agent.SetDestination (otherAnimal.transform.position);
		}

		if (explorePositions != null && fruit == null) {

			agent.SetDestination (explorePositions[randomPosition].transform.position);

			if (isExplorePosition) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0) {
					randomPosition = Random.Range (0, explorePositionsSize);
					waitTime = Random.Range(3, 8);
					isExplorePosition = false;
				}
			}
		}			
	}

	void OnCollisionEnter(Collision collision){

		if (otherAnimal != null && animalChild != null) {
			if (collision.collider == otherAnimal.GetComponent<SphereCollider> () && amountOfFood >= 10) {
				Instantiate (animalChild, transform.position, Quaternion.identity);
				amountOfFood = 0;
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		
		if(collider == explorePositions[randomPosition].GetComponent<SphereCollider>()){
			isExplorePosition = true;
		}

		if (collider == feedingArea.GetComponent<BoxCollider> ()) {
			Destroy (fruit);
			amountOfFood++;
		}
	}

	void OnTriggerStay(Collider collider){
		
		if (collider == feedingArea.GetComponent<BoxCollider> ()) {
			Destroy (fruit, 2f);
			feedTime -= Time.deltaTime;

			if (feedTime <= 0) {
				amountOfFood++;
				feedTime = 2f;
			}
		}
	}
}