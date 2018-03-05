using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class gameMaster : MonoBehaviour {

    public int points;
    public Text pointsText;
    public Text gameOverText;
    public bool gameOver = false;
    public float levelStartDelay = 2f;

    private int level = 1;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;


    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("levelImage");
        levelText = GameObject.Find("levelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    
    void Update()
    {
        pointsText.text = ("Points: " + points);
       
    }


}
