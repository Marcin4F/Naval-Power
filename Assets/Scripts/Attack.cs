using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    public GameObject mapa;
    private GameObject mapa1;

    void Awake()
    {
        instance = this;
    }

    public void Attacking()
    {
        mapa1 = Instantiate(mapa);
    }

    public void QuitAttacking()
    {
        Destroy(mapa1);
        mapa1 = null;
    }
}
