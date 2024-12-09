using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button quit;
    [SerializeField] Button play;

    // Start is called before the first frame update
    void Start()
    {
        quit.onClick.AddListener(Quitting);
        play.onClick.AddListener(StartPlay);
    }

    private void Quitting()
    {
        Debug.Log("Dziala");
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    private void StartPlay()
    {
        ScenesManager.instance.NewGame();
    }
}
