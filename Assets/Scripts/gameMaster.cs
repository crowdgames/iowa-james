using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameMaster : MonoBehaviour {

    public int points;
    public Text pointsText;
    public Text gameOverText;
    public bool gameOver = false;


    void Update()
    {
        pointsText.text = ("Points: " + points);
        if (gameOver)
        {
            gameOverText.text = ("GAME OVER");
        }
    }
}
