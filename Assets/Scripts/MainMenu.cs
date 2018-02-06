using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button btn_start;
    public Button btn_info;
    public Button btn_play;
    public Button btn_back;
    public Button btn_back2;
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public static string username;

	// Use this for initialization
	void Start () {
        CanvasSwitch(1);

        btn_start.onClick.AddListener(StartButton);
        btn_info.onClick.AddListener(InfoButton);
        btn_play.onClick.AddListener(PlayButton);
        btn_back.onClick.AddListener(BackButton);
        btn_back2.onClick.AddListener(BackButton2);
    }

    void StartButton()
    {
        CanvasSwitch(2);
    }

    void InfoButton()
    {
        CanvasSwitch(3);
    }

    void PlayButton()
    {
        InputField input = canvas2.GetComponentInChildren<InputField>();
        username = input.text;
        SceneManager.LoadScene(1);
    }

    void BackButton()
    {
        CanvasSwitch(1);
    }

    void BackButton2()
    {
        CanvasSwitch(1);
    }

    void CanvasSwitch(int index)
    {
        switch(index)
        {
            case 1:
                {
                    canvas1.gameObject.SetActive(true);
                    canvas2.gameObject.SetActive(false);
                    canvas3.gameObject.SetActive(false);
                    break;
                }
            case 2:
                {
                    canvas1.gameObject.SetActive(false);
                    canvas2.gameObject.SetActive(true);
                    canvas3.gameObject.SetActive(false);
                    break;
                }
            case 3:
                {
                    canvas1.gameObject.SetActive(false);
                    canvas2.gameObject.SetActive(false);
                    canvas3.gameObject.SetActive(true);
                    break;
                }
            default: break;
        }
    }
}
