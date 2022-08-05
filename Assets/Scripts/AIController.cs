using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject player; //the player object
    private Transform playerPosition;
    private Vector2 currentPostion;
    public float distance;
    public float speedEnemy;
    public PlayerHealth playerhealth;
    public Animator aiAnimator;
    public int enemyDamage;

    // Start is called before the first frame update
    void Start()
    {
        // Use this for initialization
        playerPosition = player.GetComponent<Transform>();
        currentPostion = GetComponent<Transform>().position;
        aiAnimator = GetComponent<Animator>();
        playerhealth = GetComponent<PlayerHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Vector2.Distance(transform.position, playerPosition.position) < distance )
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, speedEnemy * Time.deltaTime);
            //aiAnimator.setBool("run", true);
        }
        else
        {
            if( Vector2.Distance(transform.position, currentPostion) <= 0 )
            {
                //aiAnimator.setBool("run", false);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, currentPostion, speedEnemy * Time.deltaTime);
            }
        }
    }

    private void OnCollision2DEnter(BoxCollider2D collision)
    {
        BoxCollider2D enemyAttack = collision.GetComponent<BoxCollider2D>();
        PlayerHealth player = enemyAttack.GetComponent<PlayerHealth>();

        if(player != null)
        {
            playerhealth.ModHealth(-enemyDamage);
        }
    }
}
