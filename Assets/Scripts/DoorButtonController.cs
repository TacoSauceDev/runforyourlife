using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    public GameObject[] obstacles;


    // Start is called before the first frame update
    void Start()
    {
        if(obstacles.Length > 0){
            for(int i = 0; i < obstacles.Length; i++){
                    obstacles[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.CompareTag("Player")){
            
            for(int i = 0; i < obstacles.Length; i++){

                obstacles[i].SetActive(false);
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")){

            for(int i = 0; i < obstacles.Length; i++){
                obstacles[i].SetActive(true);
            }
        }
    }
}
