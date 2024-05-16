using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    private int cam;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Alpha1))
       {
           Switch();
       }
    }

    public void Switch()
    {
        if(cam == 2)
        {
            cam = 0;
        }
        else
        {
            cam++;
        }
        switch(cam)
        {
            case 0:
                cam1.SetActive(true);
                cam2.SetActive(false);
                cam3.SetActive(false);
                break;
            case 1:
                cam2.SetActive(true);
                cam1.SetActive(false);
                cam3.SetActive(false);
                break; 
            case 2:
                cam3.SetActive(true);
                cam2.SetActive(false);
                cam1.SetActive(false);
                break;
        }

    }
}
