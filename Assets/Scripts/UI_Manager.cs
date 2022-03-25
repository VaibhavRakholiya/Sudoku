using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public float time = 0;
    public int mistakes = 0;
    public GameObject AllButtons;
    [Header("Text")]
    public Text Timer_Text;
    public Text Difficutly_Text;
    public Text Mistake_Text;
    [HideInInspector] public Number_Button CurrentButton;
    public Color Green, Red, Blue, White, Grey;
    [Header("Function_Buttons")]
    [Header("Panels")]
    public GameObject NewGame_Panel;
    public GameObject GameOver_Panel;
    public Button Notes_Button;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
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
    public void IncreaseMistakes()
    {
        mistakes++;
        Mistake_Text.text = "Mistake : " + mistakes.ToString() + "/3";
        if (mistakes == 3)
        {
            GameOver();
        }
    }
    public void GameOver()
    {

    }
    public void handle_Onclick_TurnOnPencil()
    {
        //GameManager.instance.HighLightOff();
        if (GameManager.instance.isPencilOn)
        {
            GameManager.instance.isPencilOn = false;
            Notes_Button.GetComponent<Image>().color = Blue;
        }
        else
        {
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
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.RemoveButton(CurrentButton);
        //CurrentButton.ClearNotesText();
    }
    public void handle_Onclick_UndoButton()
    {
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.Undo(CurrentButton);
    }

}
