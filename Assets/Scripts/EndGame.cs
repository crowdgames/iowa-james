using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	public void QuitButton () {
        Debug.Log("Exiting...");
        Application.Quit();
	}
}
