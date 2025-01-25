using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Korweta : Ship
{
    public ParticleSystem fire1, fire2, fire3, fire4, fire5;
    public ParticleSystem smoke1, smoke2, smoke3, smoke4, smoke5;
    private ParticleSystem[] fire, smoke;
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
        smoke = new ParticleSystem[] { smoke1, smoke2, smoke3, smoke4, smoke5 };
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
            Functions.instance.RestartParticles(smoke[i]);
        }
    }

    public void DisableSmoke()
    {
        for (int i = 0; i < 5; i++)
        {
            smoke[i].Clear();
            smoke[i].Stop();
        }
    }
}
