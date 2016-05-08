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
	private bool isStarting;

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
		isStarting = true;
	}

	void Update () {

		mouse = GameObject.Find ("Mouse(Clone)");

		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
		} else {
			anim.Play (idle.name);
		}

		if (mouse != null) {
			if (isStarting) {
				Starting ();
			}
			agent.SetDestination (mouse.transform.position);

			if (!IsEating) {
				isExplorePosition = false;
				anim.Play (run.name);
			}
		}

		if (owlExplorePositions != null && mouse == null) {

			if (isStarting) {
				Starting ();
			}
				
			agent.SetDestination (owlExplorePositions [randomPosition].transform.position);			
			if (isExplorePosition) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0) {
					randomPosition = Random.Range (0, owlExplorePositions.Length);
					waitTime = Random.Range (3, 8);
					isExplorePosition = false;
					isStarting = true;
				}
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		
	}

	void OnTriggerEnter (Collider collider) {
		if (collider == owlExplorePositions [randomPosition].GetComponent<SphereCollider> ()) {

		}

		if (mouse!=null && collider == mouse.GetComponent<BoxCollider> ()) {
			Landing ();
		}

	}

	void OnTriggerExit (Collider collider) {
		if (collider == owlExplorePositions [randomPosition].GetComponent<SphereCollider> ()) {
			isStarting = true;
		}
	}

	void OnTriggerStay (Collider collider) {
		if (collider == owlExplorePositions [randomPosition].GetComponent<SphereCollider> ()) {
			Landing ();
		}
		if (mouse!=null && collider == mouse.GetComponent<BoxCollider> ()) {
			Debug.Log ("Collide with Mouse");
			Landing ();
			Destroy (mouse, 2f);
		}
	}

	void Landing(){
		Debug.Log (landingStartingTime);
		landingStartingTime -= Time.deltaTime;
		if (agent.baseOffset <= 0) {
			agent.baseOffset = 0;
			isExplorePosition = true;
			landingStartingTime = 0;
			isStarting = true;
		} else {
			agent.baseOffset = landingStartingTime;

		}
	}

	void Starting(){
		Debug.Log (landingStartingTime);
		landingStartingTime += Time.deltaTime;

		if (agent.baseOffset > 2f) {
			agent.baseOffset = 2;
			isStarting = false;
		} else {
			agent.baseOffset = landingStartingTime;	
		}
	}
}