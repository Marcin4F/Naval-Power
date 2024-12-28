using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    public static Attack instance;

    private int shipsNumber, indeks, maxSize;
    private float[] newPosition;
    public Vector3[] positions;

    public GameObject mapa, znacznik, znacznik2, znacznik3, znacznikZniszczonych; 
    private GameObject mapa1;
    private GameObject[] znaczniki, znaczniki2;
    private GameObject[,] znacznikiZniszczonych;

    private ReyCastSelecter reyCastSelecter;
    private Pancernik pancernik;

    void Awake()
    {
        instance = this;
        reyCastSelecter = GetComponent<ReyCastSelecter>();
        pancernik = FindObjectOfType<Pancernik>();

        shipsNumber = GameManagment.instance.shipsNumber;
        maxSize = GameManagment.instance.maxSize;
        znaczniki = new GameObject[shipsNumber];
        znaczniki2 = new GameObject[shipsNumber];
        znacznikiZniszczonych = new GameObject[shipsNumber, maxSize];
        positions = new Vector3[shipsNumber];
        indeks = SceneManager.GetActiveScene().buildIndex;
    }

    public void Attacking()
    {
        mapa1 = Instantiate(mapa);
        for (int i = 0; i < shipsNumber; i++)
        {
            if (PlayerPrefs.HasKey("ZnacznikiX" + indeks + i))
            {
                Vector3 pos = new Vector3(PlayerPrefs.GetFloat("ZnacznikiX" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiY" + indeks + i), PlayerPrefs.GetFloat("ZnacznikiZ" + indeks + i));
                int type = 0;
                if(indeks == 1)
                {
                    type = PlayerPrefs.GetInt("Znacznik2" + 3 + i);
                }
                else
                {
                    type = PlayerPrefs.GetInt("Znacznik2" + 1 + i);
                }
                if(type == 0)
                    znaczniki2[i] = Instantiate(znacznik2);
                else
                    znaczniki2[i] = Instantiate(znacznik3);
                znaczniki2[i].transform.position = pos;
            }
            if (GameManagment.instance.attackFields[i, 0] != null)
            {
                znaczniki[i] = Instantiate(znacznik);
                znaczniki[i].transform.position = positions[i];
            }
            if (GameManagment.instance.enemyDestroyedFields[i, 0] != "")
            {
                for(int j = 0; j < maxSize; j++)
                {
                    newPosition = Functions.instance.FieldToWorldPosition(GameManagment.instance.enemyDestroyedFields[i,j]);
                    znacznikiZniszczonych[i, j] = Instantiate(znacznikZniszczonych);
                    znacznikiZniszczonych[i, j].transform.position = new Vector3(newPosition[0], 1.65f, newPosition[1]);
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (GameManagment.instance.abilityFields[0, i] != null)
            {
                pancernik.znaczniki[i] = Instantiate(znacznik);
                pancernik.znaczniki[i].transform.position = pancernik.pancernikPositions[i];
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
            for (int j = 0; j < maxSize; j++)
            {
                Destroy(znacznikiZniszczonych[i, j]);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            Destroy(pancernik.znaczniki[i]);
        }
        mapa1 = null;
        reyCastSelecter.isAttacking = false;
        reyCastSelecter.isUsingAbility = false;
    }

    public void ChooseAttackField(string fieldName, Transform lastSelected, Dictionary<string, int> shipsID, Vector3 position)
    {
        string name = lastSelected.name;
        shipsID.TryGetValue(name, out int shipID);
        if (GameManagment.instance.attackFields[shipID, 0] == null)
        {
            znaczniki[shipID] = Instantiate(znacznik);
        }

        if (!positions.Contains(position) && !pancernik.pancernikPositions.Contains(position))
        {
            znaczniki[shipID].transform.position = position;
            positions[shipID] = position;
            GameManagment.instance.attackFields[shipID, 0] = fieldName;
        }
    }
}
