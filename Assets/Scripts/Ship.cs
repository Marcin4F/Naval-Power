using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class Ship : MonoBehaviour
{
    // zmienne podstawowe dla statków
    protected int move;
    public int hp; // zycie
    protected int zanurzenie;
    public new string name;
    public int size;
    protected int shipID;

    // zmienne dla funkcji ship drag
    protected Collider[] childColliders;
    protected Collider mainCollider;
    protected Collider[] nearbyFields;
    protected Collider nearestField;
    public Vector3 lastSelectedPosition, lastSelectedRotation, offset;

    protected int validPosition, rotacjaint, index;
    protected float rotacja, shipRotation;
    protected string nazwaPola;
    protected bool[] shipPlaced;

    protected void Start()
    {
        childColliders = GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znalezienie MainCollider
        index = SceneManager.GetActiveScene().buildIndex;

        //zapisywanie pozycji statków do pliku
        if (GameManagment.instance.gameState == 0)
        {
            PlayerPrefs.DeleteKey(name + "X");      // usuniecie jezeli istnialy jakies pliki o podanych nazwach z wczesniejszej gry
            PlayerPrefs.DeleteKey(name + "Z");
            PlayerPrefs.DeleteKey(name + "Rotation");
            GameManagment.instance.occupiedFields = new string[GameManagment.instance.shipsNumber, GameManagment.instance.maxSize];
            GameManagment.instance.destroyedFields = new string[GameManagment.instance.shipsNumber, GameManagment.instance.maxSize];
            GameManagment.instance.enemyDestroyedFields = new string[GameManagment.instance.shipsNumber, GameManagment.instance.maxSize];
            PlayerPrefs.SetString("DestroyedFields" + index, Functions.instance.ArrayToString(GameManagment.instance.destroyedFields, GameManagment.instance.maxSize));

            shipPlaced = new bool[GameManagment.instance.shipsNumber];
            for (int i = 0; i < GameManagment.instance.shipsNumber; i++)
            {
                shipPlaced[i] = false;
            }
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat(name + "X"), 0.5f, PlayerPrefs.GetFloat(name + "Z"));     // wczytanie pozycji z plikow
            transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat(name + "Rotation"), 0);
        }
    }

    //zapisywanie pozycji statków do pliku
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    // funkcje ship drag
    protected void OnMouseDown()      // klikniecie LPM
    {
        if (GameManagment.instance.gameState == 0 && InGameUI.isPaused == false)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
        {
            lastSelectedPosition = transform.position;
            lastSelectedRotation = transform.eulerAngles;
            offset = mainCollider.transform.position - MouseWorldPosition();     // wyliczenie offsetu wzgledem MainCollider
            InGameUI.instance.isDraged = true;
        }
    }

    public void OnMouseUp()        // puszczanie LPM
    {
        if (GameManagment.instance.gameState == 0 && InGameUI.isPaused == false)       // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
        {
            nearbyFields = Physics.OverlapSphere(mainCollider.transform.position, 1f)       // tworzymy sfere wokol mainCollider z promieniem 1f i znajdujemy wszystkie pola Fields
             .Where(collider => collider.CompareTag("Field"))
               .ToArray();

            if (nearbyFields.Length == 0)
            {
                transform.position = lastSelectedPosition;
                transform.rotation = Quaternion.Euler(lastSelectedRotation.x, lastSelectedRotation.y, lastSelectedRotation.z);
            }
            else if (nearbyFields.Length > 0)        // jezeli znaleziono jakies pola w ustawionym promieniu
            {
                nearestField = nearbyFields.OrderBy(field => Vector3.Distance(mainCollider.transform.position, field.transform.position))   // znalezienie najblizszego pola (sortujemy po dystansie mainCollider i danego pola
                 .FirstOrDefault();

                rotacja = transform.rotation.eulerAngles.y;
                rotacjaint = (int)rotacja;
                nazwaPola = nearestField.name;
                validPosition = Functions.instance.ValidPosition(size, nazwaPola, rotacjaint, GameManagment.instance.occupiedFields, shipID);

                if (validPosition == 2)
                {
                    Vector3 positionOffset = transform.position - mainCollider.transform.position; // roznica miedzy pozycja obiektu a MainCollider
                    transform.position = nearestField.transform.position + positionOffset;       // zmieniamy pozycje obiektu z uwzglednieniem offsetu
                    if (shipPlaced[shipID] == false)
                    {
                        InGameUI.instance.shipPlaced++;              // zwiekszenie ilosci postawionych statkow na planszy
                        shipPlaced[shipID] = true;
                    }
                    for (int i = 0; i < size; i++)
                    {
                        GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
                    }
                }
                else
                {
                    transform.position = lastSelectedPosition;
                    transform.rotation = Quaternion.Euler(lastSelectedRotation.x, lastSelectedRotation.y, lastSelectedRotation.z);
                }
            }
            PlayerPrefs.SetFloat(name + "X", transform.position.x);         // zapisanie pozycji statku do plikow
            PlayerPrefs.SetFloat(name + "Z", transform.position.z);
            PlayerPrefs.SetFloat(name + "Rotation", transform.eulerAngles.y);
            InGameUI.instance.isDraged = false;
        }
    }

    protected void OnMouseDrag()          // gdy trzymany jest LMP
    {
        if (GameManagment.instance.gameState == 0 && InGameUI.isPaused == false)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
        {
            Vector3 newPosition = MouseWorldPosition() + offset; // nowa pozycja obiektu bazujaca na pozycji myszy i offsetu wzgledem MainCollider
            Vector3 positionOffset = mainCollider.transform.position - transform.position; // uwzglednienie przesuniecia
            transform.position = newPosition - positionOffset;

            if (Input.GetKeyDown(KeyCode.A))
            {
                shipRotation = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                shipRotation = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
            }
        }

    }

    Vector3 MouseWorldPosition()        // uzyskanie pozycji myszy
    {
        var mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
    // koniec funkcji ship drag
}
