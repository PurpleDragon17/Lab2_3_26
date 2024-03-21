using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    //Code comes from Unity API 

    private CharacterController cntrllr;
    private float playerSpeed = 2.0f; 
    void Start()
    {
        cntrllr = gameObject.AddComponent<CharacterController>();
    }

   
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cntrllr.Move(move * Time.deltaTime * playerSpeed);

        if(move != Vector3.zero) {
            gameObject.transform.forward = move;
            //This makes a intretaing movement that I feel like makes the game more intretsting 
         }
    }
}
