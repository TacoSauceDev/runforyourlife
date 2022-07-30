using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D _rb;
   
    float speed = 15f;

    float maxSpeed = 5f;
    
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;

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

        if (tmpSpeed != Vector2.zero)
        {
            _rb.AddForce(tmpSpeed, ForceMode2D.Impulse);

            // This checks that the player is not above the max speed
            if (Mathf.Abs(_rb.velocity.x) > maxSpeed)
            {
                var tmp = _rb.velocity;
                // Clamp the player to the maximum speed allowed
                // Taking care to ensure that the speed is in the right
                // direction; -1 is left, 1 is right
                tmp.x = Mathf.Sign(_rb.velocity.x) * maxSpeed;
                _rb.velocity = tmp;
            }
        }
    }

 
}