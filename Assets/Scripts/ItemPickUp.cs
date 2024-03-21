using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    //This is the Game mechanic 

    public GameObject plar;

    private void Start()
    {
        plar = GameObject.FindGameObjectWithTag("player");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }

        
        
    }
    //I don't this this is doing anything, but it's here just to show how much this code bothered me 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("FUCK!");
       Destroy(gameObject);
    }
    //Code based on the sample code on Unity API 
    //Thanks to the formun people on Reddit who reminded me to give both compoants a ridgetd body 
}
