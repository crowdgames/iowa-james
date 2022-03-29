using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Randomizer {
    
    public static List<string> level_names = 
        new List<string>(){ "Level_01", "Level_02", "Level_03", "Level_04", "Level_05", "Level_06", "Level_07", "Level_08", "Level_09", "Level_10",
        "Level_11", "Level_12", "Level_13"};
    public static bool randomized = false;

    public static void LoadNextLevel()
    {
        if(randomized)
        {
            if (level_names.Count == 0)
                SceneManager.LoadScene("Level_End");

            else
            {
                int index = Random.Range(0, level_names.Count);
                string level = level_names[index];
                level_names.Remove(level);
                SceneManager.LoadScene(level);
                Debug.Log("Loaded: " + level);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
