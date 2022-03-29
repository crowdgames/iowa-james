using UnityEngine;
using UnityEngine.UI;

public class DJumpPowerUp : MonoBehaviour {

    //  Stuff for timer bar
    public Image TimerBar;
    public float IncreaseAmount;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Update () {

        //TimerBar.fillAmount -= IncreaseAmount * Time.deltaTime;

        //if (TimerBar.fillAmount <= 0f)
        //{

        //}
    }
}
