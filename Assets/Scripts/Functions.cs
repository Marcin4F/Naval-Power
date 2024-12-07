using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Functions : MonoBehaviour
{
    public string[] shipFields;
    public static Functions instance;
    private int shipsNumber, maxSize;

    private void Awake()
    {
        instance = this;
        shipsNumber = GameManagment.instance.shipsNumber;
        maxSize = GameManagment.instance.maxSize;
    }
    public int ValidPosition(int size, string nazwaPola, float rotacja, string[,] occupiedFields, int shipID)       // zwracane wartosci: 0 -> nie mozna wykonac ruchu, inny statek na drodze
                                                                                                                    // 1 -> nie mo¿na wykonaæ ruchu, wyjscie poza krawedz mapy
                                                                                                                    // 2 -> mozna wykonac ruch
    {
        int numer = int.Parse(nazwaPola.Substring(1));
        int wynik = 2;
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
                    wynik = 1;
                }
                newField = $"{litera}{numerPola}";
                if (occupiedFields != null)
                {
                    if (Enumerable.Range(0, shipsNumber)                                    // Iterujemy po wszystkich wierszach
                     .Where(row => row != shipID)                                           // Pomijamy wiersz shipId
                     .Any(row => Enumerable.Range(0, maxSize)        // Iterujemy po kolumnach w danym wierszu
                     .Any(col => occupiedFields[row, col] == newField)))                    // Sprawdzamy, czy newField istnieje
                    {
                        return 0;                                                           // Jeœli znaleziono newField w innym wierszu, zwracamy false
                    }
                }
                shipFields[i] = newField;
            }
            return wynik;
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                char literaPola = (char)(litera - polowaRozmiaru + i);
                if ((int)literaPola < 65 || (int)literaPola > 80)
                {
                    wynik = 1;
                }
                newField = $"{literaPola}{numer}";
                if (occupiedFields != null)
                {
                    if (Enumerable.Range(0, shipsNumber)             // Iterujemy po wszystkich wierszach
                     .Where(row => row != shipID)                                           // Pomijamy wiersz shipId
                     .Any(row => Enumerable.Range(0, maxSize)        // Iterujemy po kolumnach w danym wierszu
                     .Any(col => occupiedFields[row, col] == newField)))                    // Sprawdzamy, czy newField istnieje
                    {
                        return 0;                                                           // Jeœli znaleziono newField w innym wierszu, zwracamy false
                    }
                }
                shipFields[i] = newField;
            }
            return wynik;
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

    public string ArrayToString(string[,] array, int size2)        // zamiana tablicy na zmienna string. Znak ';' oddziela pola danego wiersza, znak '?' oddziela kolejne wiersze
    {
        string changed = "";
        for (int i = 0; i < shipsNumber; i++)
        {
            for (int j = 0; j < size2; j++)
            {
                changed = changed + array[i, j] + ";";
            }
            changed = changed + "?";
        }

        return changed;
    }

    public string[,] StringToArray(string changed, int size2)      // zamiana zmiennej string na tablice
    {
        string[,] array;
        array = new string[shipsNumber, size2];
        string[] tmp = changed.Split('?');

        for(int i = 0; i < shipsNumber; i++)
        {
            string[] tmp2 = tmp[i].Split(';');
            for (int j = 0; j < size2; j++)
            {
                array[i,j] = tmp2[j];
            }
        }

        return array;
    }
}
