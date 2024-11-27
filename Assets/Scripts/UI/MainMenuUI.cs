using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//aaaaaaaaaaaaaaaaaaaaa

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button quit;

    // Start is called before the first frame update
    void Start()
    {
        quit.onClick.AddListener(Quitting);
    }

    private void Quitting()
    {
        Debug.Log("Dziala");
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
