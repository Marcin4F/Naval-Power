using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ReyCastSelecter : MonoBehaviour
{
    private Transform lastSelected;
    bool isSelected = false;
    int sceneIndex;
    protected Collider nearestField;

    protected GameManagment gameManagment;
    protected Functions functions;
    protected Ship ship;


    private void Start()
    {
        gameManagment = FindObjectOfType<GameManagment>();              // dolaczenie odpowiednich skryptow (gameManagemet -> gameState; functions -> funkcje)
        functions = gameObject.GetComponent<Functions>();
        ship = gameObject.GetComponent<Ship>();

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
                Rotation('E');
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Rotation('R');
            }
        }


        static Ray GetRay()         // stworzenie promienia od kursora
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }

    void Rotation(char direction)       // funkcja dokonujaca rotacje statku
    {
        bool validPosition;
        nearestField = functions.FindingField(lastSelected);

        float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        string fieldName = nearestField.name;

        int size = ship.Size;
        Debug.Log(size);

        if (direction == 'E')       // dokonanie rotacji we wlasciwym kierunku
        {
            validPosition = functions.ValidPosition(5, fieldName, shipRotation - 90);
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
        }
        else
        {

            validPosition = functions.ValidPosition(5, fieldName, shipRotation + 90);
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
        }

        if (!validPosition)     // jezeli aktualna pozycja nie jest wlasciwa dokonujemy przesuniecia statku o odpowiednia wartosc w odopwiednim kierunku, tak aby miescil sie na mapie
        {
            if (fieldName[0] < 'C')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - (2 + 65 - (int)fieldName[0]));
            }
            else if (fieldName[0] > 'N')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + (2 - 80 + (int)fieldName[0]));
            }
            else if (fieldName.Length != 3 && fieldName[1] < '3')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x + (2 + 49 - (int)fieldName[1]), lastPosition.y, lastPosition.z);
            }

            else if (fieldName.Length == 3 && fieldName[2] > '4')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x - (2 - 54 + (int)fieldName[2]), lastPosition.y, lastPosition.z);
            }
        }
        PlayerPrefs.SetInt("PossibleRotation" + lastSelected.name + sceneIndex, 1);     // zapisanie ze dany statek dokonal ruchu
    }
}