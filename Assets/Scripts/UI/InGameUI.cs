using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public int shipPlaced = 0;              // ilosc statkow postawionych na planszy
    public static bool isPaused = false;
    private bool isEndTurn = false;

    public Button endTurn, continueGame, quit;        // inicjalizacja przyciskow na scenie
    public GameObject pauseMenu;
    private Ship ship;

    // Start is called before the first frame update
    void Awake()
    {
        ship = FindObjectOfType<Ship>();

        endTurn.onClick.AddListener(EndingTurn);        // dodanie "sluchacza" na przycisku, aktywuje sie w momencie klikniecia
        if(SceneManager.GetActiveScene().buildIndex != 2)
        {
            continueGame.onClick.AddListener(ResumeGame);
            quit.onClick.AddListener(ScenesManager.instance.MainMenu);
            pauseMenu.SetActive(false);
        }
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
        if (GameManagment.instance.gameState == 0 && shipPlaced == GameManagment.instance.shipsNumber && isPaused == false)        // jezeli wszystkie statki zostaly umieszczone na planszy to umozliwiamy zakonczenie pierwszej tury, UWAGA ZMIENIC 2 NA ILOSC STATKOW W GRZE
        {
            endTurn.interactable = true;
            isEndTurn = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PauseMenu();
            }
            else
            {
                ResumeGame();
            }
            //ScenesManager.instance.MainMenu();
        }
    }

    void PauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        endTurn.enabled = false;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Canvas.ForceUpdateCanvases();
        Time.timeScale = 1.0f;
        isPaused = false;
        if (isEndTurn)
        {
            endTurn.enabled = true;
        }
        if (ship.isDraged)
        {
            ship.OnMouseUp();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
