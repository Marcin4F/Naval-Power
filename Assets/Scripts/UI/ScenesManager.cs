using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour          // SKRYPT JEST MANAGEREM SCEN
{
    public static ScenesManager instance;           // pozwala odwolac sie do tej klasy z kazdego skryptu bez potrzeby inicjalizowania refernecji do tego skryptu
    private Attack attack;
    private string positions, attackedFields, abilityFields;


    private void Awake()        // przypisanie zmiennej instance, tej klasy, czyli ScenesManager
    {
        instance = this;
        if (!PlayerPrefs.HasKey("Direction"))
            PlayerPrefs.SetInt("Direction", 1);
        attack = GetComponent<Attack>();
    }

    public enum Scene           // lista stalych nazw scen, WAZNA JEST KOLEJNOSC, nazwy NIE musza byc identyczne z nazwami w unity liczy sie tylko kolejnosc
                                // NA PRZYSZLOSC: jezeli chcemy odwolac sie do innej sceny to: ScenesManager.instance.LoadScene(ScenesManager.Scene.'Nazwa z enum');
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

    public void NewGameHotSeat()                   // zaczecie nowej gry
    {
        LoadScene(Scene.Player1);
    }

    public void EndTurn()                   // konczenie tury i przejscie do drugiego gracza
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int direction = PlayerPrefs.GetInt("Direction");
        
        if (GameManagment.instance.gameState != 2)
        {
            positions = Functions.instance.ArrayToString(GameManagment.instance.occupiedFields, GameManagment.instance.shipsNumber, GameManagment.instance.maxSize);
            attackedFields = Functions.instance.ArrayToString(GameManagment.instance.attackFields, GameManagment.instance.shipsNumber, 1);
            abilityFields = Functions.instance.ArrayToString(GameManagment.instance.abilityFields, 1, 3);

            if (index == 1)
            {
                PlayerPrefs.SetString("Positions1", positions);
                PlayerPrefs.SetString("AttackedFields1", attackedFields);
                PlayerPrefs.SetString("AbilityFields1", abilityFields);

                for (int i = 0; i < GameManagment.instance.shipsNumber; i++)
                {
                    PlayerPrefs.SetFloat("ZnacznikiX" + index + i, attack.positions[i].x);
                    PlayerPrefs.SetFloat("ZnacznikiY" + index + i, attack.positions[i].y);
                    PlayerPrefs.SetFloat("ZnacznikiZ" + index + i, attack.positions[i].z);
                }

                direction = 1;
                PlayerPrefs.SetInt("Direction", direction);
                LoadScene(Scene.Middle);
            }
            else if (index == 3)
            {
                PlayerPrefs.SetString("Positions3", positions);
                PlayerPrefs.SetString("AttackedFields3", attackedFields);
                PlayerPrefs.SetString("AbilityFields3", abilityFields);

                for (int i = 0; i < GameManagment.instance.shipsNumber; i++)
                {
                    PlayerPrefs.SetFloat("ZnacznikiX" + index + i, attack.positions[i].x);
                    PlayerPrefs.SetFloat("ZnacznikiY" + index + i, attack.positions[i].y);
                    PlayerPrefs.SetFloat("ZnacznikiZ" + index + i, attack.positions[i].z);
                }


                direction = -1;
                PlayerPrefs.SetInt("Direction", direction);
                LoadScene(Scene.Middle);
            }
            else if (index == 2)
            {
                SceneManager.LoadScene(index + direction);
            }
        }
        else
        {
            if (index == 1)
            {
                LoadScene(Scene.Player2);
            }
            else
            {
                LoadScene(Scene.Player1);
            }
        }
        
    }

    public void MainMenu()                  // wyjscie do menu glownego, dodac pozniej pause menu pod ESC w skrypcie InGameUI
    {
        PlayerPrefs.DeleteAll();
        InGameUI.isPaused = false;
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }
}
