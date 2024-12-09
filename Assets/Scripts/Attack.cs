using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    private ReyCastSelecter reyCastSelecter;

    private int shipsNumber, indeks;
    private Vector3[] positions;
    public GameObject mapa, znacznik, znacznik2;
    private GameObject mapa1;
    private GameObject[] znaczniki, znaczniki2;

    void Awake()
    {
        instance = this;
        reyCastSelecter = GetComponent<ReyCastSelecter>();
        shipsNumber = GameManagment.instance.shipsNumber;
        znaczniki = new GameObject[shipsNumber];
        znaczniki2 = new GameObject[shipsNumber];
        positions = new Vector3[shipsNumber];
        indeks = SceneManager.GetActiveScene().buildIndex;
        //MarkFields();

    }

    public void Attacking()
    {
        mapa1 = Instantiate(mapa);
        reyCastSelecter.isAttacking = true;
        for (int i = 0; i < shipsNumber; i++)
        {
            if (PlayerPrefs.HasKey("ZnacznikiX" + indeks + i))
            {
                Vector3 pos = new Vector3(PlayerPrefs.GetFloat("ZnacznikiX" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiY" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiZ" + indeks + i));
                znaczniki2[i] = Instantiate(znacznik2);
                znaczniki2[i].transform.position = pos;
            }
            if (GameManagment.instance.attackFields[i, 0] != null)
            {
                znaczniki[i] = Instantiate(znacznik);
                znaczniki[i].transform.position = positions[i];
                PlayerPrefs.SetFloat("ZnacznikiX" + indeks + i, positions[i].x);
                PlayerPrefs.SetFloat("ZnacznikiY" + indeks + i, positions[i].y);
                PlayerPrefs.SetFloat("ZnacznikiZ" + indeks + i, positions[i].z);
            }
        }
    }

    public void QuitAttacking()
    {
        Destroy(mapa1);
        for (int i = 0; i < shipsNumber; i++)
        {
            Destroy(znaczniki[i]);
            Destroy(znaczniki2[i]);
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
        
        if (!positions.Contains(position))
        {
            Destroy(znaczniki2[shipID]);
            znaczniki[shipID].transform.position = position;
            positions[shipID] = position;
            PlayerPrefs.SetFloat("ZnacznikiX" + indeks + shipID, positions[shipID].x);
            PlayerPrefs.SetFloat("ZnacznikiY" + indeks + shipID, positions[shipID].y);
            PlayerPrefs.SetFloat("ZnacznikiZ" + indeks + shipID, positions[shipID].z);
            GameManagment.instance.attackFields[shipID, 0] = fieldName;
        }
    }

    public void MarkFields()
    {
        for (int i = 0; i < shipsNumber; i++)
        {
            if (PlayerPrefs.HasKey("ZnacznikiX" + indeks + i))
            {
                Vector3 pos = new Vector3(PlayerPrefs.GetFloat("ZnacznikiX" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiY" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiZ" + indeks + i));
                znaczniki2[i] = Instantiate(znacznik2);
                znaczniki2[i].transform.position = pos;
            }
        }
    }
}
