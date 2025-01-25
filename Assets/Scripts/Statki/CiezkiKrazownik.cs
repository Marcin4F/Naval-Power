using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiezkiKrazownik : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8;
    public ParticleSystem smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8;
    private ParticleSystem[] fire, smoke;
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
        smoke = new ParticleSystem[] { smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8 };
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
            Functions.instance.RestartParticles(smoke[i]);
        }
    }

    public void DisableSmoke()
    {
        for (int i = 0; i < 8; i++)
        {
            smoke[i].Clear();
            smoke[i].Stop();
        }
    }
}