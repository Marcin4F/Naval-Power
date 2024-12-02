using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment : MonoBehaviour
{
    public GameObject pancernik;
    public GameObject niszczyciel;
    public int gameState = 0;           // faza gry
    public int shipsNumber = 2;         // ilosc statkow w grze

    // Start is called before the first frame update
    void Awake()
    {
        gameState = PlayerPrefs.GetInt("gameState" + SceneManager.GetActiveScene().buildIndex);       // faza gry: 0 - pierwsza tura, nie wczytujemy pozycji statkow, 1 - kolejne tury wczytujemy pozycje z plikow
        if (gameState == 0)
        {
            PlayerPrefs.SetInt("gameState" + SceneManager.GetActiveScene().buildIndex, 1);
        }

        // inicjalizacja statkow
        int index = SceneManager.GetActiveScene().buildIndex;
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
            pancernikComponent2.name = "Pancernik2";

            GameObject niszczyciel2 = Instantiate(niszczyciel);
            Niszczyciel niszczycielComponent2 = niszczyciel2.GetComponent<Niszczyciel>();
            niszczycielComponent2.name = "Niszczyciel2";
        }
    }
}
