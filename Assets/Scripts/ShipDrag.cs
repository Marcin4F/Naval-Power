using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShipDrag : MonoBehaviour
{
    Vector3 offset;
    private ReyCastSelecter raycast;    //zainicjalizowanie zmiennych do skryptow ReyCastSelecter i ShipPlacement
    private Collider[] childColliders;
    private Collider mainCollider;
    private Collider[] nearbyFields;
    private Collider nearestField;

    private void Start()
    {
        raycast = FindObjectOfType<ReyCastSelecter>();  // uzyskanie dostepu do skryptu Reycast
 
        childColliders = GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));      //uzyskanie colliderow dzieci i znaleznie MainCollider
    }

    private void OnMouseDown()      // klikniecie LPM
    {
        if (raycast != null && raycast.mode == 1)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie ReyCastSelecter
            offset = transform.position - MouseWorldPosition();     // wyliczenie offsetu
    }

    private void OnMouseUp()        // puszczanie LPM
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
        }
    }

    void OnMouseDrag()          // gdy trzymany jest LMP
    {
        if (raycast != null && raycast.mode == 1)                   // sprawdzenie czy wybrany jest odpowiedni tryb w skrypcie ReyCastSelecter
            transform.position = MouseWorldPosition() + offset;     // zmiana pozycji obiektu: aktualna pozycja myszy + wyliczony offset
    }

    Vector3 MouseWorldPosition()        // uzyskanie pozycji myszy
    {
        var mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
