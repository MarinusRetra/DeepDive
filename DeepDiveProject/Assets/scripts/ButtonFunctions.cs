using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject mainmenupanel;
    public GameObject levelselectpanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
        EditorSceneManager.LoadScene(1);

    }
    public void LevelSelect2()
    {

        EditorSceneManager.LoadScene(2);
    }
    public void MainMenu()
    {
        EditorSceneManager.LoadScene(0);
    }
}
