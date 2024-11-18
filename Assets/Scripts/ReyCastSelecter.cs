using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReyCastSelecter : MonoBehaviour
{
    private Transform lastSelected;
    bool isSelected = false;
    protected GameManagment gameManagment;
    int sceneIndex;

    private void Start()
    {
        gameManagment = FindObjectOfType<GameManagment>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        string[] shipsNames = { "Pancernik(Clone)", "Niszczyciel(Clone)" };             // umozliwienie wykonywania rotacji na poczatku tury PRZENIESC NA FAZE KONCOWA
        foreach (string shipName in shipsNames)
        {
            string key = "PossibleRotation" + shipName + sceneIndex;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameManagment.gameState == 1) 
        {
            var ray = GetRay();
            if (Physics.Raycast(ray, out var raycastHit))       // sprawdzenie czy promien trafil w jakis obiekt
            {
                if (raycastHit.transform.CompareTag("Ship"))    // sprawdzenie czy trafiony obiekt to statek
                {
                    if (lastSelected != null)               // odznaczenie ostatnio wybranego statku
                    {
                        lastSelected.Translate(0, -1, 0);
                        isSelected = false;
                    }
                    var rayPos = raycastHit.transform;      // uzyskanie pozycji statku
                    lastSelected = rayPos;                  // zapisanie statku ktory wybralismy, by moc pozniej go odznaczyc
                    rayPos.Translate(0, 1, 0);              // podniesienie okretu (tymczasowe pokazanie wybrania)
                    isSelected = true;
                }
                else if (lastSelected != null)              // jezeli promien trafil w cos innego odznaczamy ostatnio wybrany statek
                {
                    lastSelected.Translate(0, -1, 0);
                    lastSelected = null;
                    isSelected = false;
                }
            }
            else if (lastSelected != null)               // jezeli promien w nic nie trafil odznaczamy ostatnio wybrany statek
            {
                lastSelected.Translate(0, -1, 0);
                lastSelected = null;
                isSelected=false;
            }
        }

        if(isSelected && PlayerPrefs.GetInt("PossibleRotation" + lastSelected.name + sceneIndex) == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
                lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
                PlayerPrefs.SetInt("PossibleRotation" + lastSelected.name + sceneIndex, 1);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
                lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
                PlayerPrefs.SetInt("PossibleRotation" + lastSelected.name + sceneIndex, 1);
            }
        }


        static Ray GetRay()         // stworzenie promienia od kursora
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }
}