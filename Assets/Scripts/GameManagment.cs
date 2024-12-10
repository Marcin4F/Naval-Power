using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameManagment : MonoBehaviour
{
    public static GameManagment instance;

    public int gameState = 0, shipsNumber = 2, maxSize = 5, shipsLeft = 2;           // faza gry
    private int index;
    public string[,] occupiedFields, attackFields, fieldsUnderAttack;

    public GameObject pancernik, niszczyciel;
    private GameObject pancernik1, niszczyciel1;
    Pancernik pancernikComponent1;
    Niszczyciel niszczycielComponent1;
    public Material zniszczony;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        index = SceneManager.GetActiveScene().buildIndex;

        // inicjalizacja statkow

        if (index == 1)
        {
            pancernik1 = Instantiate(pancernik);
            pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "Pancernik1";

            niszczyciel1 = Instantiate(niszczyciel);
            niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "Niszczyciel1";
        }
        else if (index == 3)
        {

            pancernik1 = Instantiate(pancernik);
            pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "Pancernik3";

            niszczyciel1 = Instantiate(niszczyciel);
            niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "Niszczyciel3";
        }

        gameState = PlayerPrefs.GetInt("gameState" + index);       // faza gry: 0 - pierwsza tura, nie wczytujemy pozycji statkow, 1 - kolejne tury wczytujemy pozycje z plikow
        if (gameState == 0)
        {
            PlayerPrefs.SetInt("gameState" + index, 1);
            SaveHP();
        }
        else
        {
            // wczytanie pozycji statkow oraz sprawdzenie czy statki otrzymaly obrazenia
            occupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions" + index), maxSize);
            shipsLeft = PlayerPrefs.GetInt("ShipsLeft" + index);

            pancernikComponent1.hp = PlayerPrefs.GetInt("PancernikHP" + index);
            if (pancernikComponent1.hp == 0)
            {
                pancernik1.transform.tag = "Destroyed";
                var renderer = pancernik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            niszczycielComponent1.hp = PlayerPrefs.GetInt("NiszczycielHP" + index);
            if (niszczycielComponent1.hp == 0)
            {
                niszczyciel1.transform.tag = "Destroyed";
                var renderer = niszczyciel1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }

            if (index == 1)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields3"), 1);
            }
            else if (index == 3)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields1"), 1);
            }

            for(int k = 0; k < fieldsUnderAttack.Length; k++)
            {
                string field = fieldsUnderAttack[k, 0];
                int tmp = 0;
                for (int i = 0; i < shipsNumber; i++)
                {
                    for (int j = 0; j < maxSize; j++)
                    {
                        if (field != "" && field == occupiedFields[i, j])
                        {
                            TakeDamage(i);
                            PlayerPrefs.SetInt("Znacznik2" + index + k, 1);
                            tmp = 1;
                        }
                    }
                    if (tmp == 0)
                    {
                        PlayerPrefs.SetInt("Znacznik2" + index + k, 0);
                    }
                }
            }

            SaveHP();
            if (shipsLeft <= 0)
            {
                // UWAGA DODAC ZAKONCZENIE GRY
                Debug.Log("KONIEC GRY");
            }
        }
    }

    void TakeDamage(int shipID)
    {
        switch(shipID)
        {
            case 0:
                pancernikComponent1.hp -= 1;
                if (pancernikComponent1.hp <= 0)
                {
                    pancernik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = pancernik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
            case 1:
                niszczycielComponent1.hp -= 1;
                if (niszczycielComponent1.hp <= 0)
                {
                    niszczyciel1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = niszczyciel1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
        }
    }

    void SaveHP()
    {
        PlayerPrefs.SetInt("PancernikHP" + index, pancernikComponent1.hp);
        PlayerPrefs.SetInt("NiszczycielHP" + index, niszczycielComponent1.hp);
        PlayerPrefs.SetInt("ShipsLeft" + index, shipsLeft);
    }
}
