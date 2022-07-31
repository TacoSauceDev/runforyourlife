using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody2D rb;
    public float speed = 2;
    public Animator animator;

    public float jumpForce = 5;
    public bool isGrounded;
    bool facingRight = true;

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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}
