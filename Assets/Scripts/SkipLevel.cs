using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipLevel : MonoBehaviour {

    LevelManager lm;
    SkillManager sm;
    Logger log;

	// Use this for initialization
	void Start () {
        lm = GameObject.FindObjectOfType<LevelManager>();
        sm = GameObject.FindObjectOfType<SkillManager>();
        log = GameObject.FindObjectOfType<PlayerController>().GetComponent<Logger>();
	}
	
    public void Skip()
    {
        log.LogMatch("loss");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //StartCoroutine("SkipCo");
    }
    
    
    public IEnumerator SkipCo()
    {
        Debug.Log("SKIPPING LEVEL");
        /*
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        */
        // Randomizer.LoadNextLevel();
        string level = SceneManager.GetActiveScene().name;
        sm.score = 0f;
        sm.score_task = 0f;
        sm.score_game = 0f;
        sm.level = level;
        sm.finished = 0;
        yield return sm.ReportAndRequest();
        try
        {
            string next_level = "";
            if (DataManager.matchmaking == 0)
                next_level = sm.server_data.Substring(sm.server_data.IndexOf("Level"), 10);
            else
                next_level = sm.ParseRequestResponse();
            Debug.Log("Next: " + next_level);
            SceneManager.LoadScene(next_level);
        }
        catch
        {
            if (sm.server_data != "ERROR")
                SceneManager.LoadScene("Level_End");
            else
                sm.server_error = "SkipLevel";
        }
        
    }
}
