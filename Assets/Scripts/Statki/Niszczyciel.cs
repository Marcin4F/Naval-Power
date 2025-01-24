using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niszczyciel : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9;
    private ParticleSystem[] fire;
    public Niszczyciel() //konstruktor
    {
        move = 5;
        hp = 3;
        zanurzenie = 1;
        size = 3;
        shipID = 1;
    }

    private void Awake()
    {
        fire = new ParticleSystem[] { fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9 };
        for (int i = 0; i < 9; i++)
        {
            fire[i].Stop();
        }
    }

    public void Fire()
    {
        for (int i = 0; i < 9; i++)
        {
            Functions.instance.RestartParticles(fire[i]);
        }
    }
}