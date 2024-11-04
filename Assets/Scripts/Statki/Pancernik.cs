using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancernik : Ship
{
    int faza = 0;
    public Pancernik() //konstruktor
    {
        name = "Pancernik";
        move = 5;
        hp = 5;
        zanurzenie = 2;
        fieldPosition = new Vector3(0.032f, 0.5f, 0.83f);
    }

    private void Awake()
    {
        faza = PlayerPrefs.GetInt("Faza1");
        if(faza == 0)
        {
            PlayerPrefs.DeleteKey("PancernikX");
            PlayerPrefs.DeleteKey("PancernikZ");
            faza = 1;
            PlayerPrefs.SetInt("Faza1", faza);
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("PancernikX"), 0.5f, PlayerPrefs.GetFloat("PancernikZ"));
            fieldPosition = transform.position;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("PancernikX", transform.position.x);
        PlayerPrefs.SetFloat("PancernikZ", transform.position.z);
    }
}
