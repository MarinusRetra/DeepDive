using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject mainmenupanel;
    public GameObject levelselectpanel;
    public void Play()
    {
       mainmenupanel.SetActive(false);
       levelselectpanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LevelSelect1()
    {
        SceneManager.LoadScene(1);

    }
    public void LevelSelect2()
    {

        SceneManager.LoadScene(2);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
