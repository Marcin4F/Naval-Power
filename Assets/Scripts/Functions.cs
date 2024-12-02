using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Functions : MonoBehaviour
{
    public bool ValidPosition(int size, string nazwaPola, float rotacja)
    {
        int numer = int.Parse(nazwaPola.Substring(1));
        char litera = nazwaPola[0];
        int polowaRozmiaru = size / 2;

        if (rotacja == 0 || rotacja == -180 || rotacja == 180 || rotacja == 360)
        {
            for (int i = 0; i < size; i++)
            {
                int numerPola = numer - polowaRozmiaru + i;
                if (numerPola < 1 || numerPola > 16)
                {
                    return false;
                }
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
}
