using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManagment : MonoBehaviour
{
    public static GameManagment instance;

    public int gameState = 0, shipsNumber = 5, maxSize = 5, shipsLeft = 5;                              // gameState - faza gry, shipsNumber - ilosc statkow, maxSize - maksymalny rozmiar statku, shipsLeft - ile statkow pozostalo
    private int index, hitShots = 0, missedShots = 0, tmp, hitFieldsIndex = 0, missFieldsIndex = 0, isHit, znacznikiArrayIndex;     
                                                                                                        // index - indeks sceny, hitShots - ilosc trafien, missedShots - iosc strzalow nietrafionych, tmp - zmienna pomocnicza
                                                                                                        // przy sprawdzaniu trafienia, hitFieldsIndex - indeks zapisu do tablicy hitFields, missFieldsIndex - indeks zapisu do
                                                                                                        // tablicy missFields, isHit - czy pancernik zostal trafiony
    public string[,] occupiedFields, enemyOccupiedFields, attackFields, fieldsUnderAttack, destroyedFields, enemyDestroyedFields, abilityFields, fieldsUnderAbility;
                                                                                                        // occupiedFields - pola zajmowanie przez statki, attackFields - pola ktore gracz chce atakowac, fieldsUnderAttack -
                                                                                                        // pola gracza atakowane przez przeciwnika, destroyedFields - pola zajmowane przez zniszczone statki
    private string field, destroyedFieldsString;
    private string[] hitFields, missFields;
    private float[] particlePosition;
    public bool animationPlaying;
    private bool torpedoHit;

    public GameObject pancernik, niszczyciel, ciezkiKrazownik, korweta, lekkiKrazownik, mapa;
    public ParticleSystem hitParticleHolder, missParticleHolder;
    private GameObject pancernik1, niszczyciel1, ciezkiKrazownik1, korweta1, lekkiKrazownik1, mapa1;
    private ParticleSystem[] hitParticleHolder1, missParticleHolder1, attackHitParticle1, attackMissParticle1;
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
        znacznikiArrayIndex = 0;
        animationPlaying = true;
        hitFields = new string[shipsNumber + 10];
        missFields = new string[shipsNumber + 10];

        attackFields = new string[shipsNumber, 1];
        occupiedFields = new string[shipsNumber, maxSize];
        destroyedFields = new string[shipsNumber, maxSize];
        enemyDestroyedFields = new string[shipsNumber, maxSize];
        abilityFields = new string[2, 5];

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

            PlayerPrefs.SetString("AttackedFields" + index, " ");
            SaveHP();
        }
        else if (gameState == 1)
        {
            // wczytanie pozycji statkow oraz sprawdzenie czy statki otrzymaly obrazenia
            occupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions" + index), shipsNumber, maxSize);
            destroyedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("DestroyedFields" + index), shipsNumber, maxSize);
            shipsLeft = PlayerPrefs.GetInt("ShipsLeft" + index);

            pancernikComponent1.hp = PlayerPrefs.GetInt("PancernikHP" + index);
            if (pancernikComponent1.hp <= 0)
            {
                pancernik1.transform.tag = "Destroyed";
                var renderer = pancernik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            niszczycielComponent1.hp = PlayerPrefs.GetInt("NiszczycielHP" + index);
            if (niszczycielComponent1.hp <= 0)
            {
                niszczyciel1.transform.tag = "Destroyed";
                var renderer = niszczyciel1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            ciezkiKrazownikComponent1.hp = PlayerPrefs.GetInt("CiezkiKrazownikHP" + index);
            if (ciezkiKrazownikComponent1.hp <= 0)
            {
                ciezkiKrazownik1.transform.tag = "Destroyed";
                var renderer = ciezkiKrazownik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            korwetaComponent1.hp = PlayerPrefs.GetInt("KorwetaHP" + index);
            if (korwetaComponent1.hp <= 0)
            {
                korweta1.transform.tag = "Destroyed";
                var renderer = korweta1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }
            lekkiKrazownikComponent1.hp = PlayerPrefs.GetInt("LekkiKrazownikHP" + index);
            if (lekkiKrazownikComponent1.hp <= 0)
            {
                lekkiKrazownik1.transform.tag = "Destroyed";
                var renderer = lekkiKrazownik1.GetComponent<Renderer>();
                renderer.material = zniszczony;
            }

            // wczytanie pol ktore sa atakowane przez przeciwnika
            if (index == 1)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields3"), shipsNumber, 1);
                fieldsUnderAbility = Functions.instance.StringToArray(PlayerPrefs.GetString("AbilityFields3"), 2, 5);
                enemyDestroyedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("DestroyedFields3"), shipsNumber, maxSize);
            }
            else if (index == 3)
            {
                fieldsUnderAttack = Functions.instance.StringToArray(PlayerPrefs.GetString("AttackedFields1"), shipsNumber, 1);
                fieldsUnderAbility = Functions.instance.StringToArray(PlayerPrefs.GetString("AbilityFields1"), 2, 5);
                enemyDestroyedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("DestroyedFields1"), shipsNumber, maxSize);

            }

            // sprawdzenie czy przeciwnik trafil, przechodzimy po wszystkich atakowanych polach
            for (int k = 0; k < shipsNumber; k++)
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
                            //Debug.Log("normalne ataki trafiony: " + znacznikiArrayIndex);
                            tmp = 1;
                        }
                    }
                }
                if (tmp == 0 && field != "")
                {
                    PlayerPrefs.SetInt("Znacznik2" + index + k, 0);
                    //Debug.Log("normalne ataki pudlo: " + znacznikiArrayIndex);
                    missedShots++;
                    missFields[missFieldsIndex++] = field;
                }
            }
            for (int m = 0; m < 2; m++)
            {
                for (int k = 0; k < 5; k++)
                {
                    field = fieldsUnderAbility[m, k];
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
                                if (m == 0)
                                    PlayerPrefs.SetInt("Znacznik2" + index + (shipsNumber + m + k), 1);
                                else
                                {
                                    PlayerPrefs.SetInt("Znacznik2" + index + (shipsNumber + m + k + 2), 1);
                                }
                                znacznikiArrayIndex++;
                                tmp = 1;
                            }
                        }
                    }
                    if (tmp == 0 && field != "")
                    {
                        if (m == 0)
                            PlayerPrefs.SetInt("Znacznik2" + index + (shipsNumber + m + k), 0);
                        else
                        {
                            PlayerPrefs.SetInt("Znacznik2" + index + (shipsNumber + m + k + 2), 0);
                        }
                        znacznikiArrayIndex++;
                        missedShots++;
                        missFields[missFieldsIndex++] = field;
                    }
                }
            }
            // zapis hp statkow
            SaveHP();
            destroyedFieldsString = Functions.instance.ArrayToString(destroyedFields, shipsNumber, maxSize);
            PlayerPrefs.SetString("DestroyedFields" + index, destroyedFieldsString);
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
                isHit = Random.Range(0, 100);
                if (isHit < 70 && pancernikComponent1.hp > 0)
                {
                    pancernikComponent1.hp -= 1;
                }
                if (pancernikComponent1.hp == 0)
                {
                    pancernik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = pancernik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                    for (int i = 0; i < maxSize; i++)
                    {
                        destroyedFields[0, i] = occupiedFields[0, i];
                    }
                }
                break;
            case 1:
                if (niszczycielComponent1.hp > 0)
                {
                    niszczycielComponent1.hp -= 1;
                }
                if (niszczycielComponent1.hp == 0)
                {
                    niszczyciel1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = niszczyciel1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                    for (int i = 0; i < maxSize; i++)
                    {
                        destroyedFields[1, i] = occupiedFields[1, i];
                    }
                }
                break;
            case 2:
                if (ciezkiKrazownikComponent1.hp > 0)
                {
                    ciezkiKrazownikComponent1.hp -= 1;
                }
                if (ciezkiKrazownikComponent1.hp == 0)
                {
                    ciezkiKrazownik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = ciezkiKrazownik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                    for (int i = 0; i < maxSize; i++)
                    {
                        destroyedFields[2, i] = occupiedFields[2, i];
                    }
                }
                break;
            case 3:
                if (korwetaComponent1.hp > 0)
                {
                    korwetaComponent1.hp -= 1;
                }
                if (korwetaComponent1.hp == 0)
                {
                    korweta1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = korweta1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                    for (int i = 0; i < maxSize; i++)
                    {
                        destroyedFields[3, i] = occupiedFields[3, i];
                    }
                }
                break;
            case 4:
                if (lekkiKrazownikComponent1.hp > 0)
                {
                    lekkiKrazownikComponent1.hp -= 1;
                }
                if (lekkiKrazownikComponent1.hp == 0)
                {
                    lekkiKrazownik1.transform.tag = "Destroyed";
                    shipsLeft -= 1;
                    var renderer = lekkiKrazownik1.GetComponent<Renderer>();
                    renderer.material = zniszczony;
                    for (int i = 0; i < maxSize; i++)
                    {
                        destroyedFields[4, i] = occupiedFields[4, i];
                    }
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
            hitParticleHolder1[i].transform.position = new Vector3 (particlePosition[0], 1f, particlePosition[1]);
            hitParticleHolder1[i].Play();
            Debug.Log("a");
            yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
        }
        for (int i = 0; i < missedShots; i++)
        {
            particlePosition = Functions.instance.FieldToWorldPosition(missFields[i]);
            missParticleHolder1[i] = Instantiate(missParticleHolder);
            missParticleHolder1[i].transform.position = new Vector3 (particlePosition[0], 0f, particlePosition[1]);
            missParticleHolder1[i].Play();
            yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
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
        torpedoHit = false;
        mapa1 = Instantiate(mapa);
        attackHitParticle1 = new ParticleSystem[shipsNumber + 10];
        attackMissParticle1 = new ParticleSystem[shipsNumber + 10];
        if (index == 1)
            enemyOccupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions3"), shipsNumber, maxSize);
        else
            enemyOccupiedFields = Functions.instance.StringToArray(PlayerPrefs.GetString("Positions1"), shipsNumber, maxSize);
        if (pancernikComponent1.abilityUsed == true)
        {
            PlayerPrefs.SetInt("PancernikCooldown" + index, 3);
        }
        else
        {
            PlayerPrefs.SetInt("PancernikCooldown" + index, pancernikComponent1.cooldown);
        }
        
        if (lekkiKrazownikComponent1.abilityUsed == true)
        {
            PlayerPrefs.SetInt("LekkiKrazownikCooldown" + index, 5);
        }
        else
        {
            PlayerPrefs.SetInt("LekkiKrazownikCooldown" + index, lekkiKrazownikComponent1.cooldown);
        }
        StartCoroutine(EndAnimation());
    }

    IEnumerator EndAnimation()
    {
        for (int i = 0; i < attackFields.Length; i++)
        {
            field = attackFields[i, 0];
            tmp = 0;

            if (field != "" && field != null)
            {
                for (int j = 0; j < shipsNumber; j++)
                {
                    for (int k = 0; k < maxSize; k++)
                    {
                        if (field == enemyOccupiedFields[j, k])
                        {
                            tmp = 1;
                            particlePosition = Functions.instance.FieldToWorldPosition(field);
                            attackHitParticle1[i] = Instantiate(hitParticleHolder);
                            attackHitParticle1[i].transform.position = new Vector3(particlePosition[0], 1.7f, particlePosition[1]);
                            attackHitParticle1[i].Play();
                            yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
                        }
                    }
                }
                if (tmp == 0)
                {
                    particlePosition = Functions.instance.FieldToWorldPosition(field);
                    attackMissParticle1[i] = Instantiate(missParticleHolder);
                    
                    attackMissParticle1[i].transform.position = new Vector3(particlePosition[0], 1.7f, particlePosition[1]);
                    attackMissParticle1[i].Play();
                    yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
                }
            }
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                field = abilityFields[i, j];
                tmp = 0;
                if (field == null || field == "" || torpedoHit)
                {
                    abilityFields[i, j] = null;
                    continue;
                }
                else
                {
                    for (int m = 0; m < shipsNumber; m++)
                    {
                        for (int n = 0; n < maxSize; n++)
                        {
                            if (field == enemyOccupiedFields[m, n])
                            {
                                tmp = 1;
                                particlePosition = Functions.instance.FieldToWorldPosition(field);
                                attackHitParticle1[shipsNumber + i + j] = Instantiate(hitParticleHolder);
                                attackHitParticle1[shipsNumber + i + j].transform.position = new Vector3(particlePosition[0], 2.5f, particlePosition[1]);
                                attackHitParticle1[shipsNumber + i + j].Play();
                                if (i == 1)
                                    torpedoHit = true;
                                yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
                            }
                        }
                    }
                    if (tmp == 0)
                    {
                        particlePosition = Functions.instance.FieldToWorldPosition(field);
                        attackMissParticle1[shipsNumber + i + j] = Instantiate(missParticleHolder);
                        attackMissParticle1[shipsNumber + i + j].transform.position = new Vector3(particlePosition[0], 2.5f, particlePosition[1]);
                        attackMissParticle1[shipsNumber + i + j].Play();
                        yield return new WaitForSeconds(Random.Range(0.6f, 1.3f));
                    }
                }
            }

        }
        yield return new WaitForSeconds(1f);
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
