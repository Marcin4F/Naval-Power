using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancernik : Ship
{
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
        transform.position = new Vector3(PlayerPrefs.GetFloat("PancernikX"), 0.5f, PlayerPrefs.GetFloat("PancernikZ"));
        fieldPosition = transform.position;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("PancernikX");
        PlayerPrefs.DeleteKey("PancernikZ");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("PancernikX", transform.position.x);
        PlayerPrefs.SetFloat("PancernikZ", transform.position.z);
        //PlayerPrefs.Save();
    }
}
