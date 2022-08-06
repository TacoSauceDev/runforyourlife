using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody2D rb;
    public float speed = 1.5f;
    public Animator animator;

    public float jumpForce = 5;
    public bool isGrounded;
    bool facingRight = true;
    public bool isBouncing;
    bool onWall = false;
    bool wallJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn() {
        if(!IsOwner) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x != 0){
            animator.SetFloat("Speed",1);
        }
        if(!isGrounded){
            animator.SetBool("isJumping",true);
        }else{
            animator.SetBool("isJumping",false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed",0);
        }
        
        if (Input.GetKey(KeyCode.W) && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if(rb.velocity.x > 0 && !facingRight){
            Flip();
        }
        if(rb.velocity.x < 0 && facingRight){
            Flip();
        }
        if(onWall && Input.GetKey(KeyCode.W))
        {
            wallJump = true;
            Invoke("SetWallJumpFalse", (float)0.08);
        }
        if(wallJump){
            rb.velocity = new Vector2((rb.velocity.x * -1),jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = true;
            isGrounded = false;
            Physics.gravity = new Vector3(0,0,0);
        }
        //Added a new tag "Spike" so that if a player hits a spike, they bounce back
        if (collision.gameObject.CompareTag("Spike"))
        {
            if (isGrounded == true)
              {  float bounce = 1000f;
                rb.AddForce(collision.contacts[0].normal * bounce);
                isBouncing = true;
                Invoke("StopBounce",0.3f);
              }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = false;
            isGrounded = true;
            Physics.gravity = new Vector3(0,-9.8f,0);
        }
    }
    
    //This stops the bouncing after a player hits a spike
        void StopBounce()
    {
        isBouncing = false;
    }

    void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    void SetWallJumpFalse(){
        wallJump = false;
    }
}
