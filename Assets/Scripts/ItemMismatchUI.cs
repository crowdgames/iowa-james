using UnityEngine;
using UnityEngine.UI;

public class ItemMismatchUI : MonoBehaviour
{

    public Text currentScenario;
    public Text feedbackText;
    // Use this for initialization
    void Start()
    {
        feedbackText.text = "This is an irrelevant item to the " + currentScenario.text;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
