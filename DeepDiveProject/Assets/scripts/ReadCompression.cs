using System.Collections;
using UnityEngine;
using System;

public class ReadCompression : MonoBehaviour
{
    [SerializeField] public float[] currentCompressions = new float[4];
    private readonly WheelCollider[] WheelColliders = new WheelCollider[4];

    GameObject WheelCollidersObject;


    
    bool FoundCar = false;

    private void Start()
    {
        WheelCollidersObject = GameObject.Find("Wheel Colliders");

        if (FindCar())
        { 
            //Zet alle wheel colliders in een array
            for (int i = 0; i < WheelColliders.Length; i++)
            {
                WheelColliders[i] = WheelCollidersObject.transform.GetChild(i).GetComponent<WheelCollider>();
            }
        }
    }
            
    private void Update()
    {
        //Gaat door alle wheelcolliders heen en geeft je de huidige compression terug per wiel

        if (FoundCar)
        {
            for (int i = 0; i <  WheelCollidersObject.transform.childCount; i++) 
            {
                if (WheelColliders[i].GetGroundHit(out WheelHit hit))   
                {
                    currentCompressions[i] = 1 - hit.point.y / WheelColliders[i].suspensionDistance;
                    Debug.Log($"{WheelColliders[i].name} Compression: {currentCompressions[i]}" );
                }
            }
        }
    }

    IEnumerator CompareLastCompression(float suspensionIn)
    {
        yield return new WaitForEndOfFrame();
    }

    public bool FindCar()
    {
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            FoundCar = true;

            Debug.Log(FoundCar);
            return FoundCar;
        }
        else
        { 
            FoundCar = false;

            Debug.Log(FoundCar);
            return FoundCar;

        }


    }
    
}
