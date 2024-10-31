using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlacement : MonoBehaviour
{
    public GameObject[] fields;          // tablica wszystkich pol
    public GameObject nearestField;      // najblizsze pole
    float distance;
    float minDistance = 10000f;

    public void Placement()
    {
        int counter = 0;
        fields = GameObject.FindGameObjectsWithTag("Field");     // znajdowanie wszystkich pol

        for (int i = 0; i < fields.Length; i++)          // przechodzenie po wszystkich polach
        {
            distance = Vector3.Distance(this.transform.position, fields[i].transform.position);     // liczenie dystansu od statku

            if (distance < minDistance)      // sprawdzanie czy dystans jest mniejszy od najmniejszego dotychczasowego
            {
                nearestField = fields[i];
                minDistance = distance;
            }
            if (distance < 0.5f || counter == 4)    // jezeli dystans jest mniejsyz niz 0.5 lub znalezione zostaly 4 z odlegloscia mniejsza od 1 to konczymy
                break;
            if (distance < 1f)
                counter++;
        }

        Vector3 shipPosition = transform.parent.position;        // zamiana pozycji statku na pozycje najblizszego pola
        shipPosition = new Vector3 (nearestField.transform.position.x, shipPosition.y, nearestField.transform.position.z);
        transform.parent.position = shipPosition;
    }
}
