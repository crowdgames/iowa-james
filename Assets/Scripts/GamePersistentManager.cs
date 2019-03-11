using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePersistentManager : MonoBehaviour {

    public static GamePersistentManager Instance;
    public int currentLives = 3;
    public List<string>inventoryItems;
    public List<GameObject> itemsList;
    public int inventoryCount = 0;

    public int relevantItemsCollected = 0;
    public int irrelevantItemsCollected = 0;
    public Vector3 startPosition;

    public List<SceneItemsManager> sceneItemsManager;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class SceneItemsManager
{
    public string scene;
    public int itemsInScene;

}
