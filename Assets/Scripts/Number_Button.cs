using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RDG;

public class Number_Button : MonoBehaviour
{
     public bool ishighlighted;
    public int Current_Number;
    public int index_i, index_j;
    private Text MainText, Notes_Text;
    private Button button;
    void Awake()
    {
        button = this.GetComponent<Button>();
        int index = this.transform.GetSiblingIndex();
        index_i = index / 9;
        index_j = index % 9;
        MainText = this.transform.GetChild(0).GetComponent<Text>();
        Notes_Text = this.transform.GetChild(1).GetComponent<Text>();
    }
    void Start()
    {
        if(Sudoku.instance.mat2[index_i,index_j]!=-1)
        {
            button.enabled = false;
        }
        else
        {
            button.onClick.AddListener(handle_onClick_SelectButton);
        }
    }
    private void handle_onClick_SelectButton()
    {
        UI_Manager.instance.CurrentButton = this;
        if(GameManager.instance.isHighlightOn)
        {
            StartCoroutine(GameManager.instance.HighLightOff());
            Invoke("HighLight", 0.30f);
        }
        else
        {
            HighLight();
        }
    }
   
   private void HighLight()
    {
        StartCoroutine(GameManager.instance.HighLightOn(index_i, index_j));
    }
    public void ToggleHighLight(bool status)
    {
        if(status)
        {
            GetComponent<Animator>().SetBool("Animate", true);
            ishighlighted = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("Animate2", true);
            ishighlighted = false;
        }
    }
    public void ChangeStateNotesText(bool state)
    {
        Notes_Text.gameObject.SetActive(state);
    }
    public bool GetStateNotesText()
    {
        return Notes_Text.gameObject.activeInHierarchy;
    }
    public void SetCurrentNumber(int number)
    {
        GameManager.instance.isErased = false;
        StartCoroutine(GameManager.instance.HighLightOff());
        if(!GameManager.instance.isPencilOn)
        {
            Current_Number = number;
            this.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
            this.transform.GetChild(1).gameObject.SetActive(false);
            if (Sudoku.instance.mat[index_i ,index_j] == number)
            {
                Vibration.Vibrate(25);
                button.enabled = false;
                this.setColor(UI_Manager.instance.Green);
                AudioManager.instance.Play(0);
            }
            else
            {
                Vibration.Vibrate(100);
                this.setColor(UI_Manager.instance.Red);
                UI_Manager.instance.IncreaseMistakes();
                AudioManager.instance.Play(1);
            }
        }
        else
        {
            SetNotesText(number.ToString());
            GameManager.instance.AddToUndoList(this);
        }
    }
    public void setColor(Color _color)
    {
        this.transform.GetChild(0).GetComponent<Text>().color = _color;
    }
    public void SetNotesText(string Text)
    {
        Notes_Text.gameObject.SetActive(true);
        Notes_Text.text = Text;
    }
    public void ClearNotesText()
    {
        Notes_Text.gameObject.SetActive(false);
        button.interactable = false;
        button.interactable = true;
    }
}
