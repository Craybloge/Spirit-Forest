using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper_TurnAround : MonoBehaviour
{
    public bool needTurn = false; 

    void OnTriggerEnter2D(Collider2D Player)
    {
        if(Player.gameObject.tag =="Player")
        {
            needTurn = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D Player)
    {
        if(Player.gameObject.tag == "Player")
        {
            needTurn =false; 
        }
    }
}