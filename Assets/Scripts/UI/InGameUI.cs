using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//aaaaaaaaaaaaaaaaaaaaa

public class InGameUI : MonoBehaviour
{
    [SerializeField] Button endTurn;        // inicjalizacja przyciskow na scenie

    // Start is called before the first frame update
    void Start()
    {
        endTurn.onClick.AddListener(EndingTurn);    // dodanie "sluchacza" na przycisku, aktywuje sie w momencie klikniecia
    }

    private void EndingTurn()       // odwolanie do skryptu ScenesManager i wywolanie odpowiedniej funkcji
    {
        ScenesManager.instance.EndTurn();       // NA PRZYSZLOSC: jezeli chcemy odwolac sie do innej sceny to: ScenesManager.instance.LoadScene(ScenesManager.Scene.'Nazwa z enum');
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ScenesManager.instance.MainMenu();
        }
    }
}
