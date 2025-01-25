using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;

public class Pancernik : Ship
{
    private int arrayIndex = 0;
    public int cooldown;
    public bool abilityUsed;

    public Vector3[] pancernikPositions;

    public GameObject znacznik;
    public GameObject[] znaczniki;

    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9, fire10;
    public ParticleSystem smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8, smoke9, smoke10;
    private ParticleSystem[] fire, smoke;
    public Pancernik() //konstruktor
    {
        move = 5;
        hp = 5;
        zanurzenie = 2;
        size = 5;
        shipID = 0;
    }

    private void Awake()
    {
        znaczniki = new GameObject[3];
        pancernikPositions = new Vector3[3];
        abilityUsed = false;
        index = SceneManager.GetActiveScene().buildIndex;

        cooldown = PlayerPrefs.GetInt("PancernikCooldown" + index);
        if (cooldown > 0)
            cooldown--;

        fire = new ParticleSystem[] { fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9, fire10 };
        smoke = new ParticleSystem[] { smoke1, smoke2, smoke3, smoke4, smoke5, smoke6, smoke7, smoke8, smoke9, smoke10 };
        for (int i = 0; i < 10; i++)
        {
            fire[i].Stop();
        }
    }

    public void Ability()
    {
        Attack.instance.Attacking();
    }

    public void UsingAbility(string fieldName, Vector3 position)
    {
        if(arrayIndex < 3 && !Attack.instance.positions.Contains(position) && !pancernikPositions.Contains(position))
        {
            znaczniki[arrayIndex] = Instantiate(znacznik);
            znaczniki[arrayIndex].transform.position = position;
            pancernikPositions[arrayIndex] = position;
            GameManagment.instance.abilityFields[0, arrayIndex] = fieldName;
            abilityUsed = true;
            arrayIndex++;
        }
    }

    public void StopAbility()
    {
        Attack.instance.QuitAttacking();
    }

    public void Fire()
    {
        for (int i = 0; i < 10; i++)
        {
            Functions.instance.RestartParticles(fire[i]);
            Functions.instance.RestartParticles(smoke[i]);
        }
    }

    public void DisableSmoke()
    {
        for (int i = 0; i < 10; i++)
        {
            smoke[i].Clear();
            smoke[i].Stop();
        }
    }

}
