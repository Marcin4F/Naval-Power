using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour          // SKRYPT JEST MANAGEREM SCEN
{
    public static ScenesManager instance;           // pozwala odwolac sie do tej klasy z kazdego skryptu bez potrzeby inicjalizowania refernecji do tego skryptu 

    private void Awake()        // przypisanie zmiennej instance, tej klasy, czyli ScenesManager
    {
        instance = this;
        if (!PlayerPrefs.HasKey("Direction"))
            PlayerPrefs.SetInt("Direction", 1);
    }

    public enum Scene           // lista stalych nazw scen, WAZNA JEST KOLEJNOSC, nazwy NIE musza byc identyczne z nazwami w unity liczy sie tylko kolejnosc
    {
        MainMenu,
        Player1,
        Player2
    }

    public void LoadScene(Scene scene)      // domyslnie ladowanie konktetnej sceny
    {
        SceneManager.LoadScene (scene.ToString());
    }

    public void NewGame()                   // zaczecie nowej gry
    {
        // DO DODANIA: zaczecie nowej gry
    }

    public void EndTurn()                   // konczenie tury i przejscie do drugiego gracza
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int direction = PlayerPrefs.GetInt("Direction");

        if (index == 1)
        {
            direction = 1;
            PlayerPrefs.SetInt("Direction", direction);
            SceneManager.LoadScene(index + 1);
        }
        else if (index == 3)
        {
            direction = -1;
            PlayerPrefs.SetInt("Direction", direction);
            SceneManager.LoadScene(index - 1);
        }
        else if (index == 2)
        {
            SceneManager.LoadScene(index + direction);
        }
    }

    public void MainMenu()                  // wyjscie do menu glownego, dodac pozniej pause menu pod ESC w skrypcie InGameUI
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

}
