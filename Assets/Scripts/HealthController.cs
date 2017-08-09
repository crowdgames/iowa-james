using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    public GameObject[] heart = new GameObject[5];
    public PlayerController player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < heart.Length; i++)
        {
            heart[i].SetActive(false);
        }

		for(int i = 0; i < player.curHealth; i++)
        {
            heart[i].SetActive(true);
        }
    }
}
