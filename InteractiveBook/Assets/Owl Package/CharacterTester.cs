using UnityEngine;
using System.Collections;

public class CharacterTester : MonoBehaviour {

	public Animator character_animator;
	public Texture[] shirt_texture;
	public Material shirt_material;
	public GameObject shirt_mesh;

	// Use this for initialization
	void Start () 
	{
		character_animator = GetComponent<Animator> ();
	}
	
	public void ShowShirt(int shirt_id)
	{
		shirt_mesh.SetActive (true);
		shirt_material.SetTexture ("_MainTex", shirt_texture [shirt_id]);
	}

	public void NoShirt()
	{
		shirt_mesh.SetActive (false);
	}

	public void PlayAnimation(string trigger_name)
	{
		character_animator.SetTrigger (trigger_name);
	}
}
