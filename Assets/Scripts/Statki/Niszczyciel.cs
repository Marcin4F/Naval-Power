using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niszczyciel : Ship
{
    
    public Niszczyciel() //konstruktor
    {
        move = 5;
        hp = 5;
        zanurzenie = 1;
        fieldPosition = new Vector3(5.5f, 0.5f, -2.5f);
        size = 3;
    }


}
