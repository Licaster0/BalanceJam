using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseMenuUI;

    void Start()
    {
        SwitchTo(inGameUI);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(pauseMenuUI);
        }
    }
    public void SwitchTo(GameObject _menu)
    {
        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        if (_menu == inGameUI)
        {
            PauseGame(false);
        }
        else
        {
            PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        /*
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }
        */
        SwitchTo(inGameUI);
    }
    public void MainScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
