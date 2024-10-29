using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReyCastSelecter : MonoBehaviour
{
    private Transform lastSelected;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = GetRay();
            if (Physics.Raycast(ray, out var raycastHit))
            {
                if(raycastHit.transform.CompareTag("Ship"))
                {
                    if(lastSelected != null)
                    {
                        lastSelected.Translate(0, -1, 0);
                    }
                    var rayPos = raycastHit.transform;
                    lastSelected = rayPos;
                    rayPos.Translate(0, 1, 0);
                }
                else if(lastSelected != null)
                {
                    lastSelected.Translate(0, -1 , 0);
                    lastSelected = null;
                }
            }
            else if (lastSelected != null)
            {
                lastSelected.Translate(0, -1, 0);
                lastSelected = null;
            }
        }

        Ray GetRay()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }
}