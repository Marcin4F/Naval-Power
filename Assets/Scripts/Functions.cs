using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Functions : MonoBehaviour
{
    public string[] shipFields;
    public static Functions instance;

    private void Awake()
    {
        instance = this;
    }
    public bool ValidPosition(int size, string nazwaPola, float rotacja, string[,] occupiedFields, int shipID)
    {
        int numer = int.Parse(nazwaPola.Substring(1));
        char litera = nazwaPola[0];
        int polowaRozmiaru = size / 2;
        string newField;
        shipFields = new string[size];

        if (rotacja == 0 || rotacja == -180 || rotacja == 180 || rotacja == 360)
        {
            for (int i = 0; i < size; i++)
            {
                int numerPola = numer - polowaRozmiaru + i;
                if (numerPola < 1 || numerPola > 16)
                {
                    return false;
                }
                newField = $"{litera}{numerPola}";
                if (occupiedFields != null)
                {
                    if (Enumerable.Range(0, occupiedFields.GetLength(0))                // Iterujemy po wszystkich wierszach
                     .Where(row => row != shipID)                                       // Pomijamy wiersz shipId
                     .Any(row => Enumerable.Range(0, occupiedFields.GetLength(1))       // Iterujemy po kolumnach w danym wierszu
                     .Any(col => occupiedFields[row, col] == newField)))                // Sprawdzamy, czy newField istnieje
                    {
                        return false;                                                   // Jeœli znaleziono newField w innym wierszu, zwracamy false
                    }
                }
                shipFields[i] = newField;
            }
            return true;
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                char literaPola = (char)(litera - polowaRozmiaru + i);

                if ((int)literaPola < 65 || (int)literaPola > 80)
                {
                    return false;
                }
                newField = $"{literaPola}{numer}";
                if (occupiedFields != null)
                {
                    if (Enumerable.Range(0, occupiedFields.GetLength(0))                // Iterujemy po wszystkich wierszach
                     .Where(row => row != shipID)                                       // Pomijamy wiersz shipId
                     .Any(row => Enumerable.Range(0, occupiedFields.GetLength(1))       // Iterujemy po kolumnach w danym wierszu
                     .Any(col => occupiedFields[row, col] == newField)))                // Sprawdzamy, czy newField istnieje
                    {
                        return false;                                                   // Jeœli znaleziono newField w innym wierszu, zwracamy false
                    }
                }
                shipFields[i] = newField;
            }
            return true;
        }

    }
    public Collider FindingField(Transform Object)
    {
        Collider[] nearbyFields;
        Collider nearestField;
        Collider mainCollider;
        Collider[] childColliders;

        childColliders = Object.GetComponentsInChildren<Collider>();
        mainCollider = childColliders.FirstOrDefault(collider => collider.CompareTag("MainCollider"));

        nearbyFields = Physics.OverlapSphere(mainCollider.transform.position, 1f)       // tworzymy sfere wokol mainCollider z promieniem 1f i znajdujemy wszystkie pola Fields
         .Where(collider => collider.CompareTag("Field"))
          .ToArray();

        nearestField = nearbyFields.OrderBy(field => Vector3.Distance(mainCollider.transform.position, field.transform.position))   // znalezienie najblizszego pola (sortujemy po dystansie mainCollider i danego pola
            .FirstOrDefault();

        return nearestField;
    }

    public string ArrayToString(string[,] array)        // zamiana tablicy na zmienna string. Znak ';' oddziela pola danego wiersza, znak '?' oddziela kolejne wiersze
    {
        string changed = "";
        for (int i = 0; i < GameManagment.instance.shipsNumber; i++)
        {
            for (int j = 0; j < GameManagment.instance.maxSize; j++)
            {
                changed = changed + array[i, j] + ";";
            }
            changed = changed + "?";
        }

        return changed;
    }

    public string[,] StringToArray(string changed)      // zamiana zmiennej string na tablice
    {
        string[,] occupiedFields;
        occupiedFields = new string[GameManagment.instance.shipsNumber, GameManagment.instance.maxSize];
        string[] tmp = changed.Split('?');

        for(int i = 0; i < GameManagment.instance.shipsNumber; i++)
        {
            string[] tmp2 = tmp[i].Split(';');
            for (int j = 0; j < GameManagment.instance.maxSize; j++)
            {
                occupiedFields[i,j] = tmp2[j];
            }
        }

        return occupiedFields;
    }
}
