using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour          // SKRYPT JEST MANAGEREM SCEN
{
    public static ScenesManager instance;           // pozwala odwolac sie do tej klasy z kazdego skryptu bez potrzeby inicjalizowania refernecji do tego skryptu
    private string positions, attackedFields;


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
        Middle,
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
            positions = Functions.instance.ArrayToString(GameManagment.instance.occupiedFields, GameManagment.instance.maxSize);
            PlayerPrefs.SetString("Positions1", positions);

            attackedFields = Functions.instance.ArrayToString(GameManagment.instance.attackFields, 1);
            PlayerPrefs.SetString("AttackedFields1", attackedFields);


            direction = 1;
            PlayerPrefs.SetInt("Direction", direction);
            LoadScene(Scene.Middle);
        }
        else if (index == 3)
        {
            positions = Functions.instance.ArrayToString(GameManagment.instance.occupiedFields, GameManagment.instance.maxSize);
            PlayerPrefs.SetString("Positions3", positions);

            attackedFields = Functions.instance.ArrayToString(GameManagment.instance.attackFields, 1);
            PlayerPrefs.SetString("AttackedFields3", attackedFields);

            direction = -1;
            PlayerPrefs.SetInt("Direction", direction);
            LoadScene(Scene.Middle);
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
