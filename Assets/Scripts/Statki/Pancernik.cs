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
        size = 5;
        shipID = 0;
    }

    public void Ability()
    {
        Attack.instance.Attacking();
    }

    public void UsingAbility(Vector3 position)
    {
        Debug.Log(position);
    }

    public void StopAbility()
    {
        Attack.instance.QuitAttacking();
    }
}
