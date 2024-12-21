using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FadeAway : MonoBehaviour
{
    private float alphaValue, fadeAwayPerSecond, fadeTime, waitTime;
    private int index;
    private TextMeshProUGUI fadeAwayText;

    // Start is called before the first frame update
    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (GameManagment.instance.gameState != 2)
        {
            fadeTime = 3.0f;
            waitTime = 2.0f;
        }
        else
        {
            fadeTime = 0.000000001f;
            waitTime = 0;
        }

        fadeAwayText = GetComponent<TextMeshProUGUI>();
        if (index == 1)
        {
            fadeAwayText.SetText("Player 1");
        }
        else
        {
            fadeAwayText.SetText("Player 2");
        }
        fadeAwayPerSecond = 1 / fadeTime;
        alphaValue = fadeAwayText.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else if (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphaValue);
        }
    }
}
