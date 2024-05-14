using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class RoundHandler : MonoBehaviour
{
    [SerializeField] GameObject Vehicle1; // Main Vehicle
    [SerializeField] GameObject Vehicle2; // Secondary Vehicles
    [SerializeField] GameObject Vehicle3; // Secondary Vehicles
    [SerializeField] GameObject Vehicle4; // Secondary Vehicles

    [SerializeField] TextMeshProUGUI CountDown;
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] TextMeshProUGUI LapText;
    [SerializeField] TextMeshProUGUI FinalTime;

    [SerializeField] GameObject EndScreen;

    private float TimeElapsed = 0;
    public float PlayerLap = 0;
    private bool HasStarted = false;
    public bool CanFinishLap = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (HasStarted)
        {
            TimeElapsed += Time.deltaTime;
            
            float minutes = Mathf.FloorToInt(TimeElapsed / 60);
            float seconds = Mathf.FloorToInt(TimeElapsed % 60);
            Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateLap()
    {
        PlayerLap += 1;

        if (PlayerLap > 3)
        {
            EndScreen.SetActive(true);
            float minutes = Mathf.FloorToInt(TimeElapsed / 60);
            float seconds = Mathf.FloorToInt(TimeElapsed % 60);
            FinalTime.text = "The player Ryan has won the race with a time of: " + string.Format("{0:00}:{1:00}", minutes, seconds);

            Vehicle1.GetComponent<CarController>().enabled = false;
            Vehicle2.GetComponent<AiCarHandler>().enabled = false;
            Vehicle3.GetComponent<AiCarHandler>().enabled = false;
            Vehicle4.GetComponent<AiCarHandler>().enabled = false;
        }
        else
        {
            CanFinishLap = false;
            LapText.text = "Lap: " + PlayerLap + "/3";
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        print("Close");
        Application.Quit();
    }

    public IEnumerator StartCountdown()
    {
        CountDown.text = "3";
        yield return new WaitForSeconds(1);
        CountDown.text = "2";
        yield return new WaitForSeconds(1);
        CountDown.text = "1";
        yield return new WaitForSeconds(1);
        CountDown.text = "GO";
        yield return new WaitForSeconds(1);
        CountDown.text = "";

        Vehicle1.GetComponent<CarController>().enabled = true;
        Vehicle2.GetComponent<AiCarHandler>().enabled = true;
        Vehicle3.GetComponent<AiCarHandler>().enabled = true;
        Vehicle4.GetComponent<AiCarHandler>().enabled = true;

        HasStarted = true;
    }
}
