using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Randomizer {

    public static List<int> levels = new List<int>();
    public static List<string> level_names = 
        new List<string>(){ "Level_01", "Level_02", "Level_03", "Level_04", "Level_05", "Level_06", "Level_07", "Level_08", "Level_09", "Level_10",
        "Level_11", "Level_12", "Level_13"};
    public static bool randomized = true;

    public static void LoadNextLevel()
    {
        if(randomized)
        {
            /*
            if (levels.Count == 0)
            {
                Debug.Log("Inside zero");
                for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    levels.Add(i);
                }
       
            //if (level.Count == 1)
            //    SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
            */
            if (level_names.Count == 0)
                SceneManager.LoadScene("Level_End");

            else
            {
                /*
                //Debug.Log("inside else");
                int index = Random.Range(0, levels.Count);
                int level = levels[index];
                levels.Remove(level);
                SceneManager.LoadScene(level_names[level]);
                Debug.Log("REMOVED: Index: " + index + "\tLevel: " + level);
                /*
                Debug.Log("LEVELS LEFT: ");
                for (int i = 0; i < levels.Count; i++)
                    Debug.Log(levels[i] + "\t");
                    */

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
