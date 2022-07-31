using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    //This is the amount of damage the hazard does
    public int damage;


  
    //This function will be called when another object bumps
    //into the one this script is attached to
    void OnCollisionEnter2D(Collision2D collisionData) 
    {
        //Get object we collided with
        Collider2D objectWeCollidedWith = collisionData.collider;

        //Get PlayerHeather script attached to that object (if there is one)
        PlayerHealth player = objectWeCollidedWith.GetComponent<PlayerHealth>();

        //Check if we actually found a health script on the object we collided with
        //This if statement is true if the player variable is NOT null (aka empty)        
        if (player != null)
        {
            //This means there was a playerhealth script attached to the object we bumped into
            //Which means this object is indeed a player
            //Therefore perform our action
            player.ModHealth(-damage);

        }
    
    }


}
