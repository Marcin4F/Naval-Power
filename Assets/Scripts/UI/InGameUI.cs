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
    private int displayMoveUsed, index, displayHP, loser, cooldownValue;
    public static bool isPaused = false;
    private bool isEndTurn = true;
    public bool isDraged;
    private string displayName;

    public Button endTurn, continueGame, quit, options, goBack, quitGame;        // inicjalizacja przyciskow na scenie
    public TMP_Text nazwa, hpStatku, ruchyStatku, endTurnText, winnerText, abilityCooldown;

    public GameObject pauseMenu, optionsPanel, statekPanel, gameOverPanel, quitGamePanel, shipsNamesDisplayPanel, animationBlocker, abilityPanel;
    public RawImage shipPhoto;
    public Texture korwetePhoto, niszczycielPhoto, lekkiKPhoto, ciezkiKPhoto, pancernikPhoto; 
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
            {
                shipsNamesDisplayPanel.SetActive(false);
                gameOver();
            }
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
        if (index != 2 && GameManagment.instance.gameState == 1)
        {
            GameManagment.instance.EndAnimationStarter();
        }
        else
            ScenesManager.instance.EndTurn();
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
        SetMovementValue(movesUsed, shipName);

        if (shipName == "Korweta")
        {
            abilityPanel.SetActive(false);
            shipPhoto.texture = korwetePhoto;
        }
            
        else
            abilityPanel.SetActive(true);

        if (shipName == "Pancernik")
        {
            cooldownValue = PlayerPrefs.GetInt("PancernikCooldown" + index);
            shipPhoto.texture = pancernikPhoto;
        }
        else if (shipName == "CiezkiKrazownik")
        {
            cooldownValue = PlayerPrefs.GetInt("CiezkiKrazownikCooldown" + index);
            shipPhoto.texture = ciezkiKPhoto;
        }
        else if (shipName == "LekkiKrazownik")
        {
            cooldownValue = PlayerPrefs.GetInt("LekkiKrazownikCooldown" + index);
            shipPhoto.texture = lekkiKPhoto;
        }
        else if (shipName == "Niszczyciel")
        {
            cooldownValue = PlayerPrefs.GetInt("NiszczycielCooldown" + index);
            shipPhoto.texture = niszczycielPhoto;
        }
        if (cooldownValue > 0)
            cooldownValue--;
        if (cooldownValue == 0)
            abilityCooldown.SetText("Ability is ready!");
        else
            abilityCooldown.SetText("Ready in: " + cooldownValue.ToString() + " turn(s)");
    }

    public void SetMovementValue(int movesLeft, string shipName)
    {
        displayMoveUsed = movesLeft;
        if (shipName != "Korweta" && shipName != "Korweta(Clone)")
            ruchyStatku.SetText(displayMoveUsed.ToString() + " / 2");
        else
            ruchyStatku.SetText(displayMoveUsed.ToString() + " / 4");
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
