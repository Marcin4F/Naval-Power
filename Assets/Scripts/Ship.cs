using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
//aaaaaaaaaaaaaaaaaaaaa

public class Ship : MonoBehaviour
{
    // zmienne podstawowe dla statków
    protected int move;
    protected int hp; // zycie
    protected int zanurzenie;
    public new string name;
    protected int size;

    public int Size
    {
        get { return size; }
    }

    protected string[] shipFields;

    // zmienne dla funkcji ship drag
    Vector3 offset;
    protected GameManagment gameManagment;
    protected Collider[] childColliders;
    protected Collider mainCollider;
    protected Collider[] nearbyFields;
    protected Collider nearestField;
    protected Vector3 lastSelectedPosition;
    protected Vector3 lastSelectedRotation;

    protected Functions functions;
    
    protected void Start()
    {
        gameManagment = FindObjectOfType<GameManagment>();  // uzyskanie dostepu do skryptu GameManagement
        functions = FindObjectOfType<Functions>();

        childColliders = GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znaleznie MainCollider

        //zapisywanie pozycji statków do pliku
        if (gameManagment.gameState == 0)
        {
            PlayerPrefs.DeleteKey(name + "X");      // usuniecie jezeli istnialy jakies pliki o podanych nazwach z wczesniejszej gry
            PlayerPrefs.DeleteKey(name + "Z");
            PlayerPrefs.DeleteKey(name + "Rotation");
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
        if (gameManagment.gameState == 0)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
        {
            lastSelectedPosition = transform.position;
            lastSelectedRotation = transform.eulerAngles;
            offset = transform.position - MouseWorldPosition();     // wyliczenie offsetu
        }
    }

    protected void OnMouseUp()        // puszczanie LPM
    {
        if (gameManagment.gameState == 0)       // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
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
                
                float rotacja = transform.rotation.eulerAngles.y;
                string nazwaPola = nearestField.name;
                bool validPosition = functions.ValidPosition(size, nazwaPola, rotacja);
                /*shipFields = new string[size];

                if (rotacja.y == 0 || rotacja.y == -180)
                {
                    for (int i = 0; i < size; i++)
                    {
                        int numerPola = numer - polowaRozmiaru + i;
                        if (numerPola < 1 || numerPola > 16)
                        {
                            transform.position = lastSelectedPosition;
                            transform.rotation = Quaternion.Euler(lastSelectedRotation.x, lastSelectedRotation.y, lastSelectedRotation.z);
                            validPosition = false;
                            break;
                        }
                        // DO DODANIA reset przy zlej pozycji
                        shipFields[i] = $"{litera}{numerPola}";
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        char literaPola = (char)(litera - polowaRozmiaru + i);

                        if ((int)literaPola < 65 || (int)literaPola > 80)
                        {
                            transform.position = lastSelectedPosition;
                            transform.rotation = Quaternion.Euler(lastSelectedRotation.x, lastSelectedRotation.y, lastSelectedRotation.z);
                            validPosition = false;
                            break;
                        }

                        shipFields[i] = $"{literaPola}{numer}";
                    }
                }*/

                if (validPosition)
                    transform.position = new Vector3(nearestField.transform.position.x, transform.position.y, nearestField.transform.position.z);       // zmieniamy pozycje statku na pozycje najblizszego pola
                else
                {
                    transform.position = lastSelectedPosition;
                    transform.rotation = Quaternion.Euler(lastSelectedRotation.x, lastSelectedRotation.y, lastSelectedRotation.z);
                }
            }
            PlayerPrefs.SetFloat(name + "X", transform.position.x);         // zapisanie pozycji statku do plikow
            PlayerPrefs.SetFloat(name + "Z", transform.position.z);
            PlayerPrefs.SetFloat(name + "Rotation", transform.eulerAngles.y);
        }
    }

    protected void OnMouseDrag()          // gdy trzymany jest LMP
    {
        if (gameManagment.gameState == 0)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie GameManagement
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

