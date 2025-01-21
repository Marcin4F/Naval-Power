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
    [SerializeField] Button nextPage, nextPage2;
    [SerializeField] Button previousPage, previousPage2;
    public GameObject mainMenuPanel, optionsPanel, tutorialPanel, tutorialTextPanel1, tutorialTextPanel2, tutorialTextPanel3;
    private int tutorialPage = 1;

    // Start is called before the first frame update
    void Start()
    {
        quit.onClick.AddListener(Quitting);
        playHotSeat.onClick.AddListener(StartPlayHotSeat);
        options.onClick.AddListener(OpenOptions);
        goBack.onClick.AddListener(CloseOptions);
        tutorial.onClick.AddListener(OpenTutorial);
        tutorialBack.onClick.AddListener(CloseTutorial);
        nextPage.onClick.AddListener(NextPage);
        nextPage2.onClick.AddListener(NextPage);
        previousPage.onClick.AddListener(PreviousPage);
        previousPage2.onClick.AddListener(PreviousPage);
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
        tutorialTextPanel2.SetActive(false);
        tutorialTextPanel3.SetActive(false);
    }

    private void CloseTutorial()
    {
        tutorialPage = 1;
        tutorialTextPanel1.SetActive(true);
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private void NextPage()
    {
        if (tutorialPage == 1)
        {
            tutorialTextPanel1.SetActive(false);
            tutorialTextPanel2.SetActive(true);
            tutorialPage = 2;
        }
        else if (tutorialPage == 2)
        {
            tutorialTextPanel2.SetActive(false);
            tutorialTextPanel3.SetActive(true);
            tutorialPage = 3;
        }
    }

    private void PreviousPage()
    {
        if (tutorialPage == 2)
        {
            tutorialTextPanel1.SetActive(true);
            tutorialTextPanel2.SetActive(false);
            tutorialPage = 1;
        }
        else if (tutorialPage == 3)
        {
            tutorialTextPanel2.SetActive(true);
            tutorialTextPanel3.SetActive(false);
            tutorialPage = 2;
        }
    }
}
