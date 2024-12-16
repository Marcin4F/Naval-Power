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
    public GameObject mainMenuPanel, optionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        quit.onClick.AddListener(Quitting);
        playHotSeat.onClick.AddListener(StartPlayHotSeat);
        options.onClick.AddListener(OpenOptions);
        goBack.onClick.AddListener(CloseOptions);
        optionsPanel.SetActive(false);
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
}
