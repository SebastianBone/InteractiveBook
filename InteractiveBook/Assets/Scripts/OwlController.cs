using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.SocialPlatforms;

public class OwlController: MonoBehaviour
{

	// Public properties

	/// <summary>
	/// The fruit.
	/// </summary>
	public GameObject mouse;
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
	//private float feedTime = 2f;
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

	}

	void Update () {

		mouse = GameObject.FindGameObjectWithTag ("mouse");


		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
		} else {
			anim.Play (idle.name);
		}

		if (mouse != null && !isSated) {
			agent.SetDestination (mouse.transform.position);
			// Run to the food container
			if (!IsEating) {
				isExplorePosition = false;
				anim.Play (run.name);
			}
		}

		if (explorePositions != null && mouse == null) {

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

		//if (isSated) {
		//	agent.SetDestination (house.transform.position);
		//}
	}

	void OnCollisionEnter (Collision collision) {
		
	}

	void OnTriggerEnter (Collider collider) {
		if (collider == explorePositions [randomPosition].GetComponent<SphereCollider> ()) {
			isExplorePosition = true;
		}

		//if (collider == feedingArea.GetComponent<BoxCollider> ()) {
		//	Destroy (fruit);
		//	amountOfFood++;
		//}
	}

	void OnTriggerExit() {
		IsEating = false;
		isExplorePosition = false;
	}

//	void OnTriggerStay (Collider collider) {
//		if (collider == feedingArea.GetComponent<BoxCollider> ()) {
//			Destroy (fruit, 2f);
//			IsEating = true;
//			feedTime -= Time.deltaTime;
//
//			if (feedTime <= 0) {
//				amountOfFood++;
//				feedTime = 2f;
//			}
//		} else {
//			IsEating = false;
//		}
//	}
}