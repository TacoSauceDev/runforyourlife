using UnityEngine;
using Unity.Netcode;

public class CharacterController : NetworkBehaviour
{
    Rigidbody2D _rb;
   
    float speed = 15f;

    float maxSpeed = 5f;
    
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up  = KeyCode.W;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var tmpSpeed = Vector2.zero;

        if (Input.GetKey(left))
        {
            tmpSpeed = Vector2.left * Time.deltaTime * speed;
        }
        else if (Input.GetKey(right))
        {
            tmpSpeed = Vector2.right * Time.deltaTime * speed;
        }
        else if (Input.GetKey(up)){
            tmpSpeed = Vector2.up * Time.deltaTime * speed;
        }

        if (tmpSpeed != Vector2.zero)
        {
            _rb.AddForce(tmpSpeed, ForceMode2D.Impulse);

            
                _rb.velocity =  new Vector2(Mathf.Clamp(_rb.velocity.x, maxSpeed * -1, maxSpeed),
                 Mathf.Clamp(_rb.velocity.y,maxSpeed *-1, maxSpeed));
       
        }
    }

 
}