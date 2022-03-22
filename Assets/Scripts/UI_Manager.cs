using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public GameObject AllButtons;
    [HideInInspector]public Number_Button CurrentButton;
    public Color Green, Red;
    private void Awake()
    {
        instance = this;
    }
}
