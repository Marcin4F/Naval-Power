using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    private ReyCastSelecter reyCastSelecter;

    private int shipsNumber;
    private Vector3[] positions;
    public GameObject mapa, znacznik;
    private GameObject mapa1;
    private GameObject[] znaczniki;

    void Awake()
    {
        instance = this;
        reyCastSelecter = GetComponent<ReyCastSelecter>();
        shipsNumber = GameManagment.instance.shipsNumber;
        znaczniki = new GameObject[shipsNumber];
        positions = new Vector3[shipsNumber];
    }

    public void Attacking()
    {
        mapa1 = Instantiate(mapa);
        reyCastSelecter.isAttacking = true;
        for (int i = 0; i < shipsNumber; i++)
        {
            if (GameManagment.instance.attackFields[i, 0] != null)
            {
                znaczniki[i] = Instantiate(znacznik);
                znaczniki[i].transform.position = positions[i];

            }
        }
    }

    public void QuitAttacking()
    {
        Destroy(mapa1);
        for (int i = 0; i < shipsNumber; i++)
        {
            Destroy(znaczniki[i]);
        }
        mapa1 = null;
        reyCastSelecter.isAttacking = false;
    }

    public void ChooseAttackField(string fieldName, Transform lastSelected, Dictionary<string, int> shipsID, Vector3 position)
    {
        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);
        if(GameManagment.instance.attackFields[shipID, 0] == null)
        {
            znaczniki[shipID] = Instantiate(znacznik);
        }
        
        znaczniki[shipID].transform.position = position;
        positions[shipID] = position;
        GameManagment.instance.attackFields[shipID, 0] = fieldName;
    }
}
