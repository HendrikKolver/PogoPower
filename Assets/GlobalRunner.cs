using UnityEngine;
using System.Collections;

public class GlobalRunner : MonoBehaviour {

	public PogoController pogoController;


	private static int currentLevel;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		currentLevel = Application.loadedLevel;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.X)) {
			Time.timeScale = 1;
			if(currentLevel !=2)
				Application.LoadLevel(currentLevel+1);
			else
				Application.LoadLevel(0);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			pogoController.returnToSpawnPoint ();
			Time.timeScale = 1;
		}

	
	}

	public static void setCurrentLevel(int level){
		currentLevel = level;
		Time.timeScale = 0.0f;
	}

	public static int getCurrentLevel(){
		return currentLevel;
	}
}
