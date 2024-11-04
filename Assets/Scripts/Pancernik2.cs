using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancernik2 : Ship
{
    public Pancernik2() //konstruktor
    {
        name = "Pancernik";
        move = 5;
        hp = 5;
        zanurzenie = 2;
        fieldPosition = new Vector3(0.032f, 0.5f, 0.83f);
    }

    private void Awake()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("Pancernik2X"), PlayerPrefs.GetFloat("Pancernik2Y"), PlayerPrefs.GetFloat("Pancernik2Z"));
        fieldPosition = transform.position;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Pancernik2X");
        PlayerPrefs.DeleteKey("Pancernik2Y");
        PlayerPrefs.DeleteKey("Pancernik2Z");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("Pancernik2X", transform.position.x);
        PlayerPrefs.SetFloat("Pancernik2Y", transform.position.y);
        PlayerPrefs.SetFloat("Pancernik2Z", transform.position.z);
        //PlayerPrefs.Save();
    }
}
