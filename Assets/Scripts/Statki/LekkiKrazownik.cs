using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LekkiKrazownik : Ship
{
    public int cooldown;
    private int numer;
    public bool abilityUsed;
    private string newField;
    private char litera;

    private Vector3 newPosition;
    public Vector3[] lekkiKrazownikPositions;

    public GameObject znacznik;
    public GameObject[] znaczniki;

    public ParticleSystem fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9, fire10, fire11, fire12, fire13, fire14, fire15, fire16, fire17, fire18;
    private ParticleSystem[] fire;
    public LekkiKrazownik() //konstruktor
    {
        move = 2;
        hp = 3;
        zanurzenie = 1;
        size = 3;
        shipID = 4;
    }

    private void Awake()
    {
        znaczniki = new GameObject[5];
        lekkiKrazownikPositions = new Vector3[5];
        abilityUsed = false;
        index = SceneManager.GetActiveScene().buildIndex;

        cooldown = PlayerPrefs.GetInt("LekkiKrazownikCooldown" + index);
        if (cooldown > 0)
            cooldown--;

        fire = new ParticleSystem[] { fire1, fire2, fire3, fire4, fire5, fire6, fire7, fire8, fire9, fire10, fire11, fire12, fire13, fire14, fire15, fire16, fire17, fire18 };
        for (int i = 0; i < 18; i++)
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
        if (!Attack.instance.positions.Contains(position) && !lekkiKrazownikPositions.Contains(position) && !abilityUsed)
        {
            for (int i = 0; i < 5; i++)
            {
                litera = fieldName[0];
                numer = int.Parse(fieldName.Substring(1));
                litera = (char)(litera - i);
                newField = $"{litera}{numer}";
                znaczniki[i] = Instantiate(znacznik);
                newPosition = new Vector3(position.x, position.y, position.z + i);
                znaczniki[i].transform.position = newPosition;
                lekkiKrazownikPositions[i] = newPosition;
                GameManagment.instance.abilityFields[1, i] = newField;
            }
            abilityUsed = true;
        }
    }

    public void StopAbility()
    {
        Attack.instance.QuitAttacking();
    }

    public void Fire()
    {
        for (int i = 0; i < 18; i++)
        {
            Functions.instance.RestartParticles(fire[i]);
        }
    }
}
