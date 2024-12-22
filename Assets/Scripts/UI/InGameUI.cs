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
    private int displayMoveUsed, index, displayHP, loser;
    public static bool isPaused = false;
    private bool isEndTurn = true;
    public bool isDraged;
    private string displayName;

    public Button endTurn, continueGame, quit, options, goBack, quitGame;        // inicjalizacja przyciskow na scenie
    public TMP_Text nazwa, hpStatku, ruchyStatku, endTurnText, winnerText;

    public GameObject pauseMenu, optionsPanel, statekPanel, gameOverPanel, quitGamePanel, shipsNamesDisplayPanel, animationBlocker;
    Dictionary<string, string> names = new Dictionary<string, string>
    {
        ["Pancernik"] = "Battleship",
        ["CiezkiKrazownik"] = "Heavy cruiser",
        ["Niszczyciel"] = "Destroyer",
        ["LekkiKrazownik"] = "Light cruiser",
        ["Korweta"] = "Corvette"
    };

    private Ship ship;
    public static InGameUI instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        ship = FindObjectOfType<Ship>();
        index = SceneManager.GetActiveScene().buildIndex;

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
            gameOverPanel.SetActive(false);
            quitGamePanel.SetActive(false);
            if (GameManagment.instance.gameState == 1)
                shipsNamesDisplayPanel.SetActive(false);
            else if (GameManagment.instance.gameState == 0)
                animationBlocker.SetActive(false);
            else
                gameOver();
        }
        
        if (GameManagment.instance != null && GameManagment.instance.gameState == 0)
        {
            isEndTurn = false;
            endTurn.interactable = false;               // wylaczenie przycisku konca tury na poczatku gry
        }
    }



    private void Update()
    {
        if (GameManagment.instance.gameState == 0 && shipPlaced == GameManagment.instance.shipsNumber && isPaused == false)        // jezeli wszystkie statki zostaly umieszczone na planszy to umozliwiamy zakonczenie pierwszej tury, UWAGA ZMIENIC 2 NA ILOSC STATKOW W GRZE
        {
            endTurn.interactable = true;
            isEndTurn = true;
        }
        else if(GameManagment.instance.gameState == 1 && index != 2 && !GameManagment.instance.animationPlaying)
        {
            animationBlocker.SetActive(false);
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
    private void EndingTurn()       // odwolanie do skryptu ScenesManager i wywolanie odpowiedniej funkcji
    {
        ScenesManager.instance.EndTurn();       // NA PRZYSZLOSC: jezeli chcemy odwolac sie do innej sceny to: ScenesManager.instance.LoadScene(ScenesManager.Scene.'Nazwa z enum');
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

    public void gameOver()
    {
        animationBlocker.SetActive(false);
        gameOverPanel.SetActive(true);
        quitGamePanel.SetActive(true);
        endTurnText.SetText("Check other player's board");
        quitGame.onClick.AddListener(ScenesManager.instance.MainMenu);
        loser = PlayerPrefs.GetInt("Loser");
        if (loser == 1)
        {
            winnerText.SetText("Player 2 won!");
        }
        else
            winnerText.SetText("Player 1 won!");
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
    public void SetActive(string shipName, int movesUsed, int shipSize)
    {
        statekPanel.SetActive(true);
        names.TryGetValue(shipName, out displayName);
        nazwa.SetText(displayName);
        displayHP = PlayerPrefs.GetInt(shipName + "HP" + index);
        hpStatku.SetText(displayHP.ToString() + " / " + shipSize.ToString());
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
