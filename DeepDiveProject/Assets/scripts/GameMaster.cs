using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public TextMeshProUGUI FinishText;
    public timer timer;
    public int curentCheck;

    public List<GameObject> CheckPoints;
    public void Check()
    {
        curentCheck++;
        if (curentCheck == CheckPoints.Count)
        {
            Finish();
            ////GetComponent<timer>().Finish();
            //foreach (GameObject go in CheckPoints)
            //{
            //    go.SetActive(true);
            //}
        }
    }
    public void Finish()
    {
        curentCheck = 0;
        timer.counting = false;
        gameObject.GetComponent<InGameMenu>().Menu();
        FinishText.text = $"You finished in {timer.tTime}";
    }
}
