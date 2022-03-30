using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public float time = 0;
    public int mistakes = 0;
    public GameObject AllButtons, SelectionButtons;
    [Header("Text")]
    public Text Timer_Text;
    public Text Difficutly_Text;
    public Text Mistake_Text;
    public Text FinishPanel_Description_Text;
    [HideInInspector] public Number_Button CurrentButton;
    [Header("Colors")]
    public Color Green, Red, Blue, White, Grey, Grey2;
    [Header("Function_Buttons")]
    [Header("Panels")]
    public GameObject NewGame_Panel;
    public GameObject GameOver_Panel;
    public GameObject Pause_Panel;
    public GameObject Finish_Panel;
    public Button Notes_Button;
    [Header("RectTransforms")]
    public RectTransform[] NotesTextTransforms;
    public GameObject SoundToggle,  VibrateToggle;
    private void Awake()
    {
        instance = this;
        Difficutly_Text.text = PlayerPrefs.GetString("Mode");
    }
    private void Start()
    {
        if(PlayerPrefs.GetString("Sound")=="ON")
        {
            SoundToggle.SetActive(false);
        }
        else
        {
            SoundToggle.SetActive(true);
        }
        if (PlayerPrefs.GetString("Vibrate") == "ON")
        {
            VibrateToggle.SetActive(false);
        }
        else
        {
            VibrateToggle.SetActive(true);
        }
    }
    private void Update()
    {
        if (GameManager.instance.GameState == GameManager.State.Play)
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
        mistakes += amount;
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
        GameManager.instance.Vibrate(25);
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
        AudioManager.instance.Play(3);
        NewGame_Panel.SetActive(true);
        NewGame_Panel.transform.GetChild(0).GetComponent<Animator>().SetBool("Animate", true);
    }
    public void handle_Onclick_UseEraser()
    {
       GameManager.instance.Vibrate(100);
        AudioManager.instance.Play(2);
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.RemoveButton(CurrentButton);
        //CurrentButton.ClearNotesText();
    }
    public void handle_Onclick_UndoButton()
    {
        GameManager.instance.Vibrate(25);
        AudioManager.instance.Play(2);
        StartCoroutine(GameManager.instance.HighLightOff());
        GameManager.instance.Undo(CurrentButton);
    }
    public void handle_onClick_Extralife()
    {
        AdManager.instance.ShowRewardedAd(0);
    }
    // Restart Game.
    public void handle_OnClick_RestartGame()
    {
        AudioManager.instance.Play(3);
        AdManager.instance.ShowFullScreenAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void handle_OnClick_Close_NewPanelButton()
    {
        AudioManager.instance.Play(3);
        NewGame_Panel.transform.GetChild(0).GetComponent<Animator>().SetBool("Animate2", true);
        Invoke("TurnOffNewPanel", 0.45f);
        //NewGame_Panel.SetActive(false);
    }
    public void handle_onClick_PauseGame()
    {
        AudioManager.instance.Play(3);
        GameManager.instance.setGameState(GameManager.State.Pause);
        AllButtons.SetActive(false);
        Pause_Panel.SetActive(true);
    }
    public void handle_onClick_PlayPause()
    {
        AudioManager.instance.Play(3);
        GameManager.instance.setGameState(GameManager.State.Play);
        Pause_Panel.SetActive(false);
        AllButtons.SetActive(true);
    }

    public void handle_OnClick_setMode(string mode)
    {
        AudioManager.instance.Play(3);
        PlayerPrefs.SetString("Mode", mode);
        NewGame_Panel.transform.GetChild(0).GetComponent<Animator>().SetBool("Animate2", true);
        Invoke("LoadScene", 0.45f);
    }
    public void hanlde_OnClick_ShareButton()
    {
        GameManager.instance.Vibrate(25);
        AudioManager.instance.Play(3);
        StartCoroutine(Share());
    }
    public void handle_OnClick_HintButton()
    {
        AdManager.instance.ShowRewardedAd(1);
    }
    public void handle_OnClick_ToggleSound()
    {
        GameManager.instance.Vibrate(25);
        AudioManager.instance.Play(3);
        if (PlayerPrefs.GetString("Sound") == "ON")
        {
            PlayerPrefs.SetString("Sound", "OFF");
            SoundToggle.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("Sound", "ON");
            SoundToggle.SetActive(false);
        }
    }
    public void handle_OnClick_ToggleVibrate()
    {
        GameManager.instance.Vibrate(25);
        AudioManager.instance.Play(3);
        if (PlayerPrefs.GetString("Vibrate") == "ON")
        {
            VibrateToggle.SetActive(true);
            PlayerPrefs.SetString("Vibrate", "OFF");
        }
        else
        {
            VibrateToggle.SetActive(false);
            PlayerPrefs.SetString("Vibrate", "ON");
        }
    }
    // Functions for Invoke
    private void LoadScene()
    {
        if (PlayerPrefs.GetInt("Ads") == 0)
            AdManager.instance.ShowFullScreenAd();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void TurnOffNewPanel()
    {
        NewGame_Panel.SetActive(false);
    }


    public void OnAndroidTextSharingClick()
    {
        //FindObjectOfType<AudioManager>().Play("Enter");
    }

   
    IEnumerator Share()
    {
        yield return new WaitForEndOfFrame();
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "Sudoku.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("I am inviting you to solve this Sudoku Problem.").SetUrl("https://play.google.com/store/apps/details?id=com.sports.basketball.games.dunkenstar")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}

