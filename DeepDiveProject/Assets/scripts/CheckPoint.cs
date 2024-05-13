using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameMaster GM;
    void Start()
    {
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GM.Check();
            gameObject.SetActive(false);
        }
    }
}
