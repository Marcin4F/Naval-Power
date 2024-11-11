using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancernik : Ship
{
    public Pancernik() //konstruktor
    {
        move = 5;
        hp = 5;
        zanurzenie = 2;
        fieldPosition = new Vector3(0.032f, 0.5f, 0.83f);
    }
}
