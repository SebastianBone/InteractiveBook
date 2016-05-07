using UnityEngine;
using System.Collections;

/// <summary>
/// Fox szene controller.
/// </summary>
public class FoxSzeneController : MonoBehaviour
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

	/// <summary>
	/// Animation name for diferent animation names.
	/// Used later as animation name string getter.
	/// </summary>
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
	private float waitTime;
	/// <summary>
	/// The feed time describes how long should animal eat.
	/// </summary>
	private float feedTime = 2f;
	/// <summary>
	/// State of explore position. Determinates if position was or should be visited.
	/// </summary>
	private bool isExplorePosition = false;
	/// <summary>
	/// Boolean flag that determinates if animal has finished eating.
	/// </summary>
	private bool IsEating = false;
	/// <summary>
	/// Boolean flag that determinates if animals are sated.
	/// </summary>
	private bool isSated = false;

	/// <summary>
	/// The animator object for controlling animation.
	/// </summary>
	private Animator anim;

	/// <summary>
	/// Start this instance. Properties initializer.
	/// </summary>
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		explorePositions = GameObject.FindGameObjectsWithTag ("explore");
		if (explorePositions != null)
			randomPosition = Random.Range (0, explorePositions.Length);
		waitTime = Random.Range (3, 8);
		feedingArea = GameObject.Find ("Feeder");
		house = GameObject.FindGameObjectWithTag ("house");
	}

	void Update ()
	{
		
		fruit = GameObject.FindGameObjectWithTag ("food");

		// Play animation should run either the animal is not eating orn isn't on position to explore.
		if (!IsEating && !isExplorePosition) {
			anim.Play (run.name);
			// Otherwise play idle state.
		} else {
			anim.Play (idle.name);
		}

		// Run to the food container if it has some apples AND anima isn't sated AND dont eat at the moment.
		if (fruit != null && !isSated) {
			// Set food container as new position to go to.
			agent.SetDestination (fruit.transform.position);
			// Run to the food container
			if (!IsEating) {
				// Food container is not position to explore.
				isExplorePosition = false;
				anim.Play (run.name);
			}
		}

		if (amountOfFood >= maxAmountOfFood && otherAnimal != null) {
			agent.SetDestination (otherAnimal.transform.position);
		}

		//If there are positions to explore AND food container is empty (no fruit)..
		if (explorePositions != null && fruit == null) {
			// go to random genetated position.
			agent.SetDestination (explorePositions [randomPosition].transform.position);
			// If anima is allready on goal position it should wait there for some time
			if (isExplorePosition) {
				// Decrese wait time on every frame.
				waitTime -= Time.deltaTime;
				// Check if wait delay did end.
				if (waitTime <= 0) {
					// Chose new random position to go to.
					randomPosition = Random.Range (0, explorePositions.Length);
					//Reset wait time.
					waitTime = Random.Range (3, 8);
					// Reset explore position flag for 'run' animation purposes. (See line 108)
					isExplorePosition = false;
				}
			}
		}
		// If the animal cann not carry any more fruits.
		if (amountOfFood >= maxAmountOfFood) {
			isSated = true;
		}
		// If animals are sated, they should go home.
		if (isSated) {
			agent.SetDestination (house.transform.position);
			
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter (Collider collider)
	{
		//Look if there are some exploration places near by.
		if (collider == explorePositions [randomPosition].GetComponent<SphereCollider> ()) {
			//Explore them!
			isExplorePosition = true;
		}

		//If Animal is on the way home sated.
		if (collider == house.GetComponent<BoxCollider> () && amountOfFood >= maxAmountOfFood) {
			// Make some babies.
			Instantiate (animalChild, transform.position, Quaternion.identity);
			// Reset
			amountOfFood = 0;
			isSated = false;
			IsEating = false;
		}
		// Let's eat some apples.
		if (collider == feedingArea.GetComponent<BoxCollider> () && !isSated) {
			//Remove apples from food conteiner after small delay.
			Destroy (fruit, 2f);
			//Increase number of carrying items.
			amountOfFood++;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// Reset boolean flags.
	/// </summary>
	void OnTriggerExit ()
	{
		IsEating = false;
		isExplorePosition = false;
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// Describes animal behaviour in front of food container..
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerStay (Collider collider)
	{
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