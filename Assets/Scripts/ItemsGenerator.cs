using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemsGenerator : MonoBehaviour
{

    int randomPositionForRelevantItems;
    public bool isStartingPhase = true;
    public Text locationText;
    public List<GameObject> items = new List<GameObject>();
    GameObject[] sceneItems;

    public Text scoreText;

    public string[] itemsForCurrentlocation;
    // Use this for initialization


    List<int> myCount = new List<int>();


    void Start()
    {
        //Debug.Log("Scene Number: "+SceneManager.GetActiveScene().buildIndex);
        itemsForCurrentlocation = RelevanceManager.DetermineLocation(locationText.text);
        sceneItems = GameObject.FindGameObjectsWithTag("Item");

        isStartingPhase = false;

        int relevantItemsCount = GamePersistentManager.Instance.sceneItemsManager[SceneManager.GetActiveScene().buildIndex].itemsInScene;

        for (int j = 0; j < sceneItems.Length; j++)
        {


            while (myCount.Count < 3)
            {
                int randomPositionForRelevantItems = Random.Range(0, 2 * relevantItemsCount);
                if (!myCount.Contains(randomPositionForRelevantItems))
                    myCount.Add(randomPositionForRelevantItems);
            }


            if (myCount.Contains(j))
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    sceneItems[j] = Instantiate(GamePersistentManager.Instance.itemsList[Random.Range(5, 10)],
                        sceneItems[j].transform.position, Quaternion.identity);
                }

            }

            else
            {
                sceneItems[j] = Instantiate(items[Random.Range(0, items.Count - 1)], sceneItems[j].transform.position, Quaternion.identity);

            }

        }

    }


}
