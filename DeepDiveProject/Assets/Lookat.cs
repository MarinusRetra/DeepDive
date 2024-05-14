using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    public Transform curentpoint;
    public GameMaster lookat;
    void Start()
    {
        lookat = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }
    void Update()
    {
        curentpoint = lookat.CheckPoints[lookat.curentCheck].transform;  
        gameObject.transform.LookAt(new Vector3(curentpoint.position.x, curentpoint.position.y, curentpoint.position.z));
    }
}
