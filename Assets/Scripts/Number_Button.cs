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
        
            button.onClick.AddListener(handle_onClick_SelectButton);
    }
    private void handle_onClick_SelectButton()
    {
        if(Current_Number==-1 && GameManager.instance.GameState==GameManager.State.Play)
        {
            UI_Manager.instance.CurrentButton = this;
            if (GameManager.instance.isHighlightOn)
            {
                StartCoroutine(GameManager.instance.HighLightOff());
                Invoke("HighLight", 0.30f);
            }
            else
            {
                HighLight();
            }
        }
        else if(GameManager.instance.GameState == GameManager.State.Play)
        {
            if (GameManager.instance.isHighlightOn)
                StartCoroutine(GameManager.instance.HighLightOff());
            StartCoroutine(GameManager.instance.HighLightAllNumber(Current_Number));
        }
     
    }
   
   private void HighLight()
    {
        StartCoroutine(GameManager.instance.HighLightOn(index_i, index_j));
    }
    public void ToggleHighLight(int index)
    {
        if(index==-1)
        {
            GetComponent<Animator>().SetBool("Animate2", true);
            ishighlighted = false;
     
        }
        else if(index==0)
        {
            GetComponent<Animator>().SetBool("Animate", true);
            ishighlighted = true;
        }
        else if(index==1)
        {
            GetComponent<Animator>().SetBool("Animate3", true);
            ishighlighted = true;
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
            setNumber(number);
            StartCoroutine(GameManager.instance.HighLightAllNumber(number));
            //ToggleHighLight(true);
            if (Sudoku.instance.mat[index_i ,index_j] == number)
            {
                UI_Manager.instance.CurrentButton = null;
                if (PlayerPrefs.GetString("Vibrate") == "ON")
                    Vibration.Vibrate(25);
                button.enabled = false;
                this.setColor(UI_Manager.instance.Green);
                AudioManager.instance.Play(0);
            }
            else
            {
                UI_Manager.instance.CurrentButton = null;
                if (PlayerPrefs.GetString("Vibrate")=="ON") 
                Vibration.Vibrate(100);
                this.setColor(UI_Manager.instance.Red);
                Current_Number = -1;
                UI_Manager.instance.IncreaseMistakes(1);
                AudioManager.instance.Play(1);
            }
        }
        else
        {
            SetNotesText(number.ToString());
            GameManager.instance.AddToUndoList(this);
        }
    }
    public void setNumber(int number)
    {
        Current_Number = number;
        MainText.text = number.ToString();
        Notes_Text.gameObject.SetActive(false);
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
    public void setNotesTextPosition(int i,int j)
    {
        int index = (i*3)+j;
        Notes_Text.rectTransform.localPosition = UI_Manager.instance.NotesTextTransforms[index].localPosition;
    }
    public void ClearNotesText()
    {
        Notes_Text.gameObject.SetActive(false);
        button.interactable = false;
        button.interactable = true;
    }
}
