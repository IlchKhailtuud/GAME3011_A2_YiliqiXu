using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    private float timer = 60f;
    private float timeElapsed;
    private int difficulty;

    public TMP_Text timerText;
    
    public static GameManager Instance
    {
        get => instance;
    }
    
    public int Difficulty
    {
        get => difficulty;
        set => difficulty = value;
    }
    
    private void Awake()
    {
        instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            CountDownTimer();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("lose");

        timerText.text = timer.ToString();
    }

    void CountDownTimer()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1)
        {
            timer--;
            timeElapsed = 0;
        }
    }
}
