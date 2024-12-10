using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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

    protected int validPosition;
    protected float rotacja, shipRotation;
    protected string nazwaPola;
    public bool isDraged = false;

    private InGameUI inGameUI;

    protected void Start()
    {
        inGameUI = FindObjectOfType<InGameUI>();

        childColliders = GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znaleznie MainCollider

        //zapisywanie pozycji statków do pliku
        if (GameManagment.instance.gameState == 0)
        {
            PlayerPrefs.DeleteKey(name + "X");      // usuniecie jezeli istnialy jakies pliki o podanych nazwach z wczesniejszej gry
            PlayerPrefs.DeleteKey(name + "Z");
            PlayerPrefs.DeleteKey(name + "Rotation");
            GameManagment.instance.occupiedFields = new string[GameManagment.instance.shipsNumber, GameManagment.instance.maxSize];
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
            offset = transform.position - MouseWorldPosition();     // wyliczenie offsetu
            isDraged = true;
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
                nazwaPola = nearestField.name;
                validPosition = Functions.instance.ValidPosition(size, nazwaPola, rotacja, GameManagment.instance.occupiedFields, shipID);
                
                if (validPosition == 2)
                {
                    transform.position = new Vector3(nearestField.transform.position.x, transform.position.y, nearestField.transform.position.z);       // zmieniamy pozycje statku na pozycje najblizszego pola
                    inGameUI.shipPlaced++;              // zwiekszenie ilosci postawionych statkow na planszy
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
            isDraged = false;
        }
    }

    protected void OnMouseDrag()          // gdy trzymany jest LMP
    {
        if (GameManagment.instance.gameState == 0 && InGameUI.isPaused == false)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
        {
            transform.position = MouseWorldPosition() + offset;     // zmiana pozycji obiektu: aktualna pozycja myszy + wyliczony offset

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

