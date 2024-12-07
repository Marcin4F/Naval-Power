using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment : MonoBehaviour
{
    public static GameManagment instance;
    public GameObject pancernik;
    public GameObject niszczyciel;
    public int gameState = 0, shipsNumber = 2, maxSize = 5;           // faza gry
    public string[,] occupiedFields, attackFields, fieldsUnderAttack;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        int index = SceneManager.GetActiveScene().buildIndex;
        gameState = PlayerPrefs.GetInt("gameState" + index);       // faza gry: 0 - pierwsza tura, nie wczytujemy pozycji statkow, 1 - kolejne tury wczytujemy pozycje z plikow
        if (gameState == 0)
        {
            PlayerPrefs.SetInt("gameState" + index, 1);
        }
        else
        {
            occupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions" + index), maxSize);
            if (index == 1)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields3"), 1);
            }
            else if (index == 3)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields1"), 1);
            }
        }

        // inicjalizacja statkow
        
        if (index == 1)
        {
            GameObject pancernik1 = Instantiate(pancernik);
            Pancernik pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "Pancernik1";

            GameObject niszczyciel1 = Instantiate(niszczyciel);
            Niszczyciel niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "Niszczyciel1";
        }
        else if (index == 3)
        {

            GameObject pancernik2 = Instantiate(pancernik);
            Pancernik pancernikComponent2 = pancernik2.GetComponent<Pancernik>();
            pancernikComponent2.name = "Pancernik3";

            GameObject niszczyciel2 = Instantiate(niszczyciel);
            Niszczyciel niszczycielComponent2 = niszczyciel2.GetComponent<Niszczyciel>();
            niszczycielComponent2.name = "Niszczyciel3";
        }
    }
}
