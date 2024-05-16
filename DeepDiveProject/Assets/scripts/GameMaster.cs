using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject FinishText;
    public timer timer;
    public int curentCheck;
    [SerializeField] int FinishedPlayer = 0;

    public List<GameObject> CheckPoints;
    public void Check()
    {
        curentCheck++;
        if (curentCheck == CheckPoints.Count)
        {
            timer.Finish();
            curentCheck = 0;
            ////GetComponent<timer>().Finish();
            //foreach (GameObject go in CheckPoints)
            //{
            //    go.SetActive(true);
            //}
        }
    }
    public void Finish()
    {
        FinishedPlayer++;
        if (FinishedPlayer > 0)
        {
            timer.counting = false;
            gameObject.GetComponent<InGameMenu>().Menu();
            FinishText.GetComponent<TextMeshProUGUI>().text = $"You finished in {timer.tTime}";
        }
    }
}
