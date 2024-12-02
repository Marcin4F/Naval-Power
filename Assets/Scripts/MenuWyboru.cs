using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWyboru : MonoBehaviour
{
    protected Ship ship;

    private void Start()
    {
        ship = FindObjectOfType<Ship> ();
    }
    private void OnCollisionExit(Collision other)
    {
        ship.shipNotPlaced--;
        Debug.Log(ship.shipNotPlaced);
    }

    private void OnCollisionEnter(Collision other)
    {
        ship.shipNotPlaced++;
        Debug.Log(ship.shipNotPlaced);
    }
}
