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
    public bool isAttacking = false;
    protected int sceneIndex, validPosition, gameState, tmp, size, halfSize, movesUsed;
    protected float shipRotation;
    protected string key, fieldName, selectedName;
    protected Vector3 movment;

    protected Collider nearestField;
    protected Collider[] childColliders;
    protected Collider[] childFieldColliders;   // lista wlasciwych colliderow odpowiadajacych za rozmiar
    private Transform lastSelected;

    Dictionary<string, int> shipsID = new Dictionary<string, int>();        // slownik z nazwami statkow i odpowiadajacymi im indeksami

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameState = GameManagment.instance.gameState;

        GameManagment.instance.attackFields = new string[GameManagment.instance.shipsNumber, 1];

        // UWAGA shipsNames wypelnic w kolejnosci wartosci shipID z klas statkow
        string[] shipsNames = { "Pancernik(Clone)", "Niszczyciel(Clone)", "CiezkiKrazownik(Clone)", "Korweta(Clone)", "LekkiKrazownik(Clone)" };             // umozliwienie wykonywania rotacji na poczatku tury PRZENIESC NA FAZE KONCOWA

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
        if (Movement.instance.isMoving == false && InGameUI.isPaused == false)
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
                            Attack.instance.QuitAttacking();
                            InGameUI.instance.DeActive();
                        }
                        var rayPos = raycastHit.transform;      // uzyskanie pozycji statku
                        lastSelected = rayPos;                  // zapisanie statku ktory wybralismy, by moc pozniej go odznaczyc
                        rayPos.Translate(0, 1, 0);              // podniesienie okretu (tymczasowe pokazanie wybrania)
                        isSelected = true;
                        
                        selectedName = lastSelected.name.Remove(lastSelected.name.Length - 7, 7);
                        movesUsed = PlayerPrefs.GetInt("PossibleMovement" + lastSelected.name + sceneIndex);
                        size = GetSize(lastSelected);
                        InGameUI.instance.SetActive(selectedName, movesUsed, size);
                    }

                    else if (raycastHit.transform.CompareTag("FieldMapaWyb"))
                    {
                        Attack.instance.ChooseAttackField(raycastHit.transform.name, lastSelected, shipsID, raycastHit.transform.position);
                    }

                    else if (lastSelected != null)              // jezeli promien trafil w cos innego odznaczamy ostatnio wybrany statek
                    {
                        lastSelected.Translate(0, -1, 0);
                        lastSelected = null;
                        isSelected = false;
                        Attack.instance.QuitAttacking();
                        InGameUI.instance.DeActive();
                    }
                }

                else if (lastSelected != null)               // jezeli promien w nic nie trafil odznaczamy ostatnio wybrany statek
                {
                    lastSelected.Translate(0, -1, 0);
                    lastSelected = null;
                    isSelected = false;
                    Attack.instance.QuitAttacking();
                    InGameUI.instance.DeActive();
                }
            }

            if (isSelected)
            {
                movesUsed = PlayerPrefs.GetInt("PossibleMovement" + lastSelected.name + sceneIndex);
                if (movesUsed == 0)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        movementPossible = Movement.instance.TryRotation('A', lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesUsed + 2);
                    }

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        movementPossible = Movement.instance.TryRotation('D', lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesUsed + 2);
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        movementPossible = Movement.instance.MoveBackward(lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesUsed + 2);
                    }
                }

                if (movesUsed != 2)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        movementPossible = Movement.instance.MoveForward(movesUsed, lastSelected, shipsID);
                        if (movementPossible)
                            InGameUI.instance.SetMovementValue(movesUsed + 1);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E) && isAttacking == false)
                {
                    Attack.instance.Attacking();
                }

                else if (Input.GetKeyDown(KeyCode.E) && isAttacking == true)
                {
                    Attack.instance.QuitAttacking();
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

    public void SaveInfo(int moveUsed)
    {
        PlayerPrefs.SetInt("PossibleMovement" + lastSelected.name + sceneIndex, moveUsed);      // zapisanie ze dany statek dokonal ruchu
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