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
        // DO DODANIA: ekran przejsciowy pomiedzy graczami
        int index = SceneManager.GetActiveScene().buildIndex;           // UWAGA zmienic wartosc w if-ach w przypadku zmian w ilosci/kolejnosci scen
        if (index == 1)
            SceneManager.LoadScene(index + 1);
        else if (index == 2)
            SceneManager.LoadScene(index - 1);
    }

    public void MainMenu()                  // wyjscie do menu glownego, dodac pozniej pause menu pod ESC w skrypcie InGameUI
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

}
