﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject inventoryPanel;
    public GameObject pausePanel;
    public bool usingPausePanel;

    private void Start()
    {
        isPaused = false;
        inventoryPanel.SetActive(false);
        pausePanel.SetActive(false);
        usingPausePanel = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown("pause"))
        {
            switchPause();
        }
    }

    public void Resume()
    {
        switchPause();
    }

    public void Quit()
    {
        switchPause();
        SceneManager.LoadScene("StartMenu");
    }

    private void switchPause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            usingPausePanel = true;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            inventoryPanel.SetActive(false);
        }
    }

    public void SwitchPanels()
    {
        usingPausePanel = !usingPausePanel;
        if(usingPausePanel)
        {
            pausePanel.SetActive(true);
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
    }
}
