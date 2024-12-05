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
    protected bool isSelected = false;
    protected int sceneIndex, validPosition, gameState, tmp, size, halfSize;
    protected float shipRotation;
    protected string key, fieldName;
    Vector3 movment;

    protected Collider nearestField;
    protected Collider[] childColliders;
    protected Collider[] childFieldColliders;   // lista wlasciwych colliderow odpowiadajacych za rozmiar
    private Transform lastSelected;

    Dictionary<string, int> shipsID = new Dictionary<string, int>();        // slownik z nazwami statkow i odpowiadajacymi im indeksami

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameState = GameManagment.instance.gameState;

        // UWAGA shipsNames wypelnic w kolejnosci wartosci shipID z klas statkow
        string[] shipsNames = { "Pancernik(Clone)", "Niszczyciel(Clone)" };             // umozliwienie wykonywania rotacji na poczatku tury PRZENIESC NA FAZE KONCOWA

        tmp = 0;
        foreach (string shipName in shipsNames)
        {
            key = "PossibleMovement" + shipName + sceneIndex;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }

            shipsID[shipName] = tmp++;          // uzupelnienie slownika
        }    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameState == 1) 
        {
            var ray = GetRay();
            if (Physics.Raycast(ray, out var raycastHit))       // sprawdzenie czy promien trafil w jakis obiekt
            {
                if (raycastHit.transform.CompareTag("Ship"))    // sprawdzenie czy trafiony obiekt to statek
                {
                    if (lastSelected != null)                   // odznaczenie ostatnio wybranego statku
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
                isSelected = false;
            }
        }

        if(isSelected)
        {
            int movesUsed = PlayerPrefs.GetInt("PossibleMovement" + lastSelected.name + sceneIndex);
            if (movesUsed == 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    TryRotation('A');
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    TryRotation('D');
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveBackward();
                }
            }
            
            if (movesUsed != 2)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveForward(movesUsed);
                }
            }
        }


        static Ray GetRay()         // stworzenie promienia od kursora
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }

    void TryRotation(char direction)       // proba dokonania rotacji statku
    {
        nearestField = Functions.instance.FindingField(lastSelected);

        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        fieldName = nearestField.name;

        childColliders = lastSelected.GetComponentsInChildren<Collider>();
        childFieldColliders = childColliders
            .Where(collider => collider.CompareTag("MainCollider") || collider.CompareTag("FieldCollider"))
            .ToArray();
        size = childColliders.Length - 1;           // nie wiadomo czemu musi byc -1
        halfSize = size / 2;

        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);      // uzyskanie indeksu wybranego statku

        if (direction == 'A')           // sprawdzenie czy dokonanie rotacji we wskazanym kierunku jest mozliwe
        {
            validPosition = Functions.instance.ValidPosition(size, fieldName, shipRotation - 90, GameManagment.instance.occupiedFields, shipID);
        }
        else
        {
            validPosition = Functions.instance.ValidPosition(size, fieldName, shipRotation + 90, GameManagment.instance.occupiedFields, shipID);
        }

        if (validPosition == 2)         // wykonanie rotacji
        {
            Rotate(direction, shipID);
            SaveInfo(2);
        }

        else if (validPosition == 1)        // jezeli aktualna pozycja nie jest wlasciwa dokonujemy przesuniecia statku o odpowiednia wartosc w odopwiednim kierunku, tak aby miescil sie na mapie
        {
            Rotate(direction, shipID);

            if (fieldName[0] < 'C')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - (halfSize + 65 - (int)fieldName[0]));
            }
            else if (fieldName[0] > 'N')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + (halfSize - 80 + (int)fieldName[0]));
            }
            else if (fieldName.Length != 3 && fieldName[1] < '3')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x + (halfSize + 49 - (int)fieldName[1]), lastPosition.y, lastPosition.z);
            }
            else
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x - (halfSize - 54 + (int)fieldName[2]), lastPosition.y, lastPosition.z);
            }

            nearestField = Functions.instance.FindingField(lastSelected);
            fieldName = nearestField.name;
            shipRotation = lastSelected.transform.rotation.eulerAngles.y;
            Functions.instance.ValidPosition(size, fieldName, shipRotation, GameManagment.instance.occupiedFields, shipID);
            for (int i = 0; i < Functions.instance.shipFields.Length; i++)
            {
                GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
            }
            SaveInfo(2);
        }
    }

    void MoveForward(int movesUsed)
    {
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        nearestField = Functions.instance.FindingField(lastSelected);
        fieldName = nearestField.name;
        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);      // uzyskanie indeksu wybranego statku

        if (shipRotation == 0)
        {
            int a = Functions.instance.ValidMove(size, fieldName, shipRotation, GameManagment.instance.occupiedFields, shipID, 'W');
            if (a == 1)
                movment = new Vector3(1, 0, 0);
            else
                movment = new Vector3(0, 0, 0);
        }
            
        else if (shipRotation == -90 || shipRotation == 270)
            movment = new Vector3 (0, 0, 1);
        else if (shipRotation == 180 || shipRotation == -180)
            movment = new Vector3 (-1, 0, 0);
        else if (shipRotation == 90)
            movment = new Vector3 (0, 0, -1);
        //validPosition = Functions.instance.ValidPosition(size, )
        lastSelected.transform.position += movment;
        SaveInfo(movesUsed + 1);
    }

    void MoveBackward()
    {
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        if (shipRotation == 0)
            movment = new Vector3(-1, 0, 0);
        else if (shipRotation == -90 || shipRotation == 270)
            movment = new Vector3(0, 0, -1);
        else if (shipRotation == 180 || shipRotation == -180)
            movment = new Vector3(1, 0, 0);
        else if (shipRotation == 90)
            movment = new Vector3(0, 0, 1);
        lastSelected.transform.position += movment;
        SaveInfo(2);
    }

    void SaveInfo(int moveUsed)
    {
        PlayerPrefs.SetInt("PossibleMovement" + lastSelected.name + sceneIndex, moveUsed);      // zapisanie ze dany statek dokonal ruchu
        name = lastSelected.name.Remove(lastSelected.name.Length - 7, 7);                       // 7 zeby usunac napis "(Clone)"
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        PlayerPrefs.SetFloat(name + sceneIndex + "Rotation", shipRotation);
        PlayerPrefs.SetFloat(name + sceneIndex + "X", lastSelected.transform.position.x);
        PlayerPrefs.SetFloat(name + sceneIndex + "Z", lastSelected.transform.position.z);
    }

    void Rotate(char direction, int shipID)       // rotowanie statku
    {
        if (direction == 'A')
        {
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
        }
        else
        {
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
        }

        for (int i = 0; i < Functions.instance.shipFields.Length; i++)
        {
            GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
        }
    }
}