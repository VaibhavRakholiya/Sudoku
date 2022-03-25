using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection_Number_Button : MonoBehaviour
{
    private Button button;
    private int index;
    void Awake()
    {
        button = this.GetComponent<Button>();
        index = this.transform.GetSiblingIndex();
    }
    void Start()
    {
        button.onClick.AddListener(handle_onClick_FillCurrentButton);
    }

    private void handle_onClick_FillCurrentButton()
    {
        UI_Manager.instance.CurrentButton.SetCurrentNumber(index + 1);
    }
}
