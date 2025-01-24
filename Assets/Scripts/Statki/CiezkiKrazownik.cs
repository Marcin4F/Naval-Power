using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiezkiKrazownik : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8;
    private ParticleSystem[] fire;
    public CiezkiKrazownik() //konstruktor
    {
        move = 2;
        hp = 5;
        zanurzenie = 2;
        size = 5;
        shipID = 2;
    }

    private void Awake()
    {
        fire = new ParticleSystem[] { fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8};
        for (int i = 0; i < 8; i++)
        {
            fire[i].Stop();
        }
    }

    public void Fire()
    {
        for (int i = 0; i < 8; i++)
        {
            Functions.instance.RestartParticles(fire[i]);
        }
    }

}