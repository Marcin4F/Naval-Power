using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niszczyciel : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9;
    public ParticleSystem smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8, smoke9;
    private ParticleSystem[] fire, smoke;
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
        smoke = new ParticleSystem[] { smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8, smoke9 };
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
            Functions.instance.RestartParticles(smoke[i]);
        }
    }

    public void DisableSmoke()
    {
        for (int i = 0; i < 9; i++)
        {
            smoke[i].Clear();
            smoke[i].Stop();
        }
    }
}