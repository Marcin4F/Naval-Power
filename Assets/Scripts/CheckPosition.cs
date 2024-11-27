using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPosition : MonoBehaviour
{
    public bool ValidPosition(int size, string nazwaPola, float rotacja)
    {
        int numer = int.Parse(nazwaPola.Substring(1));
        char litera = nazwaPola[0];
        int polowaRozmiaru = size / 2;

        if (rotacja == 0 || rotacja == -180 || rotacja == 180)
        {
            for (int i = 0; i < size; i++)
            {
                int numerPola = numer - polowaRozmiaru + i;
                if (numerPola < 1 || numerPola > 16)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                char literaPola = (char)(litera - polowaRozmiaru + i);

                if ((int)literaPola < 65 || (int)literaPola > 80)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
