using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour {

	// Use this for initialization
	public void GoToGame() {
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
