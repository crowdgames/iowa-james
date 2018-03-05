using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;    //will overlay screen. Here probably a graphic
    public float fadeSpeed = 0.8f;      //speed of fading

    public int drawDepth = -1000;       //texture order in draw hierarchy
    public float alpha = -1f;       //alpha vlaue between 0 and 1
    public int fadeDir = -1;        //direction to fade in=-1 or out =1


	void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        //Set GUI color
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        GUI.depth = drawDepth;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

    }

    //set direction to fading
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    private void OnLevelWasLoaded(int level)
    {
        BeginFade(-1);
    }
}
