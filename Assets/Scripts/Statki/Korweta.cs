using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Korweta : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5;
    private ParticleSystem[] fire;
    public Korweta() //konstruktor
    {
        move = 4;
        hp = 3;
        zanurzenie = 1;
        size = 3;
        shipID = 3;
    }

    private void Awake()
    {
        fire = new ParticleSystem[] { fire1, fire2, fire3, fire4, fire5 };
        for (int i = 0; i < 5; i++)
        {
            fire[i].Stop();
        }
    }

    public void Fire()
    {
        for (int i = 0; i < 5; i++)
        {
            Functions.instance.RestartParticles(fire[i]);
        }
    }
}
