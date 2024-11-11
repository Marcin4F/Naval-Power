using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment : MonoBehaviour
{
    public GameObject pancernik;
    public GameObject niszczyciel;

    // Start is called before the first frame update
    void Awake()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 1)
        {
            GameObject pancernik1 = Instantiate(pancernik);
            Pancernik pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "pancernik1";

            GameObject niszczyciel1 = Instantiate(niszczyciel);
            Niszczyciel niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "niszczyciel1";
        }
        else if (index == 3)
        {

            GameObject pancernik2 = Instantiate(pancernik);
            Pancernik pancernikComponent2 = pancernik2.GetComponent<Pancernik>();
            pancernikComponent2.name = "pancernik2";

            GameObject niszczyciel2 = Instantiate(niszczyciel);
            Niszczyciel niszczycielComponent2 = niszczyciel2.GetComponent<Niszczyciel>();
            niszczycielComponent2.name = "niszczyciel2";

        }
    }
}
