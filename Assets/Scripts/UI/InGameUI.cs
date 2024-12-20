using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public int shipPlaced = 0;              // ilosc statkow postawionych na planszy
    private int displayMoveUsed;
    public static bool isPaused = false;
    private bool isEndTurn = false;
    public bool isDraged;

    public Button endTurn, continueGame, quit, options, goBack;        // inicjalizacja przyciskow na scenie
    public TMP_Text nazwa, hpStatku, ruchyStatku;

    public GameObject pauseMenu, optionsPanel, statekPanel;
    
    private Ship ship;
    public static InGameUI instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        ship = FindObjectOfType<Ship>();

        endTurn.onClick.AddListener(EndingTurn);        // dodanie "sluchacza" na przycisku, aktywuje sie w momencie klikniecia
        if(SceneManager.GetActiveScene().buildIndex != 2)
        {
            continueGame.onClick.AddListener(ResumeGame);
            quit.onClick.AddListener(ScenesManager.instance.MainMenu);
            options.onClick.AddListener(OpenOptions);
            goBack.onClick.AddListener(CloseOptions);
            pauseMenu.SetActive(false);
            optionsPanel.SetActive(false);
            statekPanel.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Escape) && !isDraged)
        {
            if(!isPaused)
            {
                PauseMenu();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // pauzowanie gry
    void PauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        endTurn.interactable = false;
    }

    // wznawianie gry
    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        if (isEndTurn)
        {
            endTurn.interactable = true;
        }
    }

    // otwarcie okienka opcji
    private void OpenOptions()
    {
        pauseMenu.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // zamkniecie okienka opcji
    private void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }

    // aktywowanie panelu z informacjami o statku
    public void SetActive(string shipName, int movesUsed)
    {
        statekPanel.SetActive(true);
        nazwa.SetText(shipName);
        hpStatku.SetText("ShipHP");
        SetMovementValue(movesUsed);
    }

    public void SetMovementValue(int movesUsed)
    {
        displayMoveUsed = 2 - movesUsed;
        ruchyStatku.SetText(displayMoveUsed.ToString() + " / 2");
    }

    // dezaktywowanie panelu z informacjami o statku
    public void DeActive()
    {
        statekPanel.SetActive(false);
    }

    // wyczyszczenie wszystkiego przy wyjsciu z gry
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
