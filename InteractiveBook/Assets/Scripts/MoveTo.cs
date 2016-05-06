using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class MoveTo : MonoBehaviour {

	// Public properties

	public GameObject fruit; 
	public GameObject otherAnimal;
	public GameObject animalChild;
	public int amountOfFood = 0;
	public int maxAmountOfFood = 10;

	// Animation name getter.
	public AnimationClip idle;
	public AnimationClip run;
	public AnimationClip cry;
	public AnimationClip jump;
	public AnimationClip fall;

	// Private properties
	private NavMeshAgent agent;
	private GameObject house;
	private GameObject feedingArea;
	private GameObject[] explorePositions;
	private int explorePositionsSize;
	private int randomPosition;
	private float waitTime;
	private float feedTime = 1f;
	private bool isExplorePosition = false;
	private bool IsEating = false;

	//Animation
	private Animator anim;


	void Start () {
		anim = gameObject.GetComponent<Animator> ();
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

		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
		} else {
			anim.Play (idle.name);
		}

		fruit = GameObject.FindGameObjectWithTag ("food");
		if (fruit != null) {
			agent.SetDestination (fruit.transform.position);
			// Run to the food container
			if (!IsEating) {
				isExplorePosition = false;
				anim.Play (run.name);
			}
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
			Destroy (fruit, 1.5f);
			IsEating = true;
			feedTime -= Time.deltaTime;

			if (feedTime <= 0) {
				amountOfFood++;
				feedTime = 2f;
			}
		} else {
			IsEating = false;
		}
	}
}