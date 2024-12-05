using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public int shipPlaced = 0;              // ilosc statkow postawionych na planszy

    [SerializeField] Button endTurn;        // inicjalizacja przyciskow na scenie

    // Start is called before the first frame update
    void Awake()
    {
        endTurn.onClick.AddListener(EndingTurn);        // dodanie "sluchacza" na przycisku, aktywuje sie w momencie klikniecia
        if (GameManagment.instance != null && GameManagment.instance.gameState == 0)
        {
            endTurn.interactable = false;               // wylaczenie przycisku konca tury na poczatku gry
        }
    }

    private void EndingTurn()       // odwolanie do skryptu ScenesManager i wywolanie odpowiedniej funkcji
    {
        ScenesManager.instance.EndTurn();       // NA PRZYSZLOSC: jezeli chcemy odwolac sie do innej sceny to: ScenesManager.instance.LoadScene(ScenesManager.Scene.'Nazwa z enum');
    }

    private void Update()
    {
        if (GameManagment.instance.gameState == 0 && shipPlaced == GameManagment.instance.shipsNumber)        // jezeli wszystkie statki zostaly umieszczone na planszy to umozliwiamy zakonczenie pierwszej tury, UWAGA ZMIENIC 2 NA ILOSC STATKOW W GRZE
            endTurn.interactable = true;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScenesManager.instance.MainMenu();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
