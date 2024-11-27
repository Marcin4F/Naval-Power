using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindNearestField : MonoBehaviour
{
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
        //aaaaaaaaaaaaaaaaaaaaa

        return nearestField;
    }
}
