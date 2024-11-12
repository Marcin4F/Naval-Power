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
    protected int hp; // ¿ycie
    protected int zanurzenie;
    public new string name;

    protected Vector3 fieldPosition;
    protected int size;

    // zmienne dla funkcji ship drag
    Vector3 offset;
    protected ReyCastSelecter raycast;    //zainicjalizowanie zmiennych do skryptow ReyCastSelecter i ShipPlacement
    protected Collider[] childColliders;
    protected Collider mainCollider;
    protected Collider[] nearbyFields;
    protected Collider nearestField;

    int faza = 0;
    
    protected void Start()
    {
        raycast = FindObjectOfType<ReyCastSelecter>();  // uzyskanie dostepu do skryptu Reycast

        childColliders = GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znaleznie MainCollider

        //zapisywanie pozycji statków do pliku
        faza = PlayerPrefs.GetInt("Faza" + SceneManager.GetActiveScene().buildIndex);       // faza gry: 0 - pierwsza tura, nie wczytujemy pozycji statkow, 1 - kolejne tury wczytujemy pozycje z plikow
        if (faza == 0)
        {
            PlayerPrefs.DeleteKey(name + "X");      // usuniecie jezeli istnialy jakies pliki o podanych nazwach z wczesniejszej gry
            PlayerPrefs.DeleteKey(name + "Z");
            PlayerPrefs.DeleteKey(name + "Rotation");
            faza = 1;
            PlayerPrefs.SetInt("Faza" + SceneManager.GetActiveScene().buildIndex, faza);
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat(name + "X"), 0.5f, PlayerPrefs.GetFloat(name + "Z"));     // wczytanie pozycji z plikow
            transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat(name + "Rotation"), 0); 
            fieldPosition = transform.position;
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
        if (raycast != null && raycast.mode == 1)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie ReyCastSelecter
            offset = transform.position - MouseWorldPosition();     // wyliczenie offsetu
    }

    protected void OnMouseUp()        // puszczanie LPM
    {
        if (raycast != null && raycast.mode == 1)       // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie ReyCastSelecter
        {
            nearbyFields = Physics.OverlapSphere(mainCollider.transform.position, 1f)       // tworzymy sfere wokol mainCollider z promieniem 1f i znajdujemy wszystkie pola Fields
             .Where(collider => collider.CompareTag("Field"))
               .ToArray();

            if (nearbyFields.Length > 0)        // jezeli znaleziono jakies pola w ustawionym promieniu
            {
                nearestField = nearbyFields.OrderBy(field => Vector3.Distance(mainCollider.transform.position, field.transform.position))   // znalezienie najblizszego pola (sortujemy po dystansie mainCollider i danego pola
                 .FirstOrDefault();
                transform.position = new Vector3(nearestField.transform.position.x, transform.position.y, nearestField.transform.position.z);       // zmieniamy pozycje statku na pozycje najblizszego pola
            }
            PlayerPrefs.SetFloat(name + "X", transform.position.x);         // zapisanie pozycji statku do plikow
            PlayerPrefs.SetFloat(name + "Z", transform.position.z);
            PlayerPrefs.SetFloat(name + "Rotation", transform.eulerAngles.y);
        }
    }

    protected void OnMouseDrag()          // gdy trzymany jest LMP
    {
        if (raycast != null && raycast.mode == 1)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie ReyCastSelecter
            transform.position = MouseWorldPosition() + offset;     // zmiana pozycji obiektu: aktualna pozycja myszy + wyliczony offset
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            float shipRotation = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            float shipRotation = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
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

