using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlacement : MonoBehaviour
{
    public GameObject[] fields;
    public GameObject nearestField;
    float distance;
    float minDistance = 10000f;

    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        fields = GameObject.FindGameObjectsWithTag("Field");

        for (int i = 0; i < fields.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, fields[i].transform.position);

            if (distance < minDistance)
            {
                nearestField = fields[i];
                minDistance = distance;
            }
            if (distance < 0.5f || counter == 4)
                break;
            if (distance < 1f)
                counter++;
        }

        Vector3 shipPosition = transform.parent.position;
        shipPosition = new Vector3 (nearestField.transform.position.x,  shipPosition.y, nearestField.transform.position.z);
        transform.parent.position = shipPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
