using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class timer : MonoBehaviour 
{
    public TextMeshProUGUI bestTimer;
    public TextMeshProUGUI Timer;
    public static bool counting = false;
    public float tTime;
    public float tTimeScore = 0;
    void Update() 
    {
        if (counting)
        { 
            tTime += Time.deltaTime; 
            Timer.text = tTime.ToString("00.00");
        }
    } 
    public void Finish() 
    { 
        if (tTime < tTimeScore || tTimeScore == 0) 
        { 
            tTimeScore = tTime; 
            bestTimer.text = tTime.ToString("best score: 00.00"); 
        }
        tTime = 0; 
    } 


}
