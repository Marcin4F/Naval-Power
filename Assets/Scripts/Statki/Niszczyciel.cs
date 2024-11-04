using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nisczyciel : Ship
{
    public Nisczyciel() //konstruktor
    {
        name = "Niszczyciel";
        move = 5;
        hp = 5;
        zanurzenie = 1;
        fieldPosition = new Vector3(5.5f, 0.5f, -2.5f);
    }

    private void Awake()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("NiszczycielX"), 0.5f, PlayerPrefs.GetFloat("NiszczycielZ"));
        fieldPosition = transform.position;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("NiszczycielX");
        PlayerPrefs.DeleteKey("NiszczycielZ");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("NiszczycielX", transform.position.x);
        PlayerPrefs.SetFloat("NiszczycielZ", transform.position.z);
        //PlayerPrefs.Save();
    }
}
