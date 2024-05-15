using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapHandler : MonoBehaviour
{
    public GameObject GameManager;
    public bool IsCheckpoint = false;

    [SerializeField] Collider CheckPoint;
    [SerializeField] Collider Finish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IsCheckpoint)
            {
                GameManager.GetComponent<RoundHandler>().CanFinishLap = true;
            }
            else
            {
                if (GameManager.GetComponent<RoundHandler>().CanFinishLap)
                {
                    GameManager.GetComponent<RoundHandler>().UpdateLap();
                }
            }
        }
    }
}
