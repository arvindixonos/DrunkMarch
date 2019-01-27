﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private Player player;
    private BoxCollider playerBoxCollider;

    private Parent mom;

    private Parent dad;

    private bool gameStarted = false;
    public bool GameStarted
    {
        get { return gameStarted; }
    }

    private bool gameOver = false;
    public bool GameOver
    {
        get { return gameOver; }
    }

    public  string[]    levelNames;

    public eLevel currentLevel = eLevel.LEVEL_1;

    public AudioSource bgmAudioSource;

    public Image bgImage;
    public Button playButton;
    public Button retryButton;

    public void FadeBGM(float fadeValue = 0.0f)
    {
        bgmAudioSource.DOKill();
        bgmAudioSource.DOFade(fadeValue, 1f);
    }

    void Start()
    {
        InitLevel();
    }

    public void InitLevel()
    {
        bgmAudioSource = GetComponent<AudioSource>();

        mom = GameObject.FindGameObjectWithTag("Mom").GetComponent<Parent>();
        dad = GameObject.FindGameObjectWithTag("Dad").GetComponent<Parent>();
        player = FindObjectOfType<Player>();

        playerBoxCollider = player.GetComponent<BoxCollider>();
        
        bgImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(false);

        FadeBGM(1f);

        if (currentLevel == eLevel.LEVEL_2)
        {
            StartGame();

            mom.EnableVision();
            dad.EnableVision();
        }
    }

    public  Bounds GetPlayerBounds()
    {
        return playerBoxCollider.bounds;
    }

    public  void    CaughtPlayer()
    {
        gameOver = true;

        dad.LookAtPosition(player.transform.position);
        dad.CaughtPlayer();
        
        mom.LookAtPosition(player.transform.position);
        mom.CaughtPlayer();

        player.LookAtPosition(dad.transform.position);
    }

    public void StartGame()
    {
        gameStarted = true;
        playButton.gameObject.SetActive(false);
        bgImage.gameObject.SetActive(false);

        FadeBGM(0.2f);
    }

    public void EndGame()
    {
        gameOver = true;
        bgImage.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    public void RetryClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelComplete()
    {
        currentLevel = currentLevel + 1;

        if (currentLevel < eLevel.LEVEL_TOTAL)
        {
            string levelName = levelNames[(int)currentLevel];

            SceneManager.LoadScene(levelName);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            GameObject.DestroyImmediate(this.gameObject);
    }
}
