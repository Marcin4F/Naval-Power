using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancernik2 : Ship
{
    int faza = 0;
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
        faza = PlayerPrefs.GetInt("Faza3");
        if (faza == 0)
        {
            PlayerPrefs.DeleteKey("Pancernik2X");
            PlayerPrefs.DeleteKey("Pancernik2Z");
            faza = 1;
            PlayerPrefs.SetInt("Faza3", faza);
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("Pancernik2X"), 0.5f, PlayerPrefs.GetFloat("Pancernik2Z"));
            fieldPosition = transform.position;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("Pancernik2X", transform.position.x);
        PlayerPrefs.SetFloat("Pancernik2Z", transform.position.z);
    }
}
