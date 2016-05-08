using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


	public void LoadFoxScene(){
		SceneManager.LoadScene ("develop");
	}

	public void LoadOwlScene(){
		SceneManager.LoadScene ("owl_Scene");
	}
}
