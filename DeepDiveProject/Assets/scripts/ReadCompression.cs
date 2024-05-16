using System;
using UnityEngine;
public class ReadCompression : MonoBehaviour
{
    /// <summary>
    /// In deze array kun je de compression uitlezen van alle wielen met Compressionsp[element].CompressionValue
    /// </summary>
    private readonly WheelCollider[] WheelColliders = new WheelCollider[4];
    public Compression[] Compressions = new Compression[4]{ new(0, "Wiel1") ,new(0, "Wiel2"), new(0, "Wiel3"), new(0, "Wiel4") }; // initialize de dingen in de array

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
                Compressions[i].SetName(WheelColliders[i].name);
            }
        }
    }
            
    private void Update()
    {
        //Gaat door alle wheelcolliders heen en geeft je de huidige compressie van alle wielen
        if (FoundCar)
        {
            for (int i = 0; i < WheelCollidersObject.transform.childCount; i++) 
            {
                if (WheelColliders[i].GetGroundHit(out WheelHit hit))   
                {
                    Compressions[i].SetCompression(Mathf.Clamp01(Math.Abs(1 - hit.point.y / WheelColliders[i].suspensionDistance)));
                }
            }
        }
    }

    public bool FindCar()
    {
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            FoundCar = true;
            return FoundCar;
        }
        else
        { 
            FoundCar = false;
            return FoundCar;
        }


    }
    
}
