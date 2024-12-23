using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManagment : MonoBehaviour
{
    public static GameManagment instance;

    public int gameState = 0, shipsNumber = 5, maxSize = 5, shipsLeft = 5;                              // gameState - faza gry, shipsNumber - ilosc statkow, maxSize - maksymalny rozmiar statku, shipsLeft - ile statkow pozostalo
    private int index, hitShots = 0, missedShots = 0, tmp, hitFieldsIndex = 0, missFieldsIndex = 0;     // index - indeks sceny, hitShots - ilosc trafien, missedShots - iosc strzalow nietrafionych, tmp - zmienna pomocnicza
                                                                                                        // przy sprawdzaniu trafienia, hitFieldsIndex - indeks zapisu do tablicy hitFields, missFieldsIndex - indeks zapisu do
                                                                                                        // tablicy missFields
    public string[,] occupiedFields, attackFields, fieldsUnderAttack;                                   // occupiedFields - pola zajmowanie przez statki, attackFields - pola ktore gracz chce atakowac, fieldsUnderAttack -
                                                                                                        // pola gracza atakowane przez przeciwnika
    private string field;
    private string[] hitFields, missFields;
    private float[] particlePosition;
    public bool animationPlaying;

    public GameObject pancernik, niszczyciel, ciezkiKrazownik, korweta, lekkiKrazownik, mapa;
    public ParticleSystem hitParticleHolder, missParticleHolder;
    private GameObject pancernik1, niszczyciel1, ciezkiKrazownik1, korweta1, lekkiKrazownik1, mapa1;
    private ParticleSystem[] hitParticleHolder1, missParticleHolder1;
    private Pancernik pancernikComponent1;
    private Niszczyciel niszczycielComponent1;
    private CiezkiKrazownik ciezkiKrazownikComponent1;
    private Korweta korwetaComponent1;
    private LekkiKrazownik lekkiKrazownikComponent1;
    public Material zniszczony;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        index = SceneManager.GetActiveScene().buildIndex;
        animationPlaying = true;
        hitFields = new string[shipsNumber];
        missFields = new string[shipsNumber];

        // inicjalizacja statkow

        if (index == 1)
        {
            pancernik1 = Instantiate(pancernik);
            pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "Pancernik1";

            niszczyciel1 = Instantiate(niszczyciel);
            niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "Niszczyciel1";

            ciezkiKrazownik1 = Instantiate(ciezkiKrazownik);
            ciezkiKrazownikComponent1 = ciezkiKrazownik1.GetComponent<CiezkiKrazownik>();
            ciezkiKrazownikComponent1.name = "CiezkiKrazownik1";

            korweta1 = Instantiate(korweta);
            korwetaComponent1 = korweta1.GetComponent<Korweta>();
            korwetaComponent1.name = "Korweta1";

            lekkiKrazownik1 = Instantiate(lekkiKrazownik);
            lekkiKrazownikComponent1 = lekkiKrazownik1.GetComponent<LekkiKrazownik>();
            lekkiKrazownikComponent1.name = "LekkiKrazownik1";

        }
        else if (index == 3)
        {

            pancernik1 = Instantiate(pancernik);
            pancernikComponent1 = pancernik1.GetComponent<Pancernik>();
            pancernikComponent1.name = "Pancernik3";

            niszczyciel1 = Instantiate(niszczyciel);
            niszczycielComponent1 = niszczyciel1.GetComponent<Niszczyciel>();
            niszczycielComponent1.name = "Niszczyciel3";

            ciezkiKrazownik1 = Instantiate(ciezkiKrazownik);
            ciezkiKrazownikComponent1 = ciezkiKrazownik1.GetComponent<CiezkiKrazownik>();
            ciezkiKrazownikComponent1.name = "CiezkiKrazownik3";

            korweta1 = Instantiate(korweta);
            korwetaComponent1 = korweta1.GetComponent<Korweta>();
            korwetaComponent1.name = "Korweta3";

            lekkiKrazownik1 = Instantiate(lekkiKrazownik);
            lekkiKrazownikComponent1 = lekkiKrazownik1.GetComponent<LekkiKrazownik>();
            lekkiKrazownikComponent1.name = "LekkiKrazownik3";
        }

        gameState = PlayerPrefs.GetInt("gameState" + index);       // faza gry: 0 - pierwsza tura, nie wczytujemy pozycji statkow, 1 - kolejne tury wczytujemy pozycje z plikow
        if (gameState == 0)
        {
            PlayerPrefs.SetInt("gameState" + index, 1);
            SaveHP();
        }
        else if (gameState == 1)
        {
            // wczytanie pozycji statkow oraz sprawdzenie czy statki otrzymaly obrazenia
            occupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions" + index), maxSize);
            shipsLeft = PlayerPrefs.GetInt("ShipsLeft" + index);

            pancernikComponent1.hp = PlayerPrefs.GetInt("PancernikHP" + index);
            if (pancernikComponent1.hp == 0)
            {
                pancernik1.transform.tag = "Destroyed";
                var renderer = pancernik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            niszczycielComponent1.hp = PlayerPrefs.GetInt("NiszczycielHP" + index);
            if (niszczycielComponent1.hp == 0)
            {
                niszczyciel1.transform.tag = "Destroyed";
                var renderer = niszczyciel1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            ciezkiKrazownikComponent1.hp = PlayerPrefs.GetInt("CiezkiKrazownikHP" + index);
            if (ciezkiKrazownikComponent1.hp == 0)
            {
                ciezkiKrazownik1.transform.tag = "Destroyed";
                var renderer = ciezkiKrazownik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            korwetaComponent1.hp = PlayerPrefs.GetInt("KorwetaHP" + index);
            if (korwetaComponent1.hp == 0)
            {
                korweta1.transform.tag = "Destroyed";
                var renderer = korweta1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            lekkiKrazownikComponent1.hp = PlayerPrefs.GetInt("LekkiKrazownikHP" + index);
            if (lekkiKrazownikComponent1.hp == 0)
            {
                lekkiKrazownik1.transform.tag = "Destroyed";
                var renderer = lekkiKrazownik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }

            // wczytanie pol ktore sa atakowane przez przeciwnika
            if (index == 1)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields3"), 1);
            }
            else if (index == 3)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields1"), 1);
            }

            // sprawdzenie czy przeciwnik trafil, przechodzimy po wszystkich atakowanych polach
            for (int k = 0; k < fieldsUnderAttack.Length; k++)
            {
                field = fieldsUnderAttack[k, 0];
                tmp = 0;
                
                // przechodzimy po wszystkich polach zajmowanych przez statki
                for (int i = 0; i < shipsNumber; i++)
                {
                    for (int j = 0; j < maxSize; j++)
                    {
                        // jezeli dane pole jest zajete odpowiedni statek odnosi obrazenia
                        if (field != "" && field == occupiedFields[i, j])
                        {
                            TakeDamage(i);
                            hitShots++;
                            hitFields[hitFieldsIndex++] = field;
                            PlayerPrefs.SetInt("Znacznik2" + index + k, 1);
                            tmp = 1;
                        }
                    }
                }
                if (tmp == 0 && field != "")
                {
                    PlayerPrefs.SetInt("Znacznik2" + index + k, 0);
                    missedShots++;
                    missFields[missFieldsIndex++] = field;
                }
            }
            // zapis hp statkow
            SaveHP();
            hitParticleHolder1 = new ParticleSystem[hitShots];
            missParticleHolder1 = new ParticleSystem[missedShots];
            // zaczecie animacji ostrzalu
            StartCoroutine(StartAnimation());
        }
    }

    void TakeDamage(int shipID)
    {
        switch(shipID)
        {
            case 0:
                pancernikComponent1.hp -= 1;
                if (pancernikComponent1.hp <= 0)
                {
                    pancernik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = pancernik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
            case 1:
                niszczycielComponent1.hp -= 1;
                if (niszczycielComponent1.hp <= 0)
                {
                    niszczyciel1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = niszczyciel1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
            case 2:
                ciezkiKrazownikComponent1.hp -= 1;
                if (ciezkiKrazownikComponent1.hp <= 0)
                {
                    ciezkiKrazownik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = ciezkiKrazownik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
            case 3:
                korwetaComponent1.hp -= 1;
                if (korwetaComponent1.hp <= 0)
                {
                    korweta1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = korweta1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
            case 4:
                lekkiKrazownikComponent1.hp -= 1;
                if (lekkiKrazownikComponent1.hp <= 0)
                {
                    lekkiKrazownik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = lekkiKrazownik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                }
                break;
        }
    }

    IEnumerator StartAnimation()
    {
        //Debug.Log("start");
        for (int i = 0; i < hitShots; i++)
        {
            particlePosition = Functions.instance.FieldToWorldPosition(hitFields[i]);
            hitParticleHolder1[i] = Instantiate(hitParticleHolder);
            hitParticleHolder1[i].transform.position = new Vector3 (particlePosition[0], 1.5f, particlePosition[1]);
            hitParticleHolder1[i].Play();
            yield return new WaitForSeconds(Random.Range(0.6f, 1.2f));
        }
        for (int i = 0; i < missedShots; i++)
        {
            particlePosition = Functions.instance.FieldToWorldPosition(missFields[i]);
            missParticleHolder1[i] = Instantiate(missParticleHolder);
            missParticleHolder1[i].transform.position = new Vector3 (particlePosition[0], 0, particlePosition[1]);
            missParticleHolder1[i].Play();
            yield return new WaitForSeconds(Random.Range(0.6f, 1.2f));
        }

        //Debug.Log("koniec");
        animationPlaying = false;
        if (shipsLeft <= 0)
        {
            gameState = 2;
            InGameUI.instance.gameOver();
            PlayerPrefs.SetInt("Loser", index);
            PlayerPrefs.SetInt("gameState1", gameState);
            PlayerPrefs.SetInt("gameState3", gameState);
            Debug.Log("KONIEC GRY");
        }
    }

    public void EndAnimationStarter()
    {
        animationPlaying = true;
        mapa1 = Instantiate(mapa);
        StartCoroutine(EndAnimation());
    }

    IEnumerator EndAnimation()
    {

        yield return new WaitForSeconds(2f);
        ScenesManager.instance.EndTurn();
    }

    // zapis hp statkow do player prefsow
    void SaveHP()
    {
        PlayerPrefs.SetInt("PancernikHP" + index, pancernikComponent1.hp);
        PlayerPrefs.SetInt("NiszczycielHP" + index, niszczycielComponent1.hp);
        PlayerPrefs.SetInt("CiezkiKrazownikHP" + index, ciezkiKrazownikComponent1.hp);
        PlayerPrefs.SetInt("KorwetaHP" + index, korwetaComponent1.hp);
        PlayerPrefs.SetInt("LekkiKrazownikHP" + index, lekkiKrazownikComponent1.hp);
        PlayerPrefs.SetInt("ShipsLeft" + index, shipsLeft);
    }
}
