using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class ReyCastSelecter : MonoBehaviour
{
    protected bool isSelected = false, movementPossible;
    public bool isAttacking = false, isUsingAbility = false;
    protected int sceneIndex, validPosition, gameState, tmp, size, halfSize, movesLeft;
    protected float shipRotation;
    protected string key, fieldName, selectedName;
    protected Vector3 movment;

    protected Collider nearestField;
    protected Collider[] childColliders;
    protected Collider[] childFieldColliders;   // lista wlasciwych colliderow odpowiadajacych za rozmiar
    private Transform lastSelected;

    private Pancernik pancernik;

    Dictionary<string, int> shipsID = new Dictionary<string, int>();        // slownik z nazwami statkow i odpowiadajacymi im indeksami

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameState = GameManagment.instance.gameState;

        // UWAGA shipsNames wypelnic w kolejnosci wartosci shipID z klas statkow
        string[] shipsNames = { "Pancernik(Clone)", "Niszczyciel(Clone)", "CiezkiKrazownik(Clone)", "Korweta(Clone)", "LekkiKrazownik(Clone)" };             // umozliwienie wykonywania rotacji na poczatku tury

        tmp = 0;
        foreach (string shipName in shipsNames)
        {
            key = "PossibleMovement" + shipName + sceneIndex;
            if (shipName != "Korweta(Clone)")
            {
                PlayerPrefs.SetInt(key, 2);
            }
            else if (shipName == "Korweta(Clone)")
            {
                PlayerPrefs.SetInt(key, 4);
            }

            shipsID[shipName] = tmp++;          // uzupelnienie slownika
        }
    }

    void Update()
    {
        if (!Movement.instance.isMoving && !InGameUI.isPaused && !GameManagment.instance.animationPlaying)
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
                            lastSelected.Translate(0, -0.5f, 0);
                            isSelected = false;
                            Attack.instance.QuitAttacking();
                            InGameUI.instance.DeActive();
                        }
                        var rayPos = raycastHit.transform;      // uzyskanie pozycji statku
                        lastSelected = rayPos;                  // zapisanie statku ktory wybralismy, by moc pozniej go odznaczyc
                        rayPos.Translate(0, 0.5f, 0);              // podniesienie okretu (tymczasowe pokazanie wybrania)
                        isSelected = true;
                        
                        selectedName = lastSelected.name.Remove(lastSelected.name.Length - 7, 7);
                        movesLeft = PlayerPrefs.GetInt("PossibleMovement" + lastSelected.name + sceneIndex);
                        size = GetSize(lastSelected);
                        InGameUI.instance.SetActive(selectedName, movesLeft, size);
                    }

                    else if (raycastHit.transform.CompareTag("FieldMapaWyb"))
                    {
                        if (isAttacking)
                            Attack.instance.ChooseAttackField(raycastHit.transform.name, lastSelected, shipsID, raycastHit.transform.position);
                        else if (isUsingAbility)
                            pancernik.UsingAbility(raycastHit.transform.name, raycastHit.transform.position);
                    }

                    else if (lastSelected != null)              // jezeli promien trafil w cos innego odznaczamy ostatnio wybrany statek
                    {
                        lastSelected.Translate(0, -0.5f, 0);
                        lastSelected = null;
                        isSelected = false;
                        Attack.instance.QuitAttacking();
                        InGameUI.instance.DeActive();
                    }
                }

                else if (lastSelected != null)               // jezeli promien w nic nie trafil odznaczamy ostatnio wybrany statek
                {
                    lastSelected.Translate(0, -0.5f, 0);
                    lastSelected = null;
                    isSelected = false;
                    Attack.instance.QuitAttacking();
                    InGameUI.instance.DeActive();
                }
            }

            if (isSelected)
            {
                movesLeft = PlayerPrefs.GetInt("PossibleMovement" + lastSelected.name + sceneIndex);
                if (movesLeft >= 2)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        movementPossible = Movement.instance.TryRotation(movesLeft, 'A', lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesLeft - 2, lastSelected.name);
                    }

                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        movementPossible = Movement.instance.TryRotation(movesLeft, 'D', lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesLeft - 2, lastSelected.name);
                    }

                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        movementPossible = Movement.instance.MoveBackward(movesLeft, lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesLeft - 2, lastSelected.name);
                    }
                }

                if (movesLeft > 0)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        movementPossible = Movement.instance.MoveForward(movesLeft, lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesLeft - 1, lastSelected.name);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E) && !isAttacking && !isUsingAbility)
                {
                    isAttacking = true;
                    Attack.instance.Attacking();
                }

                else if (Input.GetKeyDown(KeyCode.E) && isAttacking)
                {
                   isAttacking = false;
                    Attack.instance.QuitAttacking();
                }

                else if (Input.GetKeyDown(KeyCode.Space) && !isUsingAbility && !isAttacking )
                {
                    if (lastSelected.name == "Pancernik(Clone)")
                    {
                        pancernik = lastSelected.GetComponent<Pancernik>();
                        if (pancernik.cooldown == 0)
                        {
                            isUsingAbility = true;
                            pancernik.Ability();

                        }
                    }
                }

                else if (Input.GetKeyDown(KeyCode.Space) && isUsingAbility)
                {
                    if (lastSelected.name == "Pancernik(Clone)")
                    {
                        isUsingAbility = false;
                        pancernik = lastSelected.GetComponent<Pancernik>();
                        pancernik.StopAbility();
                    }
                }

            }
            // do debugowania POZNIEJ USUNAC
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayerPrefs.SetInt("ShipsLeft3", 0);
            }
        }

        static Ray GetRay()         // stworzenie promienia od kursora
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }

    public void SaveInfo(int moveLeft)
    {
        PlayerPrefs.SetInt("PossibleMovement" + lastSelected.name + sceneIndex, moveLeft);      // zapisanie ze dany statek dokonal ruchu
        name = lastSelected.name.Remove(lastSelected.name.Length - 7, 7);                       // 7 zeby usunac napis "(Clone)"
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        PlayerPrefs.SetFloat(name + sceneIndex + "Rotation", shipRotation);
        PlayerPrefs.SetFloat(name + sceneIndex + "X", lastSelected.transform.position.x);
        PlayerPrefs.SetFloat(name + sceneIndex + "Z", lastSelected.transform.position.z);
    }

    public int GetSize(Transform lastSelected)
    {
        childColliders = lastSelected.GetComponentsInChildren<Collider>();
        childFieldColliders = childColliders
            .Where(collider => collider.CompareTag("MainCollider") || collider.CompareTag("FieldCollider"))
            .ToArray();
        size = childColliders.Length - 1;           // nie wiadomo czemu musi byc -1
        return size;
    }
}