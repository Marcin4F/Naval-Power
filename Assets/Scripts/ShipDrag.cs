using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDrag : MonoBehaviour
{
    Vector3 offset;
    private ReyCastSelecter raycast;    //zainicjalizowanie zmiennych do skryptow ReyCastSelecter i ShipPlacement
    private ShipPlacement placement;

    private void Start()
    {
        raycast = FindObjectOfType<ReyCastSelecter>();  // uzyskanie dostepu do skryptow
        placement = FindObjectOfType<ShipPlacement>();
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
            placement.Placement();                      // wywolanie funkcji Placement, OBECNIE NIE DZIALA W PELNI
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
