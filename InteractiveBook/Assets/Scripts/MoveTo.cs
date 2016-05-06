using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.SocialPlatforms;

public class MoveTo : MonoBehaviour
{

	// Public properties

	/// <summary>
	/// The fruit.
	/// </summary>
	public GameObject fruit;
	/// <summary>
	/// The other animal.
	/// </summary>
	public GameObject otherAnimal;
	/// <summary>
	/// The animal child.
	/// </summary>
	public GameObject animalChild;
	/// <summary>
	/// Initial amount of food.
	/// </summary>
	public int amountOfFood = 0;
	/// <summary>
	/// The max amount of food.
	/// </summary>
	public int maxAmountOfFood = 10;

	// Animation name getter.
	public AnimationClip idle;
	public AnimationClip run;
	public AnimationClip cry;
	public AnimationClip jump;
	public AnimationClip fall;

	// Private properties
	/// <summary>
	/// The navigation mesh agent.
	/// </summary>
	private NavMeshAgent agent;
	/// <summary>
	/// The hous of animalse.
	/// </summary>
	private GameObject house;
	/// <summary>
	/// The feeding area. Place where animals eat.
	/// </summary>
	private GameObject feedingArea;
	/// <summary>
	/// The random explore positions on the map.
	/// </summary>
	private GameObject[] explorePositions;
	/// <summary>
	/// The random position initgar for generating Range().
	/// </summary>
	private int randomPosition;
	/// <summary>
	/// The wait time on random explore position.
	/// </summary>
	private float waitTime = Random.Range (3, 8);
	private float feedTime = 2f;
	private bool isExplorePosition = false;
	private bool IsEating = false;
	private bool isSated = false;

	//Animation
	private Animator anim;


	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		explorePositions = GameObject.FindGameObjectsWithTag ("explore");
		if (explorePositions != null)
			randomPosition = Random.Range (0, explorePositions.Length);
		waitTime = Random.Range (3, 8);
		feedingArea = GameObject.Find ("Feeder");
		house = GameObject.FindGameObjectWithTag ("house");

	}

	void Update () {

		fruit = GameObject.FindGameObjectWithTag ("food");


		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
		} else {
			anim.Play (idle.name);
		}

		if (fruit != null && !isSated) {
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

			agent.SetDestination (explorePositions [randomPosition].transform.position);

			if (isExplorePosition) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0) {
					randomPosition = Random.Range (0, explorePositions.Length);
					waitTime = Random.Range (3, 8);
					isExplorePosition = false;
				}
			}
		}

		if (isSated) {
			agent.SetDestination (house.transform.position);
			
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (otherAnimal != null && animalChild != null) {
			if (collision.collider == otherAnimal.GetComponent<SphereCollider> () && amountOfFood >= 10) {
				
				isSated = true;

				amountOfFood = 0;
			}
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider == explorePositions [randomPosition].GetComponent<SphereCollider> ()) {
			isExplorePosition = true;
		}

		if (collider == house.GetComponent<SphereCollider> ()) {
			isSated = false;
			Instantiate (animalChild, transform.position, Quaternion.identity);
		}

		if (collider == feedingArea.GetComponent<BoxCollider> ()) {
			Destroy (fruit);
			amountOfFood++;
		}
	}

	void OnTriggerExit() {
		IsEating = false;
		isExplorePosition = false;
	}

	void OnTriggerStay (Collider collider) {
		if (collider == feedingArea.GetComponent<BoxCollider> ()) {
			Destroy (fruit, 2f);
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