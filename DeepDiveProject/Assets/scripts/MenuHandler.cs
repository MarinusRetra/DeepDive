using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject MapSelectionMenu;
    [SerializeField] private GameObject CarSelectionMenu;

    [SerializeField] private GameObject Vehicle1;
    [SerializeField] private GameObject Vehicle2;
    [SerializeField] private GameObject Vehicle3;
    [SerializeField] private GameObject Vehicle4;

    public void BackToMainmenu()
    {
        MainMenu.SetActive(true);
        MapSelectionMenu.SetActive(false);
        CarSelectionMenu.SetActive(false);   
    }

    public void ChooseMap1()
    {
        SceneManager.LoadScene(1);
    }

    public void ChooseMap2()
    {
        SceneManager.LoadScene(2);
    }

    public void VehicleSelection()
    {
        MainMenu.SetActive(false);
        CarSelectionMenu.SetActive(true);
        MapSelectionMenu.SetActive(false);
    }

    public void PlayGame()
    {
        MainMenu.SetActive(false);
        CarSelectionMenu.SetActive(false);
        MapSelectionMenu.SetActive(true);
    }

    public void EndGame()
    {
        Debug.Log("Player has quit the game");
        Application.Quit();
    }
}
