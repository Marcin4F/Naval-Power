using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button quit;
    [SerializeField] Button playHotSeat;
    [SerializeField] Button playPvE;
    [SerializeField] Button options;
    [SerializeField] Button goBack;
    [SerializeField] Button tutorial;
    [SerializeField] Button tutorialBack;
    public GameObject mainMenuPanel, optionsPanel, tutorialPanel;

    // Start is called before the first frame update
    void Start()
    {
        quit.onClick.AddListener(Quitting);
        playHotSeat.onClick.AddListener(StartPlayHotSeat);
        options.onClick.AddListener(OpenOptions);
        goBack.onClick.AddListener(CloseOptions);
        tutorial.onClick.AddListener(OpenTutorial);
        tutorialBack.onClick.AddListener(CloseTutorial);
        optionsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    private void Quitting()
    {
        Debug.Log("Dziala");
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    private void StartPlayHotSeat()
    {
        ScenesManager.instance.NewGameHotSeat();
    }

    private void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    private void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private void OpenTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    private void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
