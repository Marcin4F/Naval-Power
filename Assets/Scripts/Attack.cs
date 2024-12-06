using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    public GameObject mapa;

    void Awake()
    {
        instance = this;
    }

    public void Attacking()
    {
        GameObject mapa1 = Instantiate(mapa);
    }
}
