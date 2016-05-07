using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.SocialPlatforms;

public class OwlController: MonoBehaviour
{

	// Public properties

	/// <summary>
	/// The mouse.
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
	private GameObject[] owlExplorePositions;
	/// <summary>
	/// The random position integar for generating Range().
	/// </summary>
	private int randomPosition;
	/// <summary>
	/// The wait time on random explore position.
	/// </summary>
	private float waitTime;
	private float landingStartingTime = 2f;
	private bool isExplorePosition = false;
	private bool IsEating = false;
	private bool isStartingLanding;

	//Animation
	private Animator anim;


	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.baseOffset = 0;
		owlExplorePositions = GameObject.FindGameObjectsWithTag ("owlExplore");
		if (owlExplorePositions != null)
			randomPosition = Random.Range (0, owlExplorePositions.Length);
		waitTime = Random.Range (3, 8);
		isStartingLanding = true;
	}

	void Update () {

		mouse = GameObject.FindGameObjectWithTag ("mouse");


		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
		} else {
			anim.Play (idle.name);
		}

		if (mouse != null) {
			agent.SetDestination (mouse.transform.position);

			if (!IsEating) {
				isExplorePosition = false;
				anim.Play (run.name);
			}
		}

		if (owlExplorePositions != null && mouse == null) {

			if (isStartingLanding) {
				landingStartingTime += Time.deltaTime;

				Debug.Log (agent.baseOffset);
				if (agent.baseOffset > 2f) {
					agent.baseOffset = 2;
					isStartingLanding = false;
				} else {
					agent.baseOffset = landingStartingTime;	
				}
			}
				
			agent.SetDestination (owlExplorePositions [randomPosition].transform.position);			
			if (isExplorePosition) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0) {
					randomPosition = Random.Range (0, owlExplorePositions.Length);
					waitTime = Random.Range (3, 8);
					isExplorePosition = false;
					isStartingLanding = true;
				}
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
//		if (mouse != null && collision.collider == mouse.GetComponent<BoxCollider> ()) {
//			agent.height = 1;
//			agent.Stop ();
//			amountOfFood++;
//		}
	}

	void OnTriggerEnter (Collider collider) {

	}

	void OnTriggerStay (Collider collider) {
		if (collider == owlExplorePositions [randomPosition].GetComponent<SphereCollider> ()) {


			landingStartingTime -= Time.deltaTime;
			if (agent.baseOffset <= 0) {
				agent.baseOffset = 0;
				isExplorePosition = true;
				isStartingLanding = false;
			} else {
				agent.baseOffset = landingStartingTime;
			
			}
		}
	}
}