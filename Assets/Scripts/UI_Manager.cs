using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RDG;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public float time = 0;
    public int mistakes = 0;
    public GameObject AllButtons,SelectionButtons;
    [Header("Text")]
    public Text Timer_Text;
    public Text Difficutly_Text;
    public Text Mistake_Text;
    [HideInInspector] public Number_Button CurrentButton;
    [Header("Colors")]
    public Color Green, Red, Blue, White, Grey,Grey2;
    [Header("Function_Buttons")]
    [Header("Panels")]
    public GameObject NewGame_Panel;
    public GameObject GameOver_Panel;
    public GameObject Pause_Panel;
    public Button Notes_Button;
    [Header("RectTransforms")]
    public RectTransform[] NotesTextTransforms;
    private void Awake()
    {
        instance = this;
        Difficutly_Text.text = PlayerPrefs.GetString("Mode");
    }
    private void Update()
    {
        if(GameManager.instance.GameState == GameManager.State.Play)
        time += Time.deltaTime;
        if ((int)time / 60 < 10)
            Timer_Text.text = "0" + (int)time / 60 + ":" + (int)time % 60;
        if ((int)time % 60 < 10)
            Timer_Text.text = (int)time / 60 + ":0" + (int)time % 60;
        if ((int)time % 60 < 10 && (int)time / 60 < 10)
            Timer_Text.text = "0" + (int)time / 60 + ":0" + (int)time % 60;
        if ((int)time % 60 > 10 && (int)time / 60 > 10)
            Timer_Text.text = (int)time / 60 + ":" + (int)time % 60;
    }
    public void IncreaseMistakes(int amount)
    {
        mistakes+=amount;
        Mistake_Text.text = "Mistake : " + mistakes.ToString() + "/3";
        if (mistakes == 3)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        GameOver_Panel.SetActive(true);
        GameManager.instance.setGameState(GameManager.State.Pause);
    }
    public void handle_Onclick_TurnOnPencil()
    {
        AudioManager.instance.Play(2);
        //GameManager.instance.HighLightOff();
        if (GameManager.instance.isPencilOn)
        {
            GameManager.instance.isPencilOn = false;
            Notes_Button.GetComponent<Image>().color = Blue;
            foreach (Transform item in SelectionButtons.transform)
            {
                item.GetChild(0).gameObject.SetActive(false);
                item.GetChild(1).GetComponent<Text>().color = Blue;
            }
        }
        else
        {
            foreach (Transform item in SelectionButtons.transform)
            {
                item.GetChild(0).gameObject.SetActive(true);
                item.GetChild(1).GetComponent<Text>().color = Grey2;
            }
            GameManager.instance.isPencilOn = true;
            Notes_Button.GetComponent<Image>().color = Grey;
        }
    }
    public void handle_Onclick_NewGameButton()
    {
        NewGame_Panel.SetActive(true);
    }
    public void handle_Onclick_UseEraser()
    {
        if (PlayerPrefs.GetString("Vibrate") == "ON")
            Vibration.Vibrate(100);
            AudioManager.instance.Play(2);
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.RemoveButton(CurrentButton);
        //CurrentButton.ClearNotesText();
    }
    public void handle_Onclick_UndoButton()
    {
        AudioManager.instance.Play(2);
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.Undo(CurrentButton);
    }
    public void handle_onClick_Extralife()
    {
        IncreaseMistakes(-1);
        GameOver_Panel.SetActive(false);
        GameManager.instance.setGameState(GameManager.State.Play);
    }
    // Restart Game.
    public void handle_OnClick_RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void handle_OnClick_Close_NewPanelButton()
    {
        NewGame_Panel.SetActive(false);
    }
    public void handle_onClick_PauseGame()
    {
        GameManager.instance.setGameState(GameManager.State.Pause);
        AllButtons.SetActive(false);
        Pause_Panel.SetActive(true);
    }
    public void handle_onClick_PlayPause()
    {
        GameManager.instance.setGameState(GameManager.State.Play);
        Pause_Panel.SetActive(false);
        AllButtons.SetActive(true);
    }

    public void handle_OnClick_setMode(string mode)
    {
        PlayerPrefs.SetString("Mode", mode);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void hanlde_OnClick_ShareButton()
    {

    }
    public void handle_OnClick_ToggleSound()
    {
        if(PlayerPrefs.GetString("Sound")=="ON")
        {
            PlayerPrefs.SetString("Sound", "OFF");
        }
        else
        {
            PlayerPrefs.SetString("Sound", "ON");
        }
    }
    public void handle_OnClick_ToggleVibrate()
    {
        if(PlayerPrefs.GetString("Vibrate")=="ON")
        {
            PlayerPrefs.SetString("Vibrate", "OFF");
        }
        else
        {
            PlayerPrefs.SetString("Vibrate", "ON");
        }
    }
}
