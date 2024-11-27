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
    protected GameManagment gameManagment;
    int sceneIndex;
    protected Collider[] nearbyFields;
    protected Collider nearestField;
    protected Collider[] childColliders;
    protected Collider mainCollider;
    protected CheckPosition checkPosition;


    private void Start()
    {
        gameManagment = FindObjectOfType<GameManagment>();
        checkPosition = gameObject.AddComponent<CheckPosition>();

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
                    childColliders = lastSelected.GetComponentsInChildren<Collider>();
                    mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znaleznie MainCollider

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

    void Rotation(char direction)
    {
        bool validPosition;
        nearbyFields = Physics.OverlapSphere(mainCollider.transform.position, 1f)       // tworzymy sfere wokol mainCollider z promieniem 1f i znajdujemy wszystkie pola Fields
         .Where(collider => collider.CompareTag("Field"))
           .ToArray();

        nearestField = nearbyFields.OrderBy(field => Vector3.Distance(mainCollider.transform.position, field.transform.position))   // znalezienie najblizszego pola (sortujemy po dystansie mainCollider i danego pola
         .FirstOrDefault();
        
        float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        string nazwaPola = nearestField.name;

        if(direction == 'E')
        {
            validPosition = checkPosition.ValidPosition(5, nazwaPola, shipRotation - 90);
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
            PlayerPrefs.SetInt("PossibleRotation" + lastSelected.name + sceneIndex, 1);
            if (!validPosition)
            {
                string fieldName = nearestField.name;
                if (fieldName[0] < 'C')
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - (2 + 65 - (int)fieldName[0]));
                }
                else if (fieldName[0] > 'N')
                {
                    Debug.Log("c");
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + (2 + 65 - (int)fieldName[0]));
                }
                else if (fieldName[1] < '3')
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x + 2, lastPosition.y, lastPosition.z);
                }
                
                else if (fieldName.Length == 3)
                {
                    Debug.Log(fieldName[2]);
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x - 2, lastPosition.y, lastPosition.z);
                }
            }
        }
        else
        {
            validPosition = checkPosition.ValidPosition(5, nazwaPola, shipRotation + 90);
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
            PlayerPrefs.SetInt("PossibleRotation" + lastSelected.name + sceneIndex, 1);
            if (!validPosition)
            {
                if (shipRotation + 90 == 0)
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x - 2, lastPosition.y, lastPosition.z);
                }
                else if (shipRotation + 90 == 180 || shipRotation + 90 == -180)
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x + 2, lastPosition.y, lastPosition.z);
                }
                if (shipRotation + 90 == 90)
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 2);
                }
                if (shipRotation + 90 == -90)
                {
                    Vector3 lastPosition = lastSelected.transform.position;
                    lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 2);
                }
            }
        }
    }
}