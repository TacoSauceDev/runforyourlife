using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
  
   //This is the starting health value of the player
   public int beginningHealth;
   // This is the player's current health that will change as they move through the game   
   int currentHealth;

   public HealthBar healthBar;

   //This happens BEFORE Start()
   void Awake() 
   {
      //initialize our current health to be equal to the starting health
      currentHealth = beginningHealth;
      healthBar.SetMaxHealth(currentHealth);
     
   }
   
   // Changes the players current health
   // and kill them if they have zero health
   public void ModHealth(int changeHealth)
   {
      //take our current health, add changeHealth and store the result back in the current health variable
      currentHealth += changeHealth;
      //keep our current health between zero and starting value
      currentHealth = Mathf.Clamp(currentHealth,0,beginningHealth);
      healthBar.SetHealth(currentHealth);
      //Debug.Log("Player's Health is " + currentHealth);

      // If health drops to zero, that means the player is dead, colder than a well diggers ass
      if(currentHealth == 0)
      {
         Kill();
      }

   }

    //Kills the player
   public void Kill()
   {
        // This will destroy the game object that this script is attached to
  
        //Destroy(gameObject);
        gameObject.SetActive(false);
   }


}
