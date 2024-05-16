using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    public GameObject settingsPanel;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
    }
    public void Menu()
    {
        Debug.Log("menu");
        if (!settingsPanel.activeSelf)
        {
            Debug.Log("menu1");
            Time.timeScale = 0;
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.Log("menu2");
            Time.timeScale = 1;
            settingsPanel.SetActive(false);
        }
        
       

    }
}
