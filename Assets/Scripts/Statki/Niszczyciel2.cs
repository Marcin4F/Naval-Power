using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niszczyciel2 : Ship
{
    public Niszczyciel2() //konstruktor
    {
        name = "Niszczyciel";
        move = 5;
        hp = 5;
        zanurzenie = 1;
        fieldPosition = new Vector3(5.5f, 0.5f, -2.5f);
    }

    private void Awake()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("Niszczyciel2X"), 0.5f, PlayerPrefs.GetFloat("Niszczyciel2Z"));
        fieldPosition = transform.position;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Niszczyciel2X");
        PlayerPrefs.DeleteKey("Niszczyciel2Z");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("Niszczyciel2X", transform.position.x);
        PlayerPrefs.SetFloat("Niszczyciel2Z", transform.position.z);
        //PlayerPrefs.Save();
    }
}
