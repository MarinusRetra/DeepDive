using System.Collections;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    public bool RaceStarted = false;

    void Start()
    {
        StartCoroutine(nameof(OnRaceStart));
    }


    IEnumerator OnRaceStart()
    { 
      for (int i = 0; i < gameObject.transform.childCount; i++)
      { 
          yield return new WaitForSeconds(1);
          gameObject.transform.GetChild(i).GetChild(0).GetComponent<Light>().intensity = 1;
          gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red;
      }

      yield return new WaitForSeconds(Random.Range(0.1f, 3.01f));
      RaceStarted = true;

       


      for (int i = 0; i < gameObject.transform.childCount; i++)
      {
            gameObject.transform.GetChild(i).GetChild(0).GetComponent<Light>().intensity = 0;
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }
}
