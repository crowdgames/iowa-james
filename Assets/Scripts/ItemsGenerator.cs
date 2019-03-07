using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsGenerator : MonoBehaviour
{


    public bool isStartingPhase = true;
    public Text locationText;
    public List<GameObject> items = new List<GameObject>();
    GameObject[] sceneItems;

    public Text scoreText;

    public string[] itemsForCurrentlocation;
    // Use this for initialization





    void Start()
    {

        itemsForCurrentlocation = RelevanceManager.DetermineLocation(locationText.text);


        isStartingPhase = false;
        sceneItems = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject sceneItem in sceneItems)
        {
            Instantiate(items[Random.Range(0, items.Count - 1)], sceneItem.transform.position, Quaternion.identity);
        }

    }


}
