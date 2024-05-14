using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public timer timer;
    public int curentCheck;
    public List<GameObject> CheckPoints;
    public void Check()
    {
        curentCheck++;
    }
    public void Finish()
    {
        if (curentCheck == CheckPoints.Count)
        {
            timer.Finish();
            curentCheck = 0;
            GetComponent<timer>().Finish();
            foreach (GameObject go in CheckPoints) 
            {
                go.SetActive(true);
            }
        }
    }
}
