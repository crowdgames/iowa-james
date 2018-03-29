using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Randomizer {

    public static List<int> levels = new List<int>();
    public static bool randomized = true;

    Randomizer()
    {
        Debug.Log("Inside randomizer");
        
    }

    public static void LoadNextLevel()
    {
        if(randomized)
        {
            if (levels.Count == 0)
            {
                Debug.Log("Inside zero");
                for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    levels.Add(i);
                }
            }
            if (levels.Count == 1)
                SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
            else
            {
                Debug.Log("inside else");
                int index = Random.Range(0, levels.Count - 1);
                int level = levels[index];
                levels.Remove(level);
                SceneManager.LoadScene(level);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
