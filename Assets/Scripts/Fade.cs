using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
    
    public float fadeOutTime;
    private Image fadePanel;
    private Color currentColor = Color.black;
    /*
	// Use this for initialization
	void Start () {
        fadePanel = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad < fadeOutTime)
        {
            float alphaChange = Time.deltaTime / fadeOutTime;
            currentColor.a -= alphaChange;
            fadePanel.color = currentColor;
        }
        else
        {
            gameObject.SetActive(false);
        }
        
	}
    */
    public CanvasGroup cg;

    public void FadeOut()
    {
        StartCoroutine(FadeCo(cg, cg.alpha, 0));
    }

    public IEnumerator FadeCo(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageCompleted = timeSinceStarted / lerpTime;

        while(true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageCompleted = timeSinceStarted / lerpTime;

            float curVal = Mathf.Lerp(start, end, percentageCompleted);

            cg.alpha = curVal;

            if (percentageCompleted >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
    }
    
}
