using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;
    private ReyCastSelecter reyCastSelecter;

    private float shipRotation;
    private int size, validPosition, halfSize, shipRotationInt;
    private string fieldName;
    private Vector3 movement;
    public bool isMoving = false;

    protected Collider nearestField;

    private void Awake()
    {
        instance = this;
        reyCastSelecter = GetComponent<ReyCastSelecter>();
    }

    public void MoveForward(int movesUsed, Transform lastSelected, Dictionary<string, int> shipsID)
    {
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        shipRotationInt = (int)shipRotation;
        size = reyCastSelecter.GetSize(lastSelected);

        nearestField = Functions.instance.FindingField(lastSelected);
        fieldName = nearestField.name;
        char litera = fieldName[0];
        int numer = int.Parse(fieldName.Substring(1));
        string newField;

        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID); // Uzyskanie indeksu wybranego statku
        isMoving = true;

        if (shipRotationInt == 0)
        {
            numer++;
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(1, 0, 0);
            else
            {
                isMoving = false;
                return;
            }
        }
        else if (shipRotationInt == -90 || shipRotationInt == 270)
        {
            litera = (char)(litera - 1);
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(0, 0, 1);
            else
            {
                isMoving = false;
                return;
            }
        }
        else if (shipRotationInt == 180 || shipRotationInt == -180)
        {
            numer--;
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(-1, 0, 0);
            else
            {
                isMoving = false;
                return;
            }
        }
        else if (shipRotationInt == 90)
        {
            litera = (char)(litera + 1);
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(0, 0, -1);
            else
            {
                isMoving = false;
                return;
            }
        }

        for (int i = 0; i < Functions.instance.shipFields.Length; i++)
        {
            GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
        }

        // Uruchom korutyne do plynnego ruchu

        StartCoroutine(MoveObjectSmoothly(lastSelected, lastSelected.position, lastSelected.position + movement, 0.5f, movesUsed + 1));
    }



public void MoveBackward(Transform lastSelected, Dictionary<string, int> shipsID)
    {
        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        shipRotationInt = (int) shipRotation;
        size = reyCastSelecter.GetSize(lastSelected);

        nearestField = Functions.instance.FindingField(lastSelected);
        fieldName = nearestField.name;
        char litera = fieldName[0];
        int numer = int.Parse(fieldName.Substring(1));
        string newField;
        isMoving = true;

        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);      // uzyskanie indeksu wybranego statku

        if (shipRotationInt == 0)
        {
            numer--;
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(-1, 0, 0);
            else
            {
                isMoving = false;
                return;
            }
        }

        else if (shipRotationInt == -90 || shipRotationInt == 270)
        {
            litera = (char)(litera + 1);
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(0, 0, -1);
            else
            {
                isMoving = false;
                return;
            }
        }

        else if (shipRotationInt == 180 || shipRotationInt == -180)
        {
            numer++;
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(1, 0, 0);
            else
            {
                isMoving = false;
                return;
            }
        }

        else if (shipRotationInt == 90)
        {
            litera = (char)(litera - 1);
            newField = $"{litera}{numer}";
            validPosition = Functions.instance.ValidPosition(size, newField, shipRotationInt, GameManagment.instance.occupiedFields, shipID);

            if (validPosition == 2)
                movement = new Vector3(0, 0, 1);
            else
            {
                isMoving = false;
                return;
            }
        }

        for (int i = 0; i < Functions.instance.shipFields.Length; i++)
        {
            GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
        }

        // Uruchom korutyne do plynnego ruchu
        StartCoroutine(MoveObjectSmoothly(lastSelected, lastSelected.position, lastSelected.position + movement, 0.5f, 2));
    }

    private IEnumerator MoveObjectSmoothly(Transform target, Vector3 start, Vector3 end, float duration, int usedMovesFunction)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Upewnij sie, ze koncowa pozycja jest precyzyjnie ustawiona
        target.position = end;
        isMoving = false;
        reyCastSelecter.SaveInfo(usedMovesFunction);
    }

    public void TryRotation(char direction, Transform lastSelected, Dictionary<string, int> shipsID)       // proba dokonania rotacji statku
    {
        nearestField = Functions.instance.FindingField(lastSelected);

        shipRotation = lastSelected.transform.rotation.eulerAngles.y;
        shipRotationInt = (int) shipRotation;
        fieldName = nearestField.name;

        size = reyCastSelecter.GetSize(lastSelected);
        halfSize = size / 2;

        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);      // uzyskanie indeksu wybranego statku

        if (direction == 'A')           // sprawdzenie czy dokonanie rotacji we wskazanym kierunku jest mozliwe
        {
            validPosition = Functions.instance.ValidPosition(size, fieldName, shipRotationInt - 90, GameManagment.instance.occupiedFields, shipID);
        }
        else
        {
            validPosition = Functions.instance.ValidPosition(size, fieldName, shipRotationInt + 90, GameManagment.instance.occupiedFields, shipID);
        }

        if (validPosition == 2)         // wykonanie rotacji
        {
            Rotate(direction, shipID, lastSelected);
            reyCastSelecter.SaveInfo(2);
        }

        else if (validPosition == 1)        // jezeli aktualna pozycja nie jest wlasciwa dokonujemy przesuniecia statku o odpowiednia wartosc w odopwiednim kierunku, tak aby miescil sie na mapie
        {
            Rotate(direction, shipID, lastSelected);

            if (fieldName[0] < 'C')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - (halfSize + 65 - (int)fieldName[0]));
            }
            else if (fieldName[0] > 'N')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + (halfSize - 80 + (int)fieldName[0]));
            }
            else if (fieldName.Length != 3 && fieldName[1] < '3')
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x + (halfSize + 49 - (int)fieldName[1]), lastPosition.y, lastPosition.z);
            }
            else
            {
                Vector3 lastPosition = lastSelected.transform.position;
                lastSelected.transform.position = new Vector3(lastPosition.x - (halfSize - 54 + (int)fieldName[2]), lastPosition.y, lastPosition.z);
            }

            nearestField = Functions.instance.FindingField(lastSelected);
            fieldName = nearestField.name;
            shipRotation = lastSelected.transform.rotation.eulerAngles.y;
            shipRotationInt = (int) shipRotation;
            Functions.instance.ValidPosition(size, fieldName, shipRotationInt, GameManagment.instance.occupiedFields, shipID);
            for (int i = 0; i < Functions.instance.shipFields.Length; i++)
            {
                GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
            }
            reyCastSelecter.SaveInfo(2);
        }
    }

    public void Rotate(char direction, int shipID, Transform lastSelected)       // rotowanie statku
    {
        if (direction == 'A')
        {
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotationInt - 90, 0);
        }
        else
        {
            lastSelected.transform.rotation = Quaternion.Euler(0, shipRotationInt + 90, 0);
        }

        for (int i = 0; i < Functions.instance.shipFields.Length; i++)
        {
            GameManagment.instance.occupiedFields[shipID, i] = Functions.instance.shipFields[i];
        }
    }
}
