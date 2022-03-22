using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number_Button : MonoBehaviour
{
    public int Answer,Current_Number;
    private int index;
    private Button button;
    void Awake()
    {
        button = this.GetComponent<Button>();
        index = this.transform.GetSiblingIndex();
        this.transform.GetChild(0).GetComponent<Text>().fontSize = 50;
    }
    void Start()
    {
        button.onClick.AddListener(handle_onClick_SelectButton);
    }
    private void handle_onClick_SelectButton()
    {
        UI_Manager.instance.CurrentButton = this;
    }
    public void SetCurrentNumber(int number)
    {
        Current_Number = number;
        this.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
        int index = this.transform.GetSiblingIndex();
        if (Sudoku.instance.mat[index/9,index%9] == number)
        {
           this.setColor(UI_Manager.instance.Green);
        }
        else
        {
           this.setColor(UI_Manager.instance.Red);
        }
    }
    public void setColor(Color _color)
    {
        this.transform.GetChild(0).GetComponent<Text>().color = _color;
    }
}
