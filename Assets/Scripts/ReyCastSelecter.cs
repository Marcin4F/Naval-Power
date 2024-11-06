using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReyCastSelecter : MonoBehaviour
{
    private Transform lastSelected;
    public int mode = 0;
    bool isSelected = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))     // wybor trybu, TYMCZASOWE
        {
            mode = 0;
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            mode = 1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mode == 0) 
        {
            var ray = GetRay();
            if (Physics.Raycast(ray, out var raycastHit))       // sprawdzenie czy promien trafil w jakis obiekt
            {
                if (raycastHit.transform.CompareTag("Ship"))    // sprawdzenie czy trafiony obiekt to statek
                {
                    if (lastSelected != null)               // odznaczenie ostatnio wybranego statku
                    {
                        lastSelected.Translate(0, -1, 0);
                        isSelected = false;
                    }
                    var rayPos = raycastHit.transform;      // uzyskanie pozycji statku
                    lastSelected = rayPos;                  // zapisanie statku ktory wybralismy, by moc pozniej go odznaczyc
                    rayPos.Translate(0, 1, 0);              // podniesienie okretu (tymczasowe pokazanie wybrania)
                    isSelected = true;
                }
                else if (lastSelected != null)              // jezeli promien trafil w cos innego odznaczamy ostatnio wybrany statek
                {
                    lastSelected.Translate(0, -1, 0);
                    lastSelected = null;
                    isSelected = false;
                }
            }
            else if (lastSelected != null)               // jezeli promien w nic nie trafil odznaczamy ostatnio wybrany statek
            {
                lastSelected.Translate(0, -1, 0);
                lastSelected = null;
                isSelected=false;
            }
        }

        if(isSelected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
                lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation - 90, 0);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                float shipRotation = lastSelected.transform.rotation.eulerAngles.y;
                lastSelected.transform.rotation = Quaternion.Euler(0, shipRotation + 90, 0);
            }
        }


        static Ray GetRay()         // stworzenie promienia od kursora
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }
}